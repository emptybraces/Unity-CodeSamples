using UnityEngine;
using UnityEngine.Playables;

namespace Emptybraces.Timeline
{
    public class SimpleProgrammableMixerBehaviour : PlayableBehaviour
    {
        ISimpleProgrammablePlayableReceiver _trackBinding;
        float _prevWeight;
        public bool IsNotifyOnEditor, IsNotifyOnlyChangedWeight;
        bool _isPlaying;
        // public override void OnPlayableCreate(Playable playable)
        // {
        //     var current_time = playable.GetTime();
        //     _isPlaying = ClipStart < current_time && current_time < ClipEnd;
        // }

        public override void ProcessFrame(Playable playable, FrameData info, object playerData)
        {
            SetDefaults(playerData as ISimpleProgrammablePlayableReceiver);
            if (_trackBinding == null)
                return;
            var input_count = playable.GetInputCount();
            var current_playing = false;
            for (int i = 0; i < input_count; ++i)
            {
                var input_weight = playable.GetInputWeight(i);
                var input_playable = (ScriptPlayable<SimpleProgrammablePlayableBehaviour>)playable.GetInput(i);
                var input = input_playable.GetBehaviour();

                current_playing = input.IsPlaying;
                if (current_playing)
                {
                    if (Application.isPlaying || IsNotifyOnEditor)
                    {
                        if (!IsNotifyOnlyChangedWeight || input_weight != _prevWeight)
                        {
                            var is_enter = false;
                            if (!_isPlaying)
                            {
                                _isPlaying = true;
                                is_enter = true;
                            }
                            _trackBinding.OnNotify(input_weight, is_enter, false);
                        }
                    }
                    _prevWeight = input_weight;
                    break;
                }
            }
            if (_isPlaying && !current_playing)
            {
                if (Application.isPlaying || IsNotifyOnEditor)
                {
                    _prevWeight = 0;
                    _isPlaying = false;
                    _trackBinding.OnNotify(0, false, true);
                }
            }
        }

        void SetDefaults(ISimpleProgrammablePlayableReceiver receiver)
        {
            if (receiver == _trackBinding)
                return;

            _trackBinding = receiver;
            if (_trackBinding != null)
            {
            }
        }
    }
}

