using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Emptybraces.Timeline
{
    [TrackColor(0.8f, 0.8f, 0.8f)]
    [TrackClipType(typeof(SimpleProgrammablePlayableAsset))]
    [TrackBindingType(typeof(MonoBehaviour), TrackBindingFlags.None)]
    public class SimpleProgrammableTrack : TrackAsset
    {
        [SerializeField] bool _isNotifyOnEditorMode;
        [SerializeField] bool _isNotifyOnlyChangedWeight;
        public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
        {
            var playable = ScriptPlayable<SimpleProgrammableMixerBehaviour>.Create(graph, inputCount);
            var behaviour = playable.GetBehaviour();
            behaviour.IsNotifyOnEditor = _isNotifyOnEditorMode;
            behaviour.IsNotifyOnlyChangedWeight = _isNotifyOnlyChangedWeight;
            return playable;
        }
    }
}
