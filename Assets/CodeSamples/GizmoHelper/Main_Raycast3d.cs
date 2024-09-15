using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Emptybraces.GizmoHelperScene
{
	public class Main_Raycast3d : MonoBehaviour
	{
		[SerializeField] Transform[] _targets;
		[SerializeField] float _rayLength = 30;
		[SerializeField] float _raycastSize = 1;
		[SerializeField] float _camAngleMax = 30;
		[SerializeField] float _camAngleSpeed = 1;
		[SerializeField] float _camAnglePower = 1;
		[SerializeField] int _gizmoLifeTime = 5;
		[SerializeField] int _raycastEmitInterval = 10;
		[SerializeField] UnityEngine.UI.Text _uiText;

		RaycastHit[] _raycastHits = new RaycastHit[3];
		enum RaycastType { Ray, Box, Circle, Bezier }
		RaycastType _raycastType;
		MaterialPropertyBlock _mpbHit;

		void Start()
		{
			_mpbHit = new();
			_mpbHit.SetColor("_BaseColor", Color.red);
			_uiText.text = $"LMB:レイ発射({_raycastType})";
		}

		void Update()
		{
			CameraLook();
			foreach (var i in _targets)
				i.Rotate(Vector3.forward * 40 * Time.deltaTime);
			if (Input.GetMouseButtonDown(1))
			{
				_raycastType = (RaycastType)Mathf.Repeat((int)_raycastType + 1, Enum.GetValues(typeof(RaycastType)).Length);
				_uiText.text = $"LMB:レイ発射({_raycastType})";
			}

			if (Time.frameCount % _raycastEmitInterval == 0)
			{
				foreach (var i in _targets)
					i.GetComponent<MeshRenderer>().SetPropertyBlock(null);
				if (Input.GetMouseButton(0))
				{
					var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
					var boxcast_rotation = Quaternion.Euler(0, Mathf.Repeat(Time.time * 45, 90), 0);
					var hit_count = 0;
					switch (_raycastType)
					{
						case RaycastType.Ray:
						case RaycastType.Bezier:
							hit_count = Physics.RaycastNonAlloc(ray, _raycastHits, _rayLength);
							break;
						case RaycastType.Box:
							hit_count = Physics.BoxCastNonAlloc(ray.origin, Vector3.one * _raycastSize * 0.5f/*half extent*/, ray.direction, _raycastHits, boxcast_rotation, _rayLength);
							break;
						case RaycastType.Circle:
							hit_count = Physics.SphereCastNonAlloc(ray, _raycastSize, _raycastHits, _rayLength);
							break;
					}

					var gizmo_color = Color.grey;
					if (0 < hit_count)
					{
						cn.log("hit:", _raycastHits[0].collider);
						for (int i = 0; i < hit_count; ++i)
						{
							_raycastHits[i].collider.GetComponent<MeshRenderer>().SetPropertyBlock(_mpbHit);
							var hit_pt = ray.origin + ray.direction * _raycastHits[i].distance;
							var color = Color.HSVToRGB(Mathf.Repeat(Time.time, 1), 1f, .3f);
							switch (_raycastType)
							{
								case RaycastType.Box:
									GizmoHelper.DrawCube(hit_pt, Vector3.one * _raycastSize, boxcast_rotation, color, _gizmoLifeTime);
									break;
								case RaycastType.Circle:
									GizmoHelper.DrawSphere(hit_pt, _raycastSize, color, _gizmoLifeTime);
									break;
								case RaycastType.Bezier:
									GizmoHelper.DrawBezier(ray.origin, (ray.origin + hit_pt) / 2 + Random.onUnitSphere * Mathf.Lerp(4, 10, Random.value), hit_pt, color, _gizmoLifeTime);
									break;
							}
						}
						gizmo_color = Color.red;
					}
					GizmoHelper.DrawRay(ray.origin, ray.direction * _rayLength, gizmo_color, _gizmoLifeTime);
				}
			}
		}

		void CameraLook()
		{
			var scr_point = Input.mousePosition;
			if (scr_point.x < 0 || Screen.width < scr_point.x || scr_point.y < 0 || Screen.height < scr_point.y)
				return;
			var vp = Camera.main.ScreenToViewportPoint(scr_point);
			vp.x = vp.x * 2 - 1; // -1.0 ~ +1.0
			vp.y = vp.y * 2 - 1; // -1.0 ~ +1.0
			vp.x = Mathf.Clamp(vp.x * _camAnglePower, -1, 1);
			vp.y = Mathf.Clamp(vp.y * _camAnglePower, -1, 1);
			var target_transform = Camera.main.transform;
			var angle = target_transform.eulerAngles;
			var tx = (_camAngleSpeed + (_camAngleSpeed * 0.5f * Mathf.Abs(vp.x))) * Time.deltaTime; // 端にいくほど速度が少し早くなる
			var ty = (_camAngleSpeed + (_camAngleSpeed * 0.5f * Mathf.Abs(vp.y))) * Time.deltaTime; // 端にいくほど速度が少し早くなる
			angle.x = Mathf.LerpAngle(angle.x, vp.y * -_camAngleMax, tx);
			angle.y = Mathf.LerpAngle(angle.y, vp.x * _camAngleMax, ty);
			target_transform.eulerAngles = angle;
		}
	}
}
