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
        // public ClipCaps clipCaps
        // {
        //     get { return ClipCaps.Blending; }
        // }
        // public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
        // {
        //     var playable = ScriptPlayable<SimpleProgrammablePlayableBehaviour>.Create(graph, inputCount);
        //     var behaviour = playable.GetBehaviour();
        //     behaviour.IsNotifyOnEditor = _isNotifyOnEditorMode;
        //     behaviour.IsNotifyOnlyChangedWeight = _isNotifyOnlyChangedWeight;
        //     return playable;
        // }
        // public override void GatherProperties(PlayableDirector director, IPropertyCollector driver)
        // {
        //     var trackBinding = director.GetGenericBinding(this) as MonoBehaviour;
        //     if (trackBinding == null)
        //         return;

        //     // The field names are the name of the backing serializable field. These can be found from the class source,
        //     // or from the unity scene file that contains an object of that type.
        //     driver.AddFromName<TMP_Text>(trackBinding.gameObject, "m_text");
        //     driver.AddFromName<TMP_Text>(trackBinding.gameObject, "m_fontSize");
        //     driver.AddFromName<TMP_Text>(trackBinding.gameObject, "m_fontColor");

        //     base.GatherProperties(director, driver);
        // }
    }
}
