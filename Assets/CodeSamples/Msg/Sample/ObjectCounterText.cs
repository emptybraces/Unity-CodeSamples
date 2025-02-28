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
			Msg.Set(MsgId.OnCreated, OnCreated);
			Msg.Set(MsgId.OnRemoved, OnRemoved);
		}
		void OnDestroy()
		{
			Msg.Unset<Sample_Msg>(MsgId.OnInitedLongTime, OnInitedLongTime);
			Msg.Unset(MsgId.OnCreated, OnCreated);
			Msg.Unset(MsgId.OnRemoved, OnRemoved);
		}

		void Update()
		{
			_text.SetText("{0}", _current);
		}

		void OnInitedLongTime(Sample_Msg arg)
		{
			gameObject.SetActive(true);
		}

		void OnCreated() => ++_current;
		void OnRemoved() => --_current;
	}
}
