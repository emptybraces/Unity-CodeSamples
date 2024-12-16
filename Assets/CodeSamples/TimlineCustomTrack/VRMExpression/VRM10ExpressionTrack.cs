using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UniVRM10;

namespace Emptybraces.Timeline
{
	[TrackColor(0.2f, 0.6f, 0.8f)]
	[TrackClipType(typeof(VRM10ExpressionPlayableAsset))]
	[TrackBindingType(typeof(Vrm10Instance))]
	public class VRM10ExpressionTrack : TrackAsset
	{
		public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
		{
			return ScriptPlayable<VRM10ExpressionMixerBehaviour>.Create(graph, inputCount);
		}
	}
}
