using UnityEngine;
using UnityEngine.Playables;

namespace Emptybraces.Timeline
{
    [System.Serializable]
    public class UIImageColorPlayableBehaviour : PlayableBehaviour
    {
        public Color Color;
        // // Called when the owning graph starts playing
        // public override void OnGraphStart(Playable playable)
        // {
        //     cn.log("start");
        // }

        // // Called when the owning graph stops playing
        // public override void OnGraphStop(Playable playable)
        // {
        //     cn.log("stop");
        // }

        // // Called when the state of the playable is set to Play
        // public override void OnBehaviourPlay(Playable playable, FrameData info)
        // {
        //     cn.log("Play");
        // }

        // // Called when the state of the playable is set to Paused
        // public override void OnBehaviourPause(Playable playable, FrameData info)
        // {
        //     cn.log("Pasuse");
        // }

        // // Called each frame while the state is set to Play
        // public override void PrepareFrame(Playable playable, FrameData info)
        // {
        //     cn.log("Frame");
        // }
    }
}
