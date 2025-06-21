using System.Collections.Generic;
using UnityEngine;

namespace Emptybraces
{
	[System.Serializable]
	public class SaveData
	{
		public string Version;
		public int SaveCount;
		public string OverrideBindingJson;
		public List<ProjectileData> ProjectileDataList = new();
		[SerializeReference] public List<TestDataBase> TestDataList = new();

		public void Init()
		{
			Version = Application.version;
		}
		public void AddPolymList<T>() where T : TestDataBase, new()
		{
			TestDataList.Add(new T());
		}
	}

	[System.Serializable]
	public class ProjectileData
	{
		public string AASKey;
		public Vector3 Position;
		public Vector3 Rotation;
		public Vector3 Velocity;
		public Vector3 AngularVelocity;
	}

	[System.Serializable]
	public abstract class TestDataBase
	{
		public int BaseData;
	}
	[System.Serializable]
	public class TestDataClass1 : TestDataBase
	{
		public int Child1Data;
	}
	[System.Serializable]
	public class TestDataClass2 : TestDataBase
	{
		public int Child2_Data;
	}
}
