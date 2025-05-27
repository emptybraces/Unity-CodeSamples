using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;
namespace Emptybraces.MsgSample
{
	public class Sample_Msg : MonoBehaviour
	{
		[SerializeField] GameObject _prefab;
		public int Max => 200;
		List<GameObject> _instances = new List<GameObject>();
		Material _matR, _matB;

		void Awake()
		{
			_matR = new Material(Shader.Find("Unlit/Color"));
			_matR.color = Color.red;
			_matB = new Material(_matR);
			_matB.color = Color.blue;
			_prefab.SetActive(false);
			_ = InstantiateMonitor();
			Msg.Set<int>((MsgId)99, _TestCallbackSetUnsetInvokeInParentInvoke);
		}
		async Task Start()
		{
			await Awaitable.WaitForSecondsAsync(2);
			Msg.Invoke(MsgId.OnInitedLongTime, this);
		}

		void Update()
		{
			if (Mouse.current.leftButton.wasPressedThisFrame || Mouse.current.rightButton.wasPressedThisFrame)
			{
				var mat = Mouse.current.leftButton.wasPressedThisFrame ? _matR : _matB;
				int cnt = 0;
				for (int i = _instances.Count - 1; 0 <= i; --i)
				{
					if (_instances[i].GetComponent<MeshRenderer>().sharedMaterial == mat)
					{
						Destroy(_instances[i]);
						_instances.RemoveAt(i);
						++cnt;
					}
				}
				Msg.Invoke(MsgId.OnRemoved, cnt);
			}
			// set, invokeの連続テスト
			else if (Keyboard.current[Key.P].wasPressedThisFrame)
			{
				Msg.Invoke((MsgId)99, 0);
			}
		}

		void _TestCallbackSetUnsetInvokeInParentInvoke(int n)
		{
			// 無限ループチェック
			cn.log("再帰コールチェックテスト:", n);
			Msg.Invoke((MsgId)99, n + 1);
			// Invokeの中でSetして、
			Msg.Set<int>((MsgId)(100), _Logged);
			Msg.Set<int>((MsgId)(101), _Logged);
			// Invokeの中でUnsetして、
			Msg.Unset<int>((MsgId)(101), _Logged);
			// Invokeの中で呼び出す。
			Msg.Invoke<int>((MsgId)(100), 100);
			Msg.Invoke<int>((MsgId)(101), 101);
		}
		void _Logged(int i)
		{
			cn.log(i);
		}

		void OnDestroy()
		{
			if (_matR) Destroy(_matR);
			if (_matB) Destroy(_matB);
		}

		async Task InstantiateMonitor()
		{
			// var range = Enumerable.Range(0, Max);
			// var pos = range.Select(x => new Vector3(Random.Range(-1f, 1f), 1, 0)).ToArray();
			// var rot = range.Select(x => Random.rotation).ToArray();
			AsyncInstantiateOperation.SetIntegrationTimeMS(0.01f);
			while (isActiveAndEnabled)
			{
				for (int i = _instances.Count; i < Max; ++i)
				{
					var task = InstantiateAsync(_prefab, new Vector3(Random.Range(-1f, 1f), 1, 0), Random.rotation);
					// var icnt = Max - _instances.Count;
					// var task = InstantiateAsync(_prefab, icnt,
					// 	new System.Span<Vector3>(pos, _instances.Count, icnt),
					// 	new System.Span<Quaternion>(rot, _instances.Count, icnt));
					while (!task.isDone)
						await Awaitable.NextFrameAsync(destroyCancellationToken);
					_instances.AddRange(task.Result);
					foreach (var g in task.Result)
					{
						g.SetActive(true);
						g.GetComponent<MeshRenderer>().sharedMaterial = Random.value < 0.5f ? _matR : _matB;
					}
					Msg.Invoke(MsgId.OnCreated, 1);
					// Msg.Invoke(MsgId.OnCreated, icnt);

					// var g = Instantiate(_prefab);
					// g.transform.position = new Vector3(Random.Range(-1f, 1f), 1, 0);
					// g.GetComponent<MeshRenderer>().sharedMaterial = Random.value < 0.5f ? _matR : _matB;
					// g.SetActive(true);
					// _instances.Add(g);
					// Msg.Invoke(MsgId.OnCreated);
				}
				await Awaitable.WaitForSecondsAsync(0.01f, destroyCancellationToken);
			}
		}
	}
}
