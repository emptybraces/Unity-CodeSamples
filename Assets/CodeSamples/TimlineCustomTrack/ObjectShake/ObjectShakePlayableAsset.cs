using System.ComponentModel;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Emptybraces.Timeline
{
	[System.Serializable, DisplayName("EmptyBraces/Object Shake")]
	public class ObjectShakePlayableAsset : PlayableAsset, ITimelineClipAsset
	{
		public ClipCaps clipCaps => ClipCaps.Blending | ClipCaps.Extrapolation;
		[NoFoldOut]
		[NotKeyable] // NotKeyable used to prevent Timeline from making fields available for animation.
		public ObjectShakePlayableBehaviour template = new ObjectShakePlayableBehaviour();
		public override Playable CreatePlayable(PlayableGraph graph, GameObject go)
		{
			// return Playable.Create(graph);
			return ScriptPlayable<ObjectShakePlayableBehaviour>.Create(graph, template);
		}
	}

}
