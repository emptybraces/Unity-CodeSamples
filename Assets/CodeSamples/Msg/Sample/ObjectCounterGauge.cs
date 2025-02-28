using UnityEngine;
using UnityEngine.UI;
namespace Emptybraces.MsgSample
{
	public class ObjectCounterGauge : MonoBehaviour
	{
		Image _gauge;
		int _target;
		float _current;
		int _max;

		void Start()
		{
			gameObject.SetActive(false);
			_gauge = GetComponentInChildren<Image>();
			_gauge.fillAmount = 0;
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
			float v = 0;
			_current = Mathf.SmoothDamp(_current, _target, ref v, .03f);
			_gauge.fillAmount = _current / _max;
		}

		void OnInitedLongTime(Sample_Msg arg)
		{
			gameObject.SetActive(true);
			_max = arg.Max;
		}

		void OnCreated() => ++_target;
		void OnRemoved() => --_target;
	}
}
