using UnityEditor;
namespace Emptybraces.Editor
{
	static class MsgMenu
	{
		[MenuItem("Tools/Msg/Dump", false)] static void _Dump() => Msg.Dump();
	}
}
