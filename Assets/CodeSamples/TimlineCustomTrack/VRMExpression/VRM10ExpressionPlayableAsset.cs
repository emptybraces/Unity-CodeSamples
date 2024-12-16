using System.ComponentModel;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Emptybraces.Timeline
{
	[System.Serializable, DisplayName("EmptyBraces/VRM10/Expression")]
	public class VRM10ExpressionPlayableAsset : PlayableAsset, ITimelineClipAsset
	{
		public ClipCaps clipCaps => ClipCaps.Blending | ClipCaps.Extrapolation;
		[NoFoldOut, NotKeyable]
		public VRM10ExpressionPlayableBehaviour Template = new VRM10ExpressionPlayableBehaviour();
		public override Playable CreatePlayable(PlayableGraph graph, GameObject go)
		{
			// return Playable.Create(graph);
			return ScriptPlayable<VRM10ExpressionPlayableBehaviour>.Create(graph, Template);
		}
	}

}
