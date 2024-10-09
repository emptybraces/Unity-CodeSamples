
using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using System.ComponentModel;

namespace Emptybraces.Timeline
{
    // Represents the serialized data for a clip on the TextTrack
    [Serializable, DisplayName("EmptyBraces/UI/TMPro")]
    public class TMProPlayableAsset : PlayableAsset, ITimelineClipAsset
    {
        [NoFoldOut]
        [NotKeyable] // NotKeyable used to prevent Timeline from making fields available for animation.
        public TMProPlayableBehaviour template = new TMProPlayableBehaviour();

        // Implementation of ITimelineClipAsset. This specifies the capabilities of this timeline clip inside the editor.
        public ClipCaps clipCaps
        {
            get { return ClipCaps.Blending; }
        }

        // Creates the playable that represents the instance of this clip.
        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            // Using a template will clone the serialized values
            return ScriptPlayable<TMProPlayableBehaviour>.Create(graph, template);
        }
    }
}
