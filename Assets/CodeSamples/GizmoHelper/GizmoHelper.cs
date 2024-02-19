using System;
using System.Collections.Generic;
using UnityEditor;
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
				if (_gizmoRenderList[i]() < Time.time)
					_gizmoRenderList.RemoveAt(i--);
		}
		public void DrawRay(Vector3 p, Vector3 dir, float time = 0) => DrawRay(p, dir, DefaultColor, time);
		public void DrawRay(Vector3 p, Vector3 dir, Color c, float time = 0)
		{
			var end_time = Time.unscaledTime + time - 0.0001f;
			_gizmoRenderList.Add(() =>
			{
				Gizmos.color = c;
				Gizmos.DrawRay(p, dir);
				return end_time;
			});
		}
		public void DrawSphere(Vector3 p, float radius, float time = 0) => DrawSphere(p, radius, DefaultColor, time);
		public void DrawSphere(Vector3 p, float radius, Color c, float time = 0)
		{
			var end_time = Time.unscaledTime + time - 0.0001f;
			_gizmoRenderList.Add(() =>
			{
				Gizmos.color = c;
				Gizmos.DrawSphere(p, radius);
				return end_time;
			});
		}
		public void DrawSphereInterpolation(Vector3 p1, Vector3 p2, float radius, Color c, float time = 0, bool isEditorPause = false)
		{
			var end_time = Time.unscaledTime + time - 0.0001f;
			_gizmoRenderList.Add(() =>
			{
				Gizmos.color = c;
				Gizmos.DrawSphere(Vector3.Lerp(p1, p2, Interpolation), radius);
				if (isEditorPause)
				{
					EditorApplication.isPaused = true;
					isEditorPause = false;
				}
				return end_time;
			});
		}
		public void DrawWireSphere(Vector3 p, float radius, float time = 0) => DrawWireSphere(p, radius, DefaultColor, time);
		public void DrawWireSphere(Vector3 p, float radius, Color c, float time = 0)
		{
			var end_time = Time.unscaledTime + time - 0.0001f;
			_gizmoRenderList.Add(() =>
			{
				Gizmos.color = c;
				Gizmos.DrawWireSphere(p, radius);
				return end_time;
			});
		}
		public void DrawCube(Vector3 p, Quaternion r, Vector3 size, float time = 0) => DrawCube(p, r, size, DefaultColor, time);
		public void DrawCube(Vector3 p, Quaternion r, Vector3 size, Color c, float time = 0)
		{
			var end_time = Time.unscaledTime + time - 0.0001f;
			_gizmoRenderList.Add(() =>
			{
				Gizmos.color = c;
				Gizmos.matrix = Matrix4x4.TRS(p, r, size);
				Gizmos.DrawCube(Vector3.zero, Vector3.one);
				Gizmos.matrix = Matrix4x4.identity;
				return end_time;
			});
		}
		public void DrawWireCube(Vector3 p, Quaternion r, Vector3 size, float time = 0) => DrawWireCube(p, r, size, DefaultColor, time);
		public void DrawWireCube(Vector3 p, Quaternion r, Vector3 size, Color c, float time = 0)
		{
			var end_time = Time.unscaledTime + time - 0.0001f;
			_gizmoRenderList.Add(() =>
			{
				Gizmos.color = c;
				Gizmos.matrix = Matrix4x4.TRS(p, r, size);
				Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
				Gizmos.matrix = Matrix4x4.identity;
				return end_time;
			});
		}
		public void DrawLine(Vector3 p1, Vector3 p2, float time = 0) => DrawLine(p1, p2, DefaultColor, time);
		public void DrawLine(Vector3 p1, Vector3 p2, Color c, float time = 0)
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