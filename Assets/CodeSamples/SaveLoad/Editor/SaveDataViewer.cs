using UnityEngine;

namespace Emptybraces.Editor
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
			Save();
		}
		[ContextMenu("Add SerializeReference Data/TestDataClass1")]
		void Add_TestDataClass1()
		{
			Data.AddPolymList<TestDataClass1>();
		}
		[ContextMenu("Add SerializeReference Data/TestDataClass2")]
		void Add_TestDataClass2()
		{
			Data.AddPolymList<TestDataClass2>();
		}
	}
}
