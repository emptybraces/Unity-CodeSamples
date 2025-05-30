using System.ComponentModel;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Emptybraces.Timeline
{
	[System.Serializable, DisplayName("EmptyBraces/UI/ImageAlpha")]
	public class UIImageColorPlayableAsset : PlayableAsset, ITimelineClipAsset
	{
		public ClipCaps clipCaps => ClipCaps.Blending | ClipCaps.Extrapolation;
		[NoFoldOut]
		[NotKeyable] // NotKeyable used to prevent Timeline from making fields available for animation.
		public UIImageColorPlayableBehaviour template = new UIImageColorPlayableBehaviour();
		public override Playable CreatePlayable(PlayableGraph graph, GameObject go)
		{
			// return Playable.Create(graph);
			return ScriptPlayable<UIImageColorPlayableBehaviour>.Create(graph, template);
		}
	}

}
