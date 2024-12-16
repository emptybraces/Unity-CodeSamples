
using UnityEngine;
using UnityEngine.Playables;
using UniVRM10;

namespace Emptybraces.Timeline
{
	public class VRM10ExpressionMixerBehaviour : PlayableBehaviour
	{
		Vrm10Instance _vrm;
		public override void ProcessFrame(Playable playable, FrameData info, object playerData)
		{
			if (!Application.isPlaying)
				return;
			var arg = playerData as Vrm10Instance;
			if (_vrm != arg)
				_vrm = arg;
			if (_vrm == null)
				return;
			var input_count = playable.GetInputCount();
			for (int i = 0; i < input_count; i++)
			{
				var input_weight = playable.GetInputWeight(i);
				var inputPlayable = (ScriptPlayable<VRM10ExpressionPlayableBehaviour>)playable.GetInput(i);
				var input = inputPlayable.GetBehaviour();
				_vrm.Runtime.Expression.SetWeight(input.Key, input_weight);
			}
		}
	}
}

