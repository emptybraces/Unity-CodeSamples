
using UnityEditor.Timeline;
using UnityEngine.Timeline;
namespace Emptybraces.Timeline
{
	[CustomTimelineEditor(typeof(VRM10ExpressionPlayableAsset))]
	public class VRM10ExpressionPlayableAssetClipEditor : ClipEditor
	{
		public override void OnClipChanged(TimelineClip clip)
		{
			if (clip.asset is VRM10ExpressionPlayableAsset asset)
			{
				if (asset.Template.PresetKey == UniVRM10.ExpressionPreset.custom)
					clip.displayName = asset.Template.CustomKey;
				else
					clip.displayName = asset.Template.PresetKey.ToString();
			}
		}
	}
}

