using UnityEngine;
using UnityEditor;

namespace Emptybraces.Editor
{
	[CustomEditor(typeof(GizmoHelper))]
	public class GizmoHelperInspector : UnityEditor.Editor
	{
		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();
			if (GUILayout.Button("Clear All"))
			{
				GizmoHelper.RemoveAllGizmos();
			}
		}
	}
}