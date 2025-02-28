using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;
namespace Emptybraces.MsgSample
{
	public class Sample_Msg : MonoBehaviour
	{
		[SerializeField] GameObject _prefab;
		public int Max => 200;
		List<GameObject> _ins = new List<GameObject>();
		Material _matR, _matB;

		void Awake()
		{
			_matR = new Material(Shader.Find("Unlit/Color"));
			_matR.color = Color.red;
			_matB = new Material(_matR);
			_matB.color = Color.blue;
			_prefab.SetActive(false);
			_ = InstantiateMonitor();
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
				for (int i = _ins.Count - 1; 0 <= i; --i)
				{
					if (_ins[i].GetComponent<MeshRenderer>().sharedMaterial == mat)
					{
						Destroy(_ins[i]);
						_ins.RemoveAt(i);
						Msg.Invoke(MsgId.OnRemoved);
					}
				}
			}
		}

		void OnDestroy()
		{
			if (_matR) Destroy(_matR);
			if (_matB) Destroy(_matB);
		}

		async Task InstantiateMonitor()
		{
			while (isActiveAndEnabled)
			{
				if (_ins.Count <= Max)
				{
					var go = Instantiate(_prefab);
					go.transform.position = new Vector3(Random.Range(-1f, 1f), 1, 0);
					go.GetComponent<MeshRenderer>().sharedMaterial = Random.value < 0.5f ? _matR : _matB;
					go.SetActive(true);
					_ins.Add(go);
					Msg.Invoke(MsgId.OnCreated);
				}
				await Awaitable.WaitForSecondsAsync(0.01f, destroyCancellationToken);
			}
		}
	}
}
