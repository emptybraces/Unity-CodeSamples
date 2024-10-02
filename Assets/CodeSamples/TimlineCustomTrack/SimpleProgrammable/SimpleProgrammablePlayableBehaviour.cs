using UnityEngine.Playables;

namespace Emptybraces.Timeline
{
    [System.Serializable]
    public class SimpleProgrammablePlayableBehaviour : PlayableBehaviour
    {
        public bool IsPlaying { get; private set; }
        public override void OnBehaviourPlay(Playable playable, FrameData info)
        {
            IsPlaying = true;
        }

        public override void OnBehaviourPause(Playable playable, FrameData info)
        {
            IsPlaying = false;
        }
    }
}
