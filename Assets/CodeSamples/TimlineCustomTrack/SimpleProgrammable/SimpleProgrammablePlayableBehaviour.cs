using UnityEngine;
using UnityEngine.Playables;

namespace Emptybraces.Timeline
{
	[System.Serializable]
	public class SimpleProgrammablePlayableBehaviour : PlayableBehaviour
	{
		public bool IsNotifyOnEditor, IsNotifyOnlyChangedWeight;
		public bool IsPlaying { get; private set; }
		ISimpleProgrammablePlayableReceiver _trackBinding;
		float _prevWeight;
		bool _isPlaying, _isEnter, _isExit;
		public override void OnBehaviourPlay(Playable playable, FrameData info)
		{
			_isEnter = !_isPlaying;
			_isPlaying = true;
		}

		public override void OnBehaviourPause(Playable playable, FrameData info)
		{
			if (info.weight <= 0)
			{
				_isExit = _isPlaying;
				_isPlaying = false;
				_trackBinding?.OnNotify(1, false, true);
			}
		}

		public override void ProcessFrame(Playable playable, FrameData info, object playerData)
		{
			SetDefaults(playerData as ISimpleProgrammablePlayableReceiver);
			if (_trackBinding == null)
				return;
			if (Application.isPlaying || IsNotifyOnEditor)
			{
				var current_weight = info.weight;
				if (_isEnter)
				{
					_isEnter = false;
					_trackBinding.OnNotify(0, true, false);
				}
				else if (_isExit)
				{
					_isExit = false;
					_trackBinding.OnNotify(1, false, true);
				}
				else if (!IsNotifyOnlyChangedWeight || current_weight != _prevWeight)
				{
					_trackBinding.OnNotify(current_weight, false, false);
				}
				_prevWeight = current_weight;
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
