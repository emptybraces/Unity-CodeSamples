using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Emptybraces.TextFileParserSample
{
	public class Sample_TextFileParser : MonoBehaviour
	{
		[System.Serializable]
		class WordListField
		{
			public TextAsset TextAsset;
			public Button RunButton;
			public Toggle UseSpanToggle;
			public Text ResultView;
		}
		[SerializeField] WordListField _wordListField;

		[System.Serializable]
		class CustomFormatField
		{
			public TextAsset TextAsset;
			public Button RunButton;
			public Transform Parent;
			public Button SetionButton;
		}
		[SerializeField] CustomFormatField _customFormatField;

		void Start()
		{
			_wordListField.RunButton.onClick.AddListener(_OnClickButtonWordListLoad);
			_customFormatField.RunButton.onClick.AddListener(_OnClickButtonCustomFormatLoad);
		}

		void OnDestroy()
		{
			foreach (var i in CustomFormat.Data.Values)
				foreach (var ii in i)
					ii.OnDestroy();
		}
		void Update()
		{
			foreach (var i in CustomFormat.Data.Values)
				foreach (var ii in i)
					ii.Update();
		}

		void _OnClickButtonWordListLoad()
		{
			if (_wordListField.UseSpanToggle.isOn)
				Word.LoadFromTextWithSpan(_wordListField.TextAsset.text);
			else
				Word.LoadFromText(_wordListField.TextAsset.text);
			var sb = new StringBuilder();
			sb.AppendLine(Word.Get("settings/language_ja"));
			sb.AppendLine(Word.Get("settings/language_en"));
			sb.AppendLine(Word.Get("day_fmt", System.DateTime.Now.Month, System.DateTime.Now.Day));
			sb.AppendLine(Word.GetArray("status/params")[0]);
			sb.AppendLine(Word.GetArray("status/params")[1]);
			sb.AppendLine(Word.GetArray("status/params")[2]);
			_wordListField.ResultView.text = sb.ToString();
		}
		void _OnClickButtonCustomFormatLoad()
		{
			var button_parent = _customFormatField.SetionButton.transform.parent;
			while (1 < button_parent.childCount)
				DestroyImmediate(button_parent.GetChild(button_parent.childCount - 1).gameObject);
			CustomFormat.LoadFromText(_customFormatField.TextAsset.text, _customFormatField.Parent);
			foreach (var i in CustomFormat.Data)
			{
				var b = Instantiate(_customFormatField.SetionButton, button_parent);
				b.gameObject.name = i.Key;
				b.GetComponentInChildren<Text>().text = i.Key;
				b.gameObject.SetActive(true);
				b.onClick.AddListener(() => _OnClickButtonSection(i.Key));
			}
		}
		void _OnClickButtonSection(string key)
		{
			foreach (var i in CustomFormat.Data[key])
				i.Run();
		}

#if UNITY_EDITOR
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		static void _DomainReset()
		{
		}
#endif
	}
}
