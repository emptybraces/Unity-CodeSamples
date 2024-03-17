using UnityEditor;

namespace Emptybraces.Editor
{
	// 複数選択編集させないようにする
	[CustomEditor(typeof(KeyHighlighter))]
	public class KeyHighlighterInspector : UnityEditor.Editor { }
}