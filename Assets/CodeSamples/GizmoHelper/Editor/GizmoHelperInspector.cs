using UnityEngine;
using UnityEditor;

namespace EmptyBraces.Editor
{
	[CustomEditor(typeof(GizmoHelper))]//拡張するクラスを指定
	public class GizmoHelperInspector : UnityEditor.Editor
	{
		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();
			if (GUILayout.Button("Clear All"))
			{
				GizmoHelper.Instance.RemoveAllGizmos();
			}
		}
	}
}