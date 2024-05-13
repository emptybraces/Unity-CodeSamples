using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Assertions;

namespace Emptybraces
{
	public class Projectile : MonoBehaviour
	{
		[SerializeField] bool _useAsyncLoad = true;
		[SerializeField] float _emitInterval = .3f;
		[SerializeField] float _projectileLifeSpan = 3f;
		[SerializeField] float _projectilePower = 5f;
		[SerializeField] float _projectileTorquePower = 5f;
		[SerializeField] string[] _loadAASKeys;
		[field: SerializeField] public Transform ProjectileParent { get; private set; }
		float _lastEmitTime;
		void Update()
		{
			if (Input.GetMouseButton(0) && _lastEmitTime + _emitInterval < Time.time)
			{
				Debug.Log($"<color=green>Start Projectile: {Time.frameCount}</color>");
				_lastEmitTime = Time.time;
				// 生成
				var key = _loadAASKeys[Random.Range(0, _loadAASKeys.Length)];
				// 非同期ロード
				if (_useAsyncLoad)
					LoadAsync(key).Forget();
				// 同期ロード
				else
					OnLoaded(AASLoader.Load<GameObject>(key));

			}
		}

		async UniTaskVoid LoadAsync(string key)
		{
			var prefab = await AASLoader.LoadAsync<GameObject>(key);
			OnLoaded(prefab);
		}

		void OnLoaded(GameObject prefab)
		{
			Assert.IsNotNull(prefab);
			// 生成
			var instance = Instantiate(prefab, ProjectileParent);
			instance.name = prefab.name;
			Destroy(instance, _projectileLifeSpan); // タイマーで破棄

			// 飛ばす
			ProjectileObject(instance.GetComponent<Rigidbody>());
			Debug.Log($"<color=red>Finish Projectile: {Time.frameCount}</color>");
		}

		void ProjectileObject(Rigidbody rb)
		{
			rb.position = Camera.main.transform.position;
			var mp = Input.mousePosition;
			mp.z = 1;
			var wp = Camera.main.ScreenToWorldPoint(mp);
			var dir = (wp - Camera.main.transform.position).normalized;
			rb.AddForce(dir * _projectilePower, ForceMode.Force);
			rb.AddTorque(Random.onUnitSphere * _projectileTorquePower, ForceMode.Force);
		}
	}
}
