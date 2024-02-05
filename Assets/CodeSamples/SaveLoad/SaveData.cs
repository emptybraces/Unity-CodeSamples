using System.Collections.Generic;
using UnityEngine;

namespace EmptyBraces
{
	[System.Serializable]
	public class SaveData
	{
		public string Version;
		public int SaveCount;
		public List<ProjectileData> ProjectileDataList = new();

		public void Init()
		{
			Version = Application.version;
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
}