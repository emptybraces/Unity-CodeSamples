using UnityEngine;
namespace Emptybraces.MsgSample
{
	public class ObjectCounterText : MonoBehaviour
	{
		TMPro.TextMeshProUGUI _text;
		int _current;

		void Start()
		{
			_text = GetComponent<TMPro.TextMeshProUGUI>();
			gameObject.SetActive(false);
			Msg.Set<Sample_Msg>(MsgId.OnInitedLongTime, OnInitedLongTime);
			Msg.Set<int>(MsgId.OnCreated, OnCreated);
			Msg.Set<int>(MsgId.OnRemoved, OnRemoved);
		}
		void OnDestroy()
		{
			Msg.Unset<Sample_Msg>(MsgId.OnInitedLongTime, OnInitedLongTime);
			Msg.Unset<int>(MsgId.OnCreated, OnCreated);
			Msg.Unset<int>(MsgId.OnRemoved, OnRemoved);
		}

		void Update()
		{
			_text.SetText("{0}", _current);
		}

		void OnInitedLongTime(Sample_Msg arg)
		{
			gameObject.SetActive(true);
		}

		void OnCreated(int cnt) => _current += cnt;
		void OnRemoved(int cnt) => _current -= cnt;
	}
}
