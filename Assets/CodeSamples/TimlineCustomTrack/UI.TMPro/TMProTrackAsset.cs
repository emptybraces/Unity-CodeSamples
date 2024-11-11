
using TMPro;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Emptybraces.Timeline
{
    // A track that allows the user to change Text parameters from a Timeline.
    // It demonstrates the following
    //  * How to support blending of timeline clips.
    //  * How to change data over time on Components that is not supported by Animation.
    //  * Putting properties into preview mode.
    //  * Reacting to changes on the clip from the Timeline Editor.
    // Note: This track requires the TextMeshPro package to be installed in the project.
    [TrackColor(0.1394896f, 0.4411765f, 0.3413077f)]
    [TrackClipType(typeof(TMProPlayableAsset))]
    [TrackBindingType(typeof(TMP_Text))]
    public class TMProTrackAsset : TrackAsset
    {
        // Creates a runtime instance of the track, represented by a PlayableBehaviour.
        // The runtime instance performs mixing on the timeline clips.
        // public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
        // {
        //     return ScriptPlayable<TextPlayableBehaviour>.Create(graph, inputCount);
        // }

        // Invoked by the timeline editor to put properties into preview mode. This permits the timeline
        // to temporarily change fields for the purpose of previewing in EditMode.
        public override void GatherProperties(PlayableDirector director, IPropertyCollector driver)
        {
            TMP_Text trackBinding = director.GetGenericBinding(this) as TMP_Text;
            if (trackBinding == null)
                return;

            // The field names are the name of the backing serializable field. These can be found from the class source,
            // or from the unity scene file that contains an object of that type.
            driver.AddFromName<TMP_Text>(trackBinding.gameObject, "m_text");
            driver.AddFromName<TMP_Text>(trackBinding.gameObject, "m_fontSize");
            driver.AddFromName<TMP_Text>(trackBinding.gameObject, "m_fontColor");

            base.GatherProperties(director, driver);
        }
    }
}

