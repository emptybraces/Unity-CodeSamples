using UnityEngine;

namespace EmptyBraces.GizmoHelperScene
{
	public class Main_Raycast3d : MonoBehaviour
	{
		[SerializeField] float _rayLength = 30;
		[SerializeField] float _camAngleMax = 30;
		[SerializeField] float _camAngleSpeed = 1;
		[SerializeField] float _camAnglePower = 1;
		[SerializeField] int _gizmoLifeTime = 5;
		[SerializeField] int _raycastEmitInterval = 10;
		RaycastHit[] _raycastHits = new RaycastHit[1];
		void Update()
		{
			CameraLook();

			if (Time.frameCount % _raycastEmitInterval == 0 && Input.GetMouseButton(0))
			{
				var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				var hit_count = Physics.RaycastNonAlloc(ray, _raycastHits, _rayLength);
				var gizmo_color = Color.grey;
				if (0 < hit_count)
				{
					cn.log("hit:", _raycastHits[0].collider);
					gizmo_color = Color.red;
				}
				GizmoHelper.Instance.DrawRay(ray.origin, ray.direction * _rayLength, gizmo_color, _gizmoLifeTime);
			}
			if (Time.frameCount % _raycastEmitInterval == 0 && Input.GetMouseButton(1))
			{
				var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				var hit_count = Physics.SphereCastNonAlloc(ray, 0.5f, _raycastHits, _rayLength);
				var gizmo_color = Color.grey;
				if (0 < hit_count)
				{
					cn.log("hit:", _raycastHits[0].collider);
					gizmo_color = Color.red;
				}
				GizmoHelper.Instance.DrawRay(ray.origin, ray.direction * _rayLength, gizmo_color, _gizmoLifeTime);
				GizmoHelper.Instance.DrawSphereInterpolation(ray.origin, ray.origin + ray.direction * _rayLength, .5f, gizmo_color, _gizmoLifeTime, true);
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