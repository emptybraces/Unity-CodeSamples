using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

namespace Emptybraces.Timeline
{
	[System.Serializable]
	public class UIImageColorPlayableBehaviour : PlayableBehaviour
	{
		public Color Color;
		// public bool IsDefaltColorClear = true;
		// Image _trackBinding;
		// Color _defaultColor;
		// public override void ProcessFrame(Playable playable, FrameData info, object playerData)
		// {
		// 	SetDefaults(playerData as Image);
		// 	if (_trackBinding == null)
		// 		return;
		// 	_trackBinding.color = Color.Lerp(IsDefaltColorClear ? Color.clear : _defaultColor, Color, info.weight);
		// }

		// public override void OnPlayableDestroy(Playable playable)
		// {
		// 	RestoreDefaults();
		// }

		// void SetDefaults(Image image)
		// {
		// 	if (image == _trackBinding)
		// 		return;
		// 	_trackBinding = image;
		// 	if (_trackBinding != null)
		// 	{
		// 		_defaultColor = _trackBinding.color;
		// 	}
		// }
		// void RestoreDefaults()
		// {
		// 	if (_trackBinding == null)
		// 		return;
		// 	_trackBinding.color = _defaultColor;
		// }
	}
}
