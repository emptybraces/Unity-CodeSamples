using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Emptybraces
{
	[RequireComponent(typeof(Image))]
	public class KeyHighlighter : MonoBehaviour
	{
		[SerializeField] KeyCode _keyCode;
		[SerializeField] InputActionReference _inputActionRef;
		[SerializeField] float _power = 0.4f;
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
			if (_inputActionRef != null)
			{
				_inputActionRef.action.Enable();
				if (_isHold ? _inputActionRef.action.IsPressed() : _inputActionRef.action.WasPressedThisFrame())
					Highlight();
			}
			if (_isHold ? Input.GetKey(_keyCode) : Input.GetKeyDown(_keyCode))
			{
				Highlight();
			}
			_image.color = _c;
		}

		public void Highlight()
		{
			_c.a = Mathf.Min(0.7f, _c.a + _power);
		}
	}
}
