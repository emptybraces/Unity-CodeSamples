using UnityEngine;
using UnityEngine.Timeline;

namespace Emptybraces.Timeline
{
    [TrackColor(0.1f, 0.4f, 0.5f)]
    [TrackClipType(typeof(ObjectShakePlayableAsset))]
    [TrackBindingType(typeof(Transform))]
    public class ObjectShakeTrack : TrackAsset
    {
    }
}
