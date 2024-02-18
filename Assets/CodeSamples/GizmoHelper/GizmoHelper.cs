using System;
using System.Collections.Generic;
using UnityEngine;

namespace EmptyBraces
{
	public class GizmoHelper : MonoBehaviour
	{
		public Color DefaultColor = Color.white;
		[Range(0, 1)] public float Interpolation;
		public static GizmoHelper Instance
		{
			get
			{
				if (s_instance == null)
				{
					var g = new GameObject(nameof(GizmoHelper));
					DontDestroyOnLoad(g);
					s_instance = g.AddComponent<GizmoHelper>();
				}
				return s_instance;
			}
		}
		static GizmoHelper s_instance;
		List<Func<float>> _gizmoRenderList = new();
		void OnDrawGizmos()
		{
			for (int i = 0; i < _gizmoRenderList.Count; ++i)
				if (_gizmoRenderList[i]() < Time.unscaledTime)
					_gizmoRenderList.RemoveAt(i--);
		}
		public void DrawGizmosRay(Vector3 p, Vector3 dir, Color c, float time = 0)
		{
			var end_time = Time.unscaledTime + time - 0.0001f;
			_gizmoRenderList.Add(() =>
			{
				Gizmos.color = c;
				Gizmos.DrawRay(p, dir);
				return end_time;
			});
		}
		public void DrawGizmosSphere(Vector3 p, float radius, Color c, float time = 0)
		{
			var end_time = Time.unscaledTime + time - 0.0001f;
			_gizmoRenderList.Add(() =>
			{
				Gizmos.color = c;
				Gizmos.DrawSphere(p, radius);
				return end_time;
			});
		}
		public void DrawGizmosWireSphere(Vector3 p, float radius, Color c, float time = 0)
		{
			var end_time = Time.unscaledTime + time - 0.0001f;
			_gizmoRenderList.Add(() =>
			{
				Gizmos.color = c;
				Gizmos.DrawWireSphere(p, radius);
				return end_time;
			});
		}
		public void DrawGizmosCube(Vector3 p, Quaternion rotation, Vector3 size, Color c, float time = 0)
		{
			var end_time = Time.unscaledTime + time - 0.0001f;
			_gizmoRenderList.Add(() =>
			{
				Gizmos.color = c;
				Gizmos.matrix = Matrix4x4.TRS(p, rotation, size);
				Gizmos.DrawCube(Vector3.zero, Vector3.one);
				Gizmos.matrix = Matrix4x4.identity;
				return end_time;
			});
		}
		public void DrawGizmosLine(Vector3 p1, Vector3 p2, Color c, float time = 0)
		{
			var end_time = Time.unscaledTime + time - 0.0001f;
			_gizmoRenderList.Add(() =>
			{
				Gizmos.color = c;
				Gizmos.DrawLine(p1, p2);
				return end_time;
			});
		}
		public void RemoveAllGizmos()
		{
			_gizmoRenderList.Clear();
		}
	}
}