using UnityEngine;
using System.IO;
using UnityEngine.Assertions;

namespace Emptybraces
{
	public static partial class SaveLoad
	{
		public static SaveData Data => _data ??= Load();
		static SaveData _data;

#if UNITY_EDITOR
		public static string k_PathSaveData => Application.dataPath + "/CodeSamples/SaveLoad/save.dat";
#else
		public static string k_PathSaveData => Application.persistentDataPath + "/save.dat";
#endif

		public static void Save(SaveData data = null)
		{
			Assert.IsTrue(data != null);
			var dirpath = Path.GetDirectoryName(k_PathSaveData);
			// ディレクトリを作成する
			if (!Directory.Exists(dirpath))
				Directory.CreateDirectory(dirpath);
			// セーブする
			data ??= _data;
			++data.SaveCount;
			data.Version = Application.version;
			File.WriteAllText(k_PathSaveData, JsonUtility.ToJson(data, true));
#if UNITY_EDITOR
			UnityEditor.AssetDatabase.Refresh();
#endif
			Debug.Log("データをセーブしました");
		}

		public static SaveData Load()
		{
			try
			{
				if (!File.Exists(k_PathSaveData))
				{
					var new_data = new SaveData();
					new_data.Init();
					Save(new_data);
					return new_data;
				}
				return JsonUtility.FromJson<SaveData>(File.ReadAllText(k_PathSaveData));
			}
			catch (System.Exception e)
			{
				Debug.Log("error: " + e.Message);
			}
			return null;
		}

	}
}