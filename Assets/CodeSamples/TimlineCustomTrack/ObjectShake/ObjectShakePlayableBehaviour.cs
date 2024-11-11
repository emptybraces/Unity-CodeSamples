using UnityEngine;
using UnityEngine.Playables;

namespace Emptybraces.Timeline
{
	[System.Serializable]
	public class ObjectShakePlayableBehaviour : PlayableBehaviour
	{
		public float Amplitude = 1;
		public float Frequency = 1;
		Transform _target;
		Vector3 _origin;

		public override void ProcessFrame(Playable playable, FrameData info, object playerData)
		{
			SetDefaults(playerData as Transform);
			if (_target == null)
				return;

			var time = Time.time * Frequency;
			var vibration = new Vector3(
				(Mathf.PerlinNoise(time, 0) - 0.5f) * 2 * Amplitude, 
				(Mathf.PerlinNoise(0, time) - 0.5f) * 2 * Amplitude, 
				(Mathf.PerlinNoise(time, time) - 0.5f) * 2 * Amplitude);
			_target.position = _origin + vibration * info.weight;
		}

		public override void OnPlayableDestroy(Playable playable)
		{
			RestoreDefaults();
		}

		void SetDefaults(Transform t)
		{
			if (t == _target)
				return;
			_target = t;
			if (_target != null)
			{
				_origin = _target.position;
			}
		}
		void RestoreDefaults()
		{
			if (_target == null)
				return;
			_target.position = _origin;
		}
	}
}
