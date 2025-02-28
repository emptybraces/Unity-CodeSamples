using System;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System.IO;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;

namespace Emptybraces.TextFileParserSample
{
	public static class CustomFormat
	{
		static string[] k_Split = new[] { "\t", " ", ", " };
		static string k_SectionID = "#";
		public static Dictionary<string, ActionBase[]> Data = new();
		static List<string> _tmpStringList = new(128);
		public static async UniTask<bool> LoadFromFile(string path, Transform parent)
		{
			try
			{
				string text;
				using (var stream = new StreamReader(path, System.Text.Encoding.UTF8))
				{
					text = await stream.ReadToEndAsync();
				}
				if (string.IsNullOrEmpty(text))
				{
					Debug.LogError("Failed read." + path);
					return false;
				}
				LoadFromText(text, parent);
				cn.log("Complete read.");
			}
			catch (Exception e)
			{
				Debug.LogError("Exception: " + e.Message);
				return false;
			}
			return true;
		}

		public static void LoadFromText(string textData, Transform parent)
		{
			foreach (var i in Data.Values)
				foreach (var ii in i)
					ii.OnDestroy();
			Data.Clear();
			_tmpStringList.Clear();
			int line_no = 0;
			string last_key = "";
			var lines = textData.Trim().Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
			var tmp = new List<ActionBase>();
			foreach (var line in lines)
			{
				++line_no;
				var trimmed_line = line.Trim();
				if (line.StartsWith("/", StringComparison.Ordinal) || trimmed_line == "")
					continue;
				// セクションを検出
				if (trimmed_line.StartsWith(k_SectionID, StringComparison.Ordinal))
				{
					// 前のセクションを保存
					__AddActions(last_key, tmp);
					last_key = trimmed_line.Replace(k_SectionID, "").Trim();
					continue;
				}
				// セクション内のデータを検出
				var splits = trimmed_line.Split(k_Split, StringSplitOptions.RemoveEmptyEntries);
				if (0 == splits.Length)
				{
					cn.loge("found empty line.", line, line_no);
					continue;
				}
				switch (splits[0])
				{
					case "Action1": tmp.Add(new Action1(parent, splits)); break;
					case "Action2": tmp.Add(new Action2(splits)); break;
				}
			}
			// 前のセクションを保存
			__AddActions(last_key, tmp);
			static void __AddActions(string key, List<ActionBase> tmp)
			{
				if (0 < tmp.Count)
				{
					Data[key] = tmp.ToArray();
					tmp.Clear();
				}
			}
		}

		public abstract class ActionBase
		{
			public abstract void Run();
			public virtual void Update() { }
			public virtual void OnDestroy() { }
		}
		public class Action1 : ActionBase
		{
			string _objID;
			Vector2 _pos;
			Vector2? _posTo;
			float _speed;
			RectTransform _rt;
			Transform _parent;
			public Action1(Transform parent, string[] args)
			{
				_parent = parent;
				int idx = 1;
				_objID = args[idx++];
				int.TryParse(args[idx++].Replace("(", ""), System.Globalization.NumberStyles.Integer, System.Globalization.CultureInfo.InvariantCulture, out int x);
				int.TryParse(args[idx++].Replace(")", ""), System.Globalization.NumberStyles.Integer, System.Globalization.CultureInfo.InvariantCulture, out int y);
				_pos = new Vector2(x, y);
				if (idx < args.Length)
				{
					int.TryParse(args[idx++].Replace("(", ""), System.Globalization.NumberStyles.Integer, System.Globalization.CultureInfo.InvariantCulture, out x);
					int.TryParse(args[idx++].Replace(")", ""), System.Globalization.NumberStyles.Integer, System.Globalization.CultureInfo.InvariantCulture, out y);
					_posTo = new Vector2(x, y);
					float.TryParse(args[idx++], System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out _speed);
				}
			}
			public override void Run()
			{
				if (_rt != null)
					return;
				var g = Addressables.InstantiateAsync(_objID, _parent);
				g.WaitForCompletion();
				_rt = (RectTransform)g.Result.transform;
				_rt.anchoredPosition = _pos;
			}
			public override void Update()
			{
				if (_rt == null || _posTo == null)
					return;
				var t = (Mathf.Sin(Time.time * _speed) + 1) / 2;
				_rt.anchoredPosition = Vector2.Lerp(_pos, _posTo.Value, t);
			}
			public override void OnDestroy()
			{
				if (_rt != null)
					Addressables.ReleaseInstance(_rt.gameObject);
			}
		}
		public class Action2 : ActionBase
		{
			string _tag;
			string _contents;
			public Action2(string[] args)
			{
				int idx = 1;
				_tag = args[idx++];
				_contents = args[idx];
			}
			public override void Run()
			{
				if (GameObject.FindWithTag(_tag).TryGetComponent<Text>(out var c))
					c.text = _contents;
			}
		}

#if UNITY_EDITOR
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		static void _DomainReset()
		{
			Data.Clear();
		}
#endif
	}
}
