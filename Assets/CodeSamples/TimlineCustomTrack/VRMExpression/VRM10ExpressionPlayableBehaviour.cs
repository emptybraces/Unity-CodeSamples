using UnityEngine.Playables;
using UniVRM10;

namespace Emptybraces.Timeline
{
	[System.Serializable]
	public class VRM10ExpressionPlayableBehaviour : PlayableBehaviour
	{
		public ExpressionPreset PresetKey;
		public string CustomKey;
		public ExpressionKey Key { get; private set; }
		public override void OnPlayableCreate(Playable playable)
		{
			if (PresetKey == ExpressionPreset.custom && CustomKey == "")
			{
				cn.logw("カスタムIDを入力してください");
				return;
			}
			Key = new ExpressionKey(PresetKey, CustomKey);
		}
	}
}
