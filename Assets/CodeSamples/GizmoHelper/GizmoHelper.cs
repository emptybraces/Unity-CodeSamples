using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace Emptybraces
{
	[AddComponentMenu("")]
	public class GizmoHelper : MonoBehaviour
	{
		Color DefaultColor = Color.white;
		static GizmoHelper Instance
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
		List<Func<bool>> _gizmoRenderList = new();

		void OnDrawGizmos()
		{
			for (int i = 0; i < _gizmoRenderList.Count; ++i)
				if (_gizmoRenderList[i]())
					_gizmoRenderList.RemoveAt(i--);
		}
		[Conditional("UNITY_EDITOR")] public static void DrawRay(Vector3 p, Vector3 dir, float time = 0) => DrawRay(p, dir, Instance.DefaultColor, time);
		[Conditional("UNITY_EDITOR")]
		public static void DrawRay(Vector3 p, Vector3 dir, Color c, float span = 0, bool unscaled = false)
		{
			var end_time = unscaled ? Time.unscaledTime : Time.time + span;
			Instance._gizmoRenderList.Add(() =>
			{
				Gizmos.color = c;
				Gizmos.DrawRay(p, dir);
				return end_time < (unscaled ? Time.unscaledTime : Time.time);
			});
		}
		[Conditional("UNITY_EDITOR")] public static void DrawMesh(Mesh mesh, int submeshIndex, Vector3 p, Quaternion r, Vector3 s, float time = 0) => DrawMesh(mesh, submeshIndex, p, r, s, Instance.DefaultColor, time);
		[Conditional("UNITY_EDITOR")]
		public static void DrawMesh(Mesh mesh, int submeshIndex, Vector3 p, Quaternion r, Vector3 s, Color c, float span = 0, bool unscaled = false)
		{
			var end_time = unscaled ? Time.unscaledTime : Time.time + span;
			Instance._gizmoRenderList.Add(() =>
			{
				Gizmos.color = c;
				Gizmos.DrawMesh(mesh, submeshIndex, p, r, s);
				return end_time < (unscaled ? Time.unscaledTime : Time.time);
			});
		}
		[Conditional("UNITY_EDITOR")] public static void DrawWireMesh(Mesh mesh, int submeshIndex, Vector3 p, Quaternion r, Vector3 s, float time = 0) => DrawWireMesh(mesh, submeshIndex, p, r, s, Instance.DefaultColor, time);
		[Conditional("UNITY_EDITOR")]
		public static void DrawWireMesh(Mesh mesh, int submeshIndex, Vector3 p, Quaternion r, Vector3 s, Color c, float span = 0, bool unscaled = false)
		{
			var end_time = unscaled ? Time.unscaledTime : Time.time + span;
			Instance._gizmoRenderList.Add(() =>
			{
				Gizmos.color = c;
				Gizmos.DrawWireMesh(mesh, submeshIndex, p, r, s);
				return end_time < (unscaled ? Time.unscaledTime : Time.time);
			});
		}
		[Conditional("UNITY_EDITOR")] public static void DrawSphere(Vector3 p, float radius, float time = 0) => DrawSphere(p, radius, Instance.DefaultColor, time);
		[Conditional("UNITY_EDITOR")]
		public static void DrawSphere(Vector3 p, float radius, Color c, float span = 0, bool unscaled = false)
		{
			var end_time = unscaled ? Time.unscaledTime : Time.time + span;
			Instance._gizmoRenderList.Add(() =>
			{
				Gizmos.color = c;
				Gizmos.DrawSphere(p, radius);
				return end_time < (unscaled ? Time.unscaledTime : Time.time);
			});
		}
		[Conditional("UNITY_EDITOR")] public static void DrawWireSphere(Vector3 p, float radius, float span = 0) => DrawWireSphere(p, radius, Instance.DefaultColor, span);
		[Conditional("UNITY_EDITOR")]
		public static void DrawWireSphere(Vector3 p, float radius, Color c, float span = 0, bool unscaled = false)
		{
			var end_time = unscaled ? Time.unscaledTime : Time.time + span;
			Instance._gizmoRenderList.Add(() =>
			{
				Gizmos.color = c;
				Gizmos.DrawWireSphere(p, radius);
				return end_time < (unscaled ? Time.unscaledTime : Time.time);
			});
		}
		[Conditional("UNITY_EDITOR")] public static void DrawCube(Vector3 p, Vector3 size, Quaternion r, float span = 0) => DrawCube(p, size, r, Instance.DefaultColor, span);
		[Conditional("UNITY_EDITOR")]
		public static void DrawCube(Vector3 p, Vector3 size, Quaternion r, Color c, float span = 0, bool unscaled = false)
		{
			var end_time = unscaled ? Time.unscaledTime : Time.time + span;
			Instance._gizmoRenderList.Add(() =>
			{
				Gizmos.color = c;
				Gizmos.matrix = Matrix4x4.TRS(p, r, size);
				Gizmos.DrawCube(Vector3.zero, Vector3.one);
				Gizmos.matrix = Matrix4x4.identity;
				return end_time < (unscaled ? Time.unscaledTime : Time.time);
			});
		}
		[Conditional("UNITY_EDITOR")] public static void DrawWireCube(Vector3 p, Vector3 size, Quaternion r, float span = 0) => DrawWireCube(p, size, r, Instance.DefaultColor, span);
		[Conditional("UNITY_EDITOR")]
		public static void DrawWireCube(Vector3 p, Vector3 size, Quaternion r, Color c, float span = 0, bool unscaled = false)
		{
			var end_time = unscaled ? Time.unscaledTime : Time.time + span;
			Instance._gizmoRenderList.Add(() =>
			{
				Gizmos.color = c;
				Gizmos.matrix = Matrix4x4.TRS(p, r, size);
				Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
				Gizmos.matrix = Matrix4x4.identity;
				return end_time < (unscaled ? Time.unscaledTime : Time.time);
			});
		}
		[Conditional("UNITY_EDITOR")] public static void DrawLine(Vector3 p1, Vector3 p2, float span = 0) => DrawLine(p1, p2, Instance.DefaultColor, span);
		[Conditional("UNITY_EDITOR")]
		public static void DrawLine(Vector3 p1, Vector3 p2, Color c, float span = 0, bool unscaled = false)
		{
			var end_time = unscaled ? Time.unscaledTime : Time.time + span;
			Instance._gizmoRenderList.Add(() =>
			{
				Gizmos.color = c;
				Gizmos.DrawLine(p1, p2);
				return end_time < (unscaled ? Time.unscaledTime : Time.time);
			});
		}
		[Conditional("UNITY_EDITOR")] public static void DrawBezier(Vector3 p1, Vector3 p2, Vector3 p3, float span = 0) => DrawBezier(p1, p2, p3, Instance.DefaultColor, span);
		[Conditional("UNITY_EDITOR")]
		public static void DrawBezier(Vector3 p1, Vector3 p2, Vector3 p3, Color c, float span = 0, bool unscaled = false)
		{
			var end_time = unscaled ? Time.unscaledTime : Time.time + span;
			Instance._gizmoRenderList.Add(() =>
			{
				Gizmos.color = c;
				var prev = p1;
				const int cnt = 8;
				float f = 1 / (float)cnt;
				for (int i = 1; i <= cnt; ++i)
				{
					var next = __Bezier2d(p1, p2, p3, i * f);
					Gizmos.DrawLine(prev, next);
					prev = next;
				}
				return end_time < (unscaled ? Time.unscaledTime : Time.time);
			});
			static Vector3 __Bezier2d(Vector3 p0, Vector3 p1, Vector3 p2, float t) => (1.0f - t) * (1.0f - t) * p0 + 2.0f * (1.0f - t) * t * p1 + t * t * p2;
		}
		[Conditional("UNITY_EDITOR")]
		public static void RemoveAllGizmos()
		{
			Instance._gizmoRenderList.Clear();
		}

		// [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		// static void _DomainReset()
		// {
		// }
	}
}
