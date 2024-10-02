using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Emptybraces.Timeline
{
    [TrackColor(0.8f, 0.8f, 0.8f)]
    [TrackClipType(typeof(UIImageColorPlayableAsset))]
    [TrackBindingType(typeof(MonoBehaviour))]
    public class UIImageColorTrack : TrackAsset
    {
        public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
        {
            return ScriptPlayable<UIImageColorMixerBehaviour>.Create(graph, inputCount);
        }
    }
}
