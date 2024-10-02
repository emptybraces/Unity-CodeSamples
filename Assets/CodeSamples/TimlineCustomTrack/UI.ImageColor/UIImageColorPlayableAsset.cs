using System.ComponentModel;
using Timeline.Samples;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Emptybraces.Timeline
{
    [System.Serializable, DisplayName("EmptyBraces/UI/ImageAlpha")]
    public class UIImageColorPlayableAsset : PlayableAsset
    {
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
