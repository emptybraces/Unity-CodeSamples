using UnityEngine;
using UnityEngine.UI;

namespace EmptyBraces
{
	[RequireComponent(typeof(Image))]
	public class KeyHighlighter : MonoBehaviour
	{
		[SerializeField] KeyCode _keyCode;
		[SerializeField] float _power = 0.5f;
		[SerializeField] bool _isHold;
		Image _image;
		Color _c;
		void Awake()
		{
			_image = GetComponent<Image>();
			_c = _image.color;
		}

		void Update()
		{
			_c.a = Mathf.Max(0, _c.a -= Time.deltaTime * 3);
			if (_isHold ? Input.GetKey(_keyCode) : Input.GetKeyDown(_keyCode))
			{
				_c.a = Mathf.Min(1, _c.a + _power);
			}
			_image.color = _c;
		}
	}
}