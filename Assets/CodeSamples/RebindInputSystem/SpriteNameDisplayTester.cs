using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

namespace Emptybraces.RebindInputSystemSample
{
	public class SpriteNameDisplayTester : MonoBehaviour
	{
		[SerializeField] TextMeshProUGUI _outputText;
		InputAction _anyKeyAction;

		void OnEnable()
		{
			// 任意キー入力検出用アクションを用意（既存の InputActionAsset からも可）
			_anyKeyAction = new InputAction("AnyKey", binding: "<Keyboard>/anyKey");
			_anyKeyAction.performed += _OnAnyKey;
			_anyKeyAction.Enable();
		}

		void OnDisable()
		{
			_anyKeyAction.performed -= _OnAnyKey;
			_anyKeyAction.Disable();
		}

		void _OnAnyKey(InputAction.CallbackContext context)
		{
			InputControl control = context.control;
			string path = control.path; // e.g. "<Keyboard>/space"
			string deviceName = control.device.name; // "Keyboard"
			string keyName = path.Replace($"<{deviceName}>/", ""); // e.g. "space"

			Debug.Log($"Key Pressed: {keyName}");

			if (_outputText != null)
			{
				_outputText.text = $"<sprite name=\"{keyName}\">  [{keyName}]";
			}
		}
	}
}