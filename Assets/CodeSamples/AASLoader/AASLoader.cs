using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Assertions;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Emptybraces
{
	public static class AASLoader
	{
		static Dictionary<string, AsyncOperationHandle<Object>> _cache = new(16);
		static Dictionary<string, AsyncOperationHandle<Sprite>> _cacheSprite = new(16);
		static bool IsInvalidKey(string key) => key == "" || key == "none";
		// 同期ロード
		public static T Load<T>(string key) where T : Object
		{
			Assert.IsFalse(typeof(T) == typeof(Sprite));
			if (IsInvalidKey(key))
				return null;
			if (_cache.TryGetValue(key, out var handle))
				return handle.Status == AsyncOperationStatus.Succeeded ? (T)handle.Result : null;
			Debug.Log($"{nameof(AASLoader)}: Load() key={key}");
			handle = Addressables.LoadAssetAsync<Object>(key);
			_cache[key] = handle;
			handle.WaitForCompletion();
			return (T)handle.Result;
		}
		// 同期ロード
		public static Sprite LoadSprite(string key)
		{
			if (IsInvalidKey(key))
				return null;
			if (_cacheSprite.TryGetValue(key, out var handle))
				return handle.Status == AsyncOperationStatus.Succeeded ? handle.Result : null;
			Debug.Log($"{nameof(AASLoader)}: LoadSprite() key={key}");
			handle = Addressables.LoadAssetAsync<Sprite>(key);
			_cacheSprite[key] = handle;
			handle.WaitForCompletion();
			return handle.Result;
		}
		// 非同期ロード
		public static async UniTask<T> LoadAsync<T>(string key) where T : UnityEngine.Object
		{
			Assert.IsFalse(typeof(T) == typeof(Sprite));
			if (IsInvalidKey(key))
				return null;
			if (_cache.TryGetValue(key, out var handle))
			{
				Debug.Log($"{nameof(AASLoader)}: LoadAsync<T>() key={key}, type={typeof(T).Name}, fromCache={true}");
				if (!handle.IsDone)
					await handle;
				return handle.Status == AsyncOperationStatus.Succeeded ? (T)handle.Result : null;
			}
			Debug.Log($"{nameof(AASLoader)}: LoadAsync<T>() key={key}, type={typeof(T).Name}, fromCache={false}");
			handle = Addressables.LoadAssetAsync<Object>(key);
			_cache[key] = handle;
			await handle;
			return handle.Status == AsyncOperationStatus.Succeeded ? (T)handle.Result : null;
		}
		// 非同期ロード
		public static async UniTask<Sprite> LoadSpriteAsync(string key, System.Action<Sprite> onLoaded)
		{
			if (IsInvalidKey(key))
				return null;
			if (_cacheSprite.TryGetValue(key, out var handle))
			{
				Debug.Log($"{nameof(AASLoader)}: LoadSpriteAsync<T>() key={key}, fromCache={true}");
				if (!handle.IsDone)
					await handle;
				return handle.Status == AsyncOperationStatus.Succeeded ? handle.Result : null;
			}
			Debug.Log($"{nameof(AASLoader)}: LoadSpriteAsync<T>() key={key}, fromCache={false}");
			handle = Addressables.LoadAssetAsync<Sprite>(key);
			_cacheSprite[key] = handle;
			await handle;
			return handle.Status == AsyncOperationStatus.Succeeded ? handle.Result : null;
		}
		public static GameObject Instantiate(string key, Transform parent = null)
		{
			var p = Load<GameObject>(key);
			if (p != null)
				return GameObject.Instantiate(p, parent);
			return null;
		}
		public static T Instantiate<T>(Transform parent = null) where T : Component
		{
			var p = Load<GameObject>(typeof(T).Name);
			if (p != null)
				return GameObject.Instantiate(p, parent).GetComponent<T>();
			return null;
		}
		public static async UniTask<GameObject> InstantiateAsync(string key, Transform parent = null)
		{
			var p = await LoadAsync<GameObject>(key);
			if (p != null)
				return GameObject.Instantiate(p, parent);
			return null;
		}
		public static async UniTask<T> InstantiateAsync<T>(string key, Transform parent = null)
		{
			var p = await LoadAsync<GameObject>(key);
			if (p != null)
				return GameObject.Instantiate(p, parent).GetComponent<T>();
			return default;
		}

		public static async UniTask<T> InstantiateAsync<T>(Transform parent = null) where T : Component
		{
			var p = await LoadAsync<GameObject>(typeof(T).Name);
			if (p != null)
				return GameObject.Instantiate(p, parent).GetComponent<T>();
			return null;
		}

		public static void Release()
		{
			Debug.Log($"{nameof(AASLoader)}: Release()");
			foreach (var i in _cache)
				Addressables.Release(i.Value);
			foreach (var i in _cacheSprite)
				Addressables.Release(i.Value);
			_cache.Clear();
			_cacheSprite.Clear();
		}

		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		static void OnBeforeSceneLoad()
		{
			// アプリケーションが終了したときに解放する
			Application.quitting += Release;
		}
#if UNITY_EDITOR
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		static void _DomainReset()
		{
			Application.quitting -= Release;
		}
#endif
	}
}
