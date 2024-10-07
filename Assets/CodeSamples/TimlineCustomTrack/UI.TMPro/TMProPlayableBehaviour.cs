using System;
using Nnfs.Serializable;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;

namespace Emptybraces.Timeline
{
    // Runtime representation of a TextClip.
    // The Serializable attribute is required to be animated by timeline, and used as a template.
    [Serializable]
    public class TMProPlayableBehaviour : PlayableBehaviour
    {
        public Color Color = Color.white;
        public ToggleInt FontSize = new ToggleInt(24);
        public string Text = "";
        Color _defaultColor;
        float _defaultFontSize;
        string _defaultText;
        TMP_Text m_TrackBinding;
        public override void ProcessFrame(Playable playable, FrameData info, object playerData)
        {
            SetDefaults(playerData as TMP_Text);
            if (m_TrackBinding == null)
                return;
            m_TrackBinding.color = Color.Lerp(Color.clear, Color, info.weight);
            if (FontSize.Enabled)
                m_TrackBinding.fontSize = Mathf.RoundToInt(Mathf.Lerp(_defaultFontSize, FontSize.Value, info.weight));
            m_TrackBinding.text = Text;
        }
        // Invoked when the playable graph is destroyed, typically when PlayableDirector.Stop is called or the timeline
        // is complete.
        public override void OnPlayableDestroy(Playable playable)
        {
            RestoreDefaults();
        }
        public override void OnBehaviourPause(Playable playable, FrameData info)
        {
            RestoreDefaults();
        }
        void SetDefaults(TMP_Text text)
        {
            if (text == m_TrackBinding)
                return;

            RestoreDefaults();

            m_TrackBinding = text;
            if (m_TrackBinding != null)
            {
                _defaultColor = m_TrackBinding.color;
                _defaultFontSize = m_TrackBinding.fontSize;
                _defaultText = m_TrackBinding.text;
            }
        }

        void RestoreDefaults()
        {
            if (m_TrackBinding == null)
                return;

            m_TrackBinding.color = _defaultColor;
            m_TrackBinding.fontSize = _defaultFontSize;
            m_TrackBinding.text = _defaultText;
        }
    }
}
