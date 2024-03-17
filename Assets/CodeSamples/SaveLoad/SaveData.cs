using System.Collections.Generic;
using UnityEngine;

namespace Emptybraces
{
	[System.Serializable]
	public class SaveData
	{
		public string Version;
		public int SaveCount;
		public List<ProjectileData> ProjectileDataList = new();
		public List<TestDataAbstractClass> TestDataAbstractList = new(); // 表示されない。
		public List<TestDataBaseClass> TestDataDerivedList = new(); // 親クラスでインスタンス化
		[SerializeReference] public List<TestDataBaseClass> TestDataPolymorphismList = new();

		public void Init()
		{
			Version = Application.version;
			TestDataDerivedList.Add(new TestDataDerivedClass());
			TestDataPolymorphismList.Add(new TestDataDerivedClass());
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
	public abstract class TestDataAbstractClass
	{
		public int N;
	}
	[System.Serializable]
	public class TestDataBaseClass
	{
		public int N;
	}
	[System.Serializable]
	public class TestDataDerivedClass : TestDataBaseClass
	{
		public int NN;
	}
}
