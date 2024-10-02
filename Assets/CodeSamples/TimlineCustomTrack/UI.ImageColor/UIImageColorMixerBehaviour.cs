using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

namespace Emptybraces.Timeline
{
    public class UIImageColorMixerBehaviour : PlayableBehaviour
    {
        Image _trackBinding;
        Color _defaultColor;
        // Called every frame that the timeline is evaluated. ProcessFrame is invoked after its' inputs.
        public override void ProcessFrame(Playable playable, FrameData info, object playerData)
        {
            SetDefaults(playerData as Image);
            if (_trackBinding == null)
                return;

            var input_count = playable.GetInputCount();
            var blended_color = Color.clear;
            var total_weight = 0f;

            // 0以上の一番小さい値をとれば、単体でのイーズインアウト時、ブレンド時もfrom側のブレンド値となる。
            for (int i = 0; i < input_count; i++)
            {
                var input_weight = playable.GetInputWeight(i);
                var inputPlayable = (ScriptPlayable<UIImageColorPlayableBehaviour>)playable.GetInput(i);
                var input = inputPlayable.GetBehaviour();
                blended_color += input.Color * input_weight;
                total_weight += input_weight;
            }
            _trackBinding.color = Color.Lerp(_defaultColor, blended_color, total_weight);
        }

        // Invoked when the playable graph is destroyed, typically when PlayableDirector.Stop is called or the timeline is complete.
        public override void OnPlayableDestroy(Playable playable)
        {
            RestoreDefaults();
        }

        void SetDefaults(Image image)
        {
            if (image == _trackBinding)
                return;
            _trackBinding = image;
            if (_trackBinding != null)
            {
                _defaultColor = _trackBinding.color;
            }
        }
        void RestoreDefaults()
        {
            if (_trackBinding == null)
                return;
            _trackBinding.color = _defaultColor;
        }
    }
}

