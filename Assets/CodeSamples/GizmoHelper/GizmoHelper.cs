using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace EmptyBraces
{
	[AddComponentMenu("")]
	public class GizmoHelper : MonoBehaviour
	{
		public Color DefaultColor = Color.white;
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
		List<Func<bool>> _gizmoRenderList = new();
		void OnDrawGizmos()
		{
			for (int i = 0; i < _gizmoRenderList.Count; ++i)
				if (_gizmoRenderList[i]())
					_gizmoRenderList.RemoveAt(i--);
		}
		[Conditional("DEBUG")] public void DrawRay(Vector3 p, Vector3 dir, float time = 0) => DrawRay(p, dir, DefaultColor, time);
		[Conditional("DEBUG")]
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
		[Conditional("DEBUG")] public static void DrawSphere(Vector3 p, float radius, float time = 0) => DrawSphere(p, radius, Instance.DefaultColor, time);
		[Conditional("DEBUG")]
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
		[Conditional("DEBUG")] public static void DrawWireSphere(Vector3 p, float radius, float span = 0) => DrawWireSphere(p, radius, Instance.DefaultColor, span);
		[Conditional("DEBUG")]
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
		[Conditional("DEBUG")] public static void DrawCube(Vector3 p, Vector3 size, Quaternion r, float span = 0) => DrawCube(p, size, r, Instance.DefaultColor, span);
		[Conditional("DEBUG")]
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
		[Conditional("DEBUG")] public static void DrawWireCube(Vector3 p, Vector3 size, Quaternion r, float span = 0) => DrawWireCube(p, size, r, Instance.DefaultColor, span);
		[Conditional("DEBUG")]
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
		[Conditional("DEBUG")] public static void DrawLine(Vector3 p1, Vector3 p2, float span = 0) => DrawLine(p1, p2, Instance.DefaultColor, span);
		[Conditional("DEBUG")]
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
		[Conditional("DEBUG")]
		public static void RemoveAllGizmos()
		{
			Instance._gizmoRenderList.Clear();
		}
	}
}