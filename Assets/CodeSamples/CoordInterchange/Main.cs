using UnityEngine;
namespace Emptybraces.CoordInterchange
{
	public class Main : MonoBehaviour
	{
		[SerializeField] Transform _box;
		[SerializeField] RectTransform _rt;
		[SerializeField] float _boxExtent = .5f;
		enum Mode { MousePositionToWorld_SetZ, MousePositionToWorld_Raycast, MousePositionToOverlayCanvas }
		Mode _mode = Mode.MousePositionToOverlayCanvas;

#if UNITY_EDITOR
		void OnGUI()
		{
			GUI.Label(new Rect(10, Screen.height - 50, 300, 20), _mode.ToString(), UnityEditor.EditorStyles.boldLabel);
			GUI.Label(new Rect(10, Screen.height - 30, 300, 20), $"RMB:変更", UnityEditor.EditorStyles.boldLabel);
		}
#endif
		void Start()
		{
			_rt.GetComponentInParent<Canvas>().gameObject.SetActive(false);

		}

		void Update()
		{
			if (Input.GetMouseButtonDown(1))
				_mode = (Mode)Mathf.Repeat((int)_mode + 1, System.Enum.GetValues(typeof(Mode)).Length);
			switch (_mode)
			{
				case Mode.MousePositionToWorld_SetZ: _MousePositionToWorld_SetZ(Camera.main); break;
				case Mode.MousePositionToWorld_Raycast: _MousePositionToWorld_Raycast(Camera.main); break;
				case Mode.MousePositionToOverlayCanvas: _MousePositionToOverlayCanvas(Camera.main); break;
			}


		}

		void _MousePositionToWorld_SetZ(Camera camera)
		{
			var sp = Input.mousePosition;
			sp.z = Mathf.Repeat(Time.time * 2, 10);
			var wp = camera.ScreenToWorldPoint(sp);
			_box.position = wp;

		}
		const float BOX_EXTENT = 0.5f;
		void _MousePositionToWorld_Raycast(Camera camera)
		{
			if (Physics.Raycast(camera.ScreenPointToRay(Input.mousePosition), out var hit, 100, LayerMask.GetMask("raycast")))
			{
				_box.position = hit.point + hit.normal * _boxExtent;
				_box.forward = hit.normal;
			}
		}
		void _MousePositionToOverlayCanvas(Camera camera)
		{
			_rt.GetComponentInParent<Canvas>(true).gameObject.SetActive(true);
			if (RectTransformUtility.ScreenPointToLocalPointInRectangle(_rt, Input.mousePosition, camera, out var point))
			{
				_rt.anchoredPosition = point;

			}
		}
	}
}