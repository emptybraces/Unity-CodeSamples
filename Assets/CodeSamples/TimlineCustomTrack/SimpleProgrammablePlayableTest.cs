using UnityEngine;
namespace Emptybraces.Timeline
{
	public class SimpleProgrammablePlayableTest : MonoBehaviour, ISimpleProgrammablePlayableReceiver
	{
		[SerializeField] float _speed;
		[SerializeField] float _power;
		Rigidbody _rb;
		Vector3 _p;
		void Awake()
		{
			_rb = transform.GetComponentInChildren<Rigidbody>(true);
			_rb.gameObject.SetActive(false);
			_p = transform.position;
		}
		public void OnNotify(float value, bool isEnter, bool isExit)
		{
			if (!Application.isPlaying)
				_rb = transform.GetComponentInChildren<Rigidbody>(true);
			if (isEnter)
				_rb.gameObject.SetActive(true);
			else if (isExit)
				_rb.gameObject.SetActive(false);
			_rb.transform.localScale = Mathf.Lerp(0.1f, 2, value) * Vector3.one;
			var pos = _p;
			pos.x += (Mathf.PerlinNoise(Time.time * _speed, .1f + Time.time * _speed) - 0.5f) * _power;
			pos.y += (Mathf.PerlinNoise(.2f + Time.time * _speed, .3f + Time.time * _speed) - 0.5f) * _power;
			if (!Application.isPlaying)
				_rb.transform.localPosition = pos;
			else
				_rb.MovePosition(pos);
			cn.logBluef(value, isEnter, isExit);
		}
	}
}
