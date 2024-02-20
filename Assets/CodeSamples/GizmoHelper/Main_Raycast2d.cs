using System;
using UnityEngine;

namespace EmptyBraces.GizmoHelperScene
{
	public class Main_Raycast2d : MonoBehaviour
	{
		[SerializeField] Transform _raycastFrom;
		[SerializeField] Transform[] _targets;
		[SerializeField] float _raycastSize = 1;
		[SerializeField] float _gizmoLifeTime = .5f;
		[SerializeField] int _raycastEmitInterval = 10;
		RaycastHit2D[] _raycastHits = new RaycastHit2D[3];
		enum RaycastType { Ray, Box, Circle }
		RaycastType _raycastType;
#if UNITY_EDITOR
		void OnGUI()
		{
			GUI.Label(new Rect(10, Screen.height - 50, 200, 20), $"LMB:レイ発射", UnityEditor.EditorStyles.boldLabel);
			GUI.Label(new Rect(10, Screen.height - 30, 200, 20), $"RMB:レイ変更({_raycastType})", UnityEditor.EditorStyles.boldLabel);
		}
#endif
		void Update()
		{
			foreach (var i in _targets)
				i.Rotate(Vector3.forward * 40 * Time.deltaTime);
			if (Input.GetMouseButtonDown(1))
				_raycastType = (RaycastType)Mathf.Repeat((int)_raycastType + 1, Enum.GetValues(typeof(RaycastType)).Length);
			if (Time.frameCount % _raycastEmitInterval == 0)
			{
				foreach (var i in _targets)
					i.GetComponent<SpriteRenderer>().color = Color.white;
				if (Input.GetMouseButton(0))
				{
					var p = Camera.main.ScreenToWorldPoint(Input.mousePosition);
					p.z = 0;
					var dir = (p - _raycastFrom.position).normalized;
					var mag = (p - _raycastFrom.position).magnitude;
					_raycastFrom.rotation = Quaternion.LookRotation(Vector3.forward, dir);
					var boxcast_angle = Mathf.Repeat(Time.time * 45, 90);
					int hit_count;
					switch (_raycastType)
					{
						case RaycastType.Box:
							hit_count = Physics2D.BoxCast(_raycastFrom.position, Vector2.one * _raycastSize, boxcast_angle, dir, new ContactFilter2D().NoFilter(), _raycastHits, mag);
							break;
						case RaycastType.Circle:
							hit_count = Physics2D.CircleCast(_raycastFrom.position, _raycastSize, dir, new ContactFilter2D().NoFilter(), _raycastHits, mag);
							break;
						default:
							hit_count = Physics2D.Raycast(_raycastFrom.position, dir, new ContactFilter2D().NoFilter(), _raycastHits, mag);
							break;
					}
					var gizmo_color = Color.grey;
					if (0 < hit_count)
					{
						cn.log("hit:", hit_count);
						for (int i = 0; i < hit_count; ++i)
						{
							_raycastHits[i].collider.GetComponent<SpriteRenderer>().color = Color.red;
							switch (_raycastType)
							{
								case RaycastType.Box:
									GizmoHelper.DrawCube(_raycastFrom.position + dir * _raycastHits[i].distance, Vector3.one * _raycastSize, Quaternion.Euler(0, 0, boxcast_angle), Color.HSVToRGB(Mathf.Repeat(Time.time, 1), 1f, .3f), _gizmoLifeTime);
									break;
								case RaycastType.Circle:
									GizmoHelper.DrawSphere(_raycastFrom.position + dir * _raycastHits[i].distance, _raycastSize, Color.HSVToRGB(Mathf.Repeat(Time.time, 1), 1f, .3f), _gizmoLifeTime);
									break;
							}
						}
						gizmo_color = Color.red;
					}
					GizmoHelper.DrawRay(_raycastFrom.position, dir * mag, gizmo_color, _gizmoLifeTime);
				}
			}
		}
	}
}