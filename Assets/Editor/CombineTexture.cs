using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
namespace Editor
{
	public class TextureCombiner : EditorWindow
	{
		[SerializeField] int spacing = 4;
		[SerializeField] int outputWidth = 1024;
		[SerializeField] int outputHeight = 1024;
		[SerializeField] List<Texture2D> texturesToCombine = new List<Texture2D>();
		Texture2D previewTexture;
		Vector2 scroll;
		Material alphaMaterial;


		[MenuItem("Window/Combine Selected Textures")]
		static void _showWindow()
		{
			var window = GetWindow<TextureCombiner>(nameof(TextureCombiner));
			window.minSize = new Vector2(500, 400);
		}

		void OnDisable()
		{
			if (alphaMaterial != null)
			{
				DestroyImmediate(alphaMaterial);
				alphaMaterial = null;
			}
		}

		void OnGUI()
		{
			EditorGUILayout.LabelField("Texture Grid Combiner", EditorStyles.boldLabel);
			spacing = EditorGUILayout.IntField("Spacing (pixels):", spacing);
			using (_ = new EditorGUILayout.HorizontalScope())
			{
				outputWidth = EditorGUILayout.IntField("Output Image Width (px):", outputWidth);
				outputHeight = EditorGUILayout.IntField("Output Image Height (px):", outputHeight);
			}
			EditorGUILayout.Space();
			SerializedObject so = new SerializedObject(this);
			SerializedProperty texturesProp = so.FindProperty("texturesToCombine");
			EditorGUILayout.PropertyField(texturesProp, true);
			so.ApplyModifiedProperties();

			GUI.enabled = texturesToCombine.Count > 0;
			if (GUILayout.Button("Combine Textures"))
			{
				_CombineTexture();
			}

			EditorGUILayout.Space(20);
			if (previewTexture != null)
			{
				EditorGUILayout.LabelField("Preview:", EditorStyles.boldLabel);

				scroll = EditorGUILayout.BeginScrollView(scroll, GUILayout.Height(200));
				float aspect = (float)previewTexture.width / previewTexture.height;
				float width = EditorGUIUtility.currentViewWidth - 40;
				float height = width / aspect;
				Rect rect = GUILayoutUtility.GetRect(width, height);
				EditorGUI.DrawPreviewTexture(rect, previewTexture, GetAlphaBlendMaterial());
				EditorGUILayout.EndScrollView();
			}
		}

		Material GetAlphaBlendMaterial()
		{
			if (alphaMaterial == null)
			{
				Shader shader = Shader.Find("UI/Default"); // 透過対応の定番
				if (shader != null)
				{
					alphaMaterial = new Material(shader);
				}
			}
			return alphaMaterial;
		}
		void _CombineTexture()
		{
			if (texturesToCombine == null || texturesToCombine.Count == 0)
			{
				Debug.LogWarning("No textures assigned.");
				return;
			}

			Texture2D combined = new Texture2D(outputWidth, outputHeight, TextureFormat.RGBA32, false);
			Color32[] clearPixels = new Color32[outputWidth * outputHeight];
			for (int i = 0; i < clearPixels.Length; i++)
				clearPixels[i] = new Color32(0, 0, 0, 0);
			combined.SetPixels32(clearPixels);

			int offsetX = 0;
			int offsetY = 0;
			int currentRowMaxHeight = 0;

			foreach (var tex in texturesToCombine)
			{
				if (tex == null) continue;

				string path = AssetDatabase.GetAssetPath(tex);
				Texture2D readableTex = GetReadableTexture(tex, path);
				if (readableTex == null) continue;

				int texWidth = readableTex.width;
				int texHeight = readableTex.height;

				// 行の幅を超える場合は折り返し
				if (offsetX + texWidth > outputWidth)
				{
					offsetX = 0;
					offsetY += currentRowMaxHeight + spacing;
					currentRowMaxHeight = 0;
				}

				// 縦の制限を超えるなら結合中断
				if (offsetY + texHeight > outputHeight)
				{
					Debug.LogWarning($"Texture placement exceeds output image height. Stopping at texture: {tex.name}");
					break;
				}

				// 貼り付け
				Color[] pixels = readableTex.GetPixels();
				combined.SetPixels(offsetX, offsetY, texWidth, texHeight, pixels);

				offsetX += texWidth + spacing;
				currentRowMaxHeight = Mathf.Max(currentRowMaxHeight, texHeight);
			}

			combined.Apply();
			// 保存
			string savePath = "Assets/Combined.png";
			File.WriteAllBytes(savePath, combined.EncodeToPNG());
			AssetDatabase.Refresh();

			Debug.Log($"Combined texture saved to {savePath}");

			// アセットとしてロードして表示
			previewTexture = AssetDatabase.LoadAssetAtPath<Texture2D>(savePath);
		}

		Texture2D GetReadableTexture(Texture2D original, string path)
		{
			TextureImporter importer = AssetImporter.GetAtPath(path) as TextureImporter;
			if (importer != null && !importer.isReadable)
			{
				importer.isReadable = true;
				importer.textureCompression = TextureImporterCompression.Uncompressed;
				importer.SaveAndReimport();
			}

			Texture2D readableCopy = new Texture2D(original.width, original.height, TextureFormat.RGBA32, false);
			Graphics.CopyTexture(original, readableCopy);
			return readableCopy;
		}
	}
}