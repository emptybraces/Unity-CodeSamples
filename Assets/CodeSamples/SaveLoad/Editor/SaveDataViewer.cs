using UnityEngine;

namespace EmptyBraces.Editor
{
	[CreateAssetMenu(menuName = "CustomMenu/" + nameof(SaveDataViewer))]
	public class SaveDataViewer : ScriptableObject
	{
		public SaveData Data;

		[ContextMenu("Load")]
		void Load()
		{
			Data = SaveLoad.Load();
		}
		[ContextMenu("Save")]
		void Save()
		{
			SaveLoad.Save(Data);
		}
		[ContextMenu("New")]
		void New()
		{
			Data = new SaveData();
			Data.Init();
			Save();
		}
	}
}