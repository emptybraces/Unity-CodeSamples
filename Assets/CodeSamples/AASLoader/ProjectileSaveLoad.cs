using UnityEngine;

namespace Emptybraces
{
	[RequireComponent(typeof(Projectile))]
	public class ProjectileSaveLoad : MonoBehaviour
	{
		Projectile _projectile;
		SaveData _saveData;

		void Start()
		{
			_projectile = GetComponent<Projectile>();
			_saveData = SaveLoad.Load();
		}

		public void Update()
		{
			if (Input.GetKeyDown(KeyCode.S))
				Save();
			else if (Input.GetKeyDown(KeyCode.L))
				Load();
		}

		void Save()
		{
			_saveData.ProjectileDataList.Clear();
			foreach (Transform t in _projectile.ProjectileParent)
			{
				var rb = t.GetComponent<Rigidbody>();
				_saveData.ProjectileDataList.Add(new ProjectileData
				{
					AASKey = t.name,
					Position = t.position,
					Rotation = t.eulerAngles,
					Velocity = rb.velocity,
					AngularVelocity = rb.angularVelocity,
				});
			}
			SaveLoad.Save(_saveData);
		}

		void Load()
		{
			_saveData = SaveLoad.Load();
			// 表示されているオブジェクトを全部破棄してから、
			foreach (Transform t in _projectile.ProjectileParent)
				Destroy(t.gameObject);
			// ロード
			foreach (var i in _saveData.ProjectileDataList)
			{
				var instance = AASLoader.Instantiate(i.AASKey, _projectile.ProjectileParent);
				instance.name = i.AASKey;
				Destroy(instance, 4);
				var rb = instance.GetComponent<Rigidbody>();
				rb.transform.position = i.Position;
				rb.transform.eulerAngles = i.Rotation;
				rb.velocity = i.Velocity;
				rb.angularVelocity = i.AngularVelocity;
			}
		}
	}
}