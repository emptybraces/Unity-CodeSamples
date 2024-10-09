using System.ComponentModel;
using UnityEngine;
using UnityEngine.Playables;

namespace Emptybraces.Timeline
{
	[System.Serializable, DisplayName("EmptyBraces/SimpleAtaptablePlayable")]
	public class SimpleProgrammablePlayableAsset : PlayableAsset
	{
		[NoFoldOut] public SimpleProgrammablePlayableBehaviour template = new SimpleProgrammablePlayableBehaviour();
		public override Playable CreatePlayable(PlayableGraph graph, GameObject go)
		{
			var playable = ScriptPlayable<SimpleProgrammablePlayableBehaviour>.Create(graph, template);
			return playable;
		}
	}

}
