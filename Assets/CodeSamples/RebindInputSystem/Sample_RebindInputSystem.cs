using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.UI;

namespace Emptybraces.RebindInputSystemSample
{
	public class Sample_RebindInputSystem : MonoBehaviour
	{
		[SerializeField] GameObject _keyContents;
		[SerializeField] TMPro.TextMeshProUGUI _tmInput;
		[SerializeField] Text _tmTime;
		[SerializeField] Transform _parentKeymou, _parentPad;
		[SerializeField] string[] _mouseDeviceExcludeActions = new string[] { "Navigate", "Move" };
		[SerializeField, Min(3)] float _waitInputTime = 10f;
		[SerializeField] Text _tmRebindMessage;
		[SerializeField] KeyHighlighter _highlighter;

		SampleActionMap _actionMap;
		const string _schemaKeymou = "Keymou";
		const string _schemaPad = "Pad";
		InputActionRebindingExtensions.RebindingOperation _rebindOperation;
		Dictionary<string, TMPro.TextMeshProUGUI> _bindingToTMPro = new();
		bool _isBusy;
		float _waitInputStartTime;
		IDisposable _onAnyKeyDownListener;

		void Awake()
		{
			_actionMap = new SampleActionMap();
			_actionMap.Enable();
			_actionMap.UI.Navigate.performed += __OnInput;
			_actionMap.Main.Interact.performed += __OnInput;
			_tmRebindMessage.transform.parent.gameObject.SetActive(false);
			if (EventSystem.current != null)
			{
				var module = EventSystem.current.GetComponent<InputSystemUIInputModule>();
				module.actionsAsset = _actionMap.asset;
			}

			void __OnInput(InputAction.CallbackContext ctx)
			{
				_tmInput.text = $"Pressed {ctx.action.name}: {ctx.control.displayName}, Path: {ctx.control.path}";
				_highlighter.Highlight();
			}
		}

		void Start()
		{
			_InstantiateBySchema(_schemaKeymou, _parentKeymou);
			_InstantiateBySchema(_schemaPad, _parentPad);
			Destroy(_keyContents.gameObject);
		}

		void OnEnable()
		{
			if (!string.IsNullOrEmpty(SaveLoad.Data.OverrideBindingJson))
				_actionMap.LoadBindingOverridesFromJson(SaveLoad.Data.OverrideBindingJson);
		}

		void OnDisable()
		{
			_onAnyKeyDownListener?.Dispose();
			SaveLoad.Data.OverrideBindingJson = _actionMap.SaveBindingOverridesAsJson();
			SaveLoad.Save();
			_rebindOperation?.Cancel();
			_rebindOperation?.Dispose();
		}

		void Update()
		{
			if (0 < _waitInputStartTime)
			{
				var t = Mathf.Max(_waitInputStartTime - Time.unscaledTime, 0);
				_tmTime.text = $"{t:f1}";
				if (t == 0)
					_waitInputStartTime = 0;
			}
		}

		public void ResetOverrideBindingAll()
		{
			foreach (var actionMap in _actionMap.asset.actionMaps)
				foreach (var a in actionMap.actions)
					ResetOverrideBinding(a);

		}
		public void ResetOverrideBinding(InputAction action, int bindingIndex = -1)
		{
			if (bindingIndex == -1)
				action.RemoveAllBindingOverrides();
			else
				action.RemoveBindingOverride(bindingIndex);
			foreach (var binding in action.bindings)
			{
				var key = _GetKey(action, binding);
				if (_bindingToTMPro.TryGetValue(key, out var tm))
				{
					_SetBindingLabel(tm, action, binding);
				}
			}
		}

		void _InstantiateBySchema(string schema, Transform parent)
		{
			var g = Instantiate(_keyContents, parent);
			var tm = g.transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>();
			tm.text = $"Scheme: {schema}";
			Destroy(g.transform.GetChild(1).gameObject); // reset button
			foreach (var actionMap in _actionMap.asset.actionMaps)
			{
				g = Instantiate(_keyContents, parent);
				tm = g.transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>();
				tm.text = $"  ActionMap: {actionMap.name}";
				Destroy(g.transform.GetChild(1).gameObject); // reset button
				foreach (var action in actionMap.actions)
				{
					foreach (var binding in action.bindings)
					{
						if (binding.groups != null && binding.groups.Contains(schema))
						{
							g = Instantiate(_keyContents, parent);
							tm = g.transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>();
							_SetBindingLabel(tm, action, binding);
							tm.GetComponent<Button>().onClick.AddListener(() =>
							{
								_PerformInteractiveRebinding(action, binding, schema);
							});
							_bindingToTMPro.Add(_GetKey(action, binding), tm);
							g.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(() =>
							{
								ResetOverrideBinding(action, _GetBindingIndex(action, binding));
							});
						}
					}
				}
			}
		}

		void _PerformInteractiveRebinding(InputAction action, InputBinding binding, string scheme)
		{
			cn.logf();
			if (_isBusy) // キーボードは問題なし、スイッチコンだと決定キーおすともう一度始まってしまうため、1フレーム待つ
				return;
			_isBusy = true;
			_rebindOperation?.Cancel();
			_actionMap.Disable();
			var binding_id = binding.id;
			var binding_idx = action.bindings.IndexOf(x => x.id == binding_id);
			var overlay = _tmRebindMessage.transform.parent.gameObject;
			binding = action.bindings[binding_idx]; // 最新の設定内容のbindingを取得
			var rebind_msg = binding.isPartOfComposite
				? $"<{action.name}/{binding.name}>: 新しいキーを入力してください..."
				: $"<{action.name}>: 新しいキーを入力してください...";
			if (scheme == _schemaKeymou)
				rebind_msg += "\n(Escでキャンセル)";
			_tmRebindMessage.text = rebind_msg;
			overlay.SetActive(true);
			_waitInputStartTime = Time.unscaledTime + _waitInputTime;
			cn.log("PerformInteractiveRebinding call");
			_rebindOperation = action.PerformInteractiveRebinding(binding_idx)
				.WithCancelingThrough("<Keyboard>/escape")
				.OnMatchWaitForAnother(.2f) // 少し待ってから最後に入力があったものを確定する
				.WithTimeout(_waitInputTime)
				.OnCancel(
					operation =>
					{
						cn.log("OnCancel");
						__CleanUp(true);
					})
				.OnComplete(
					operation =>
					{
						cn.log("OnComplete");
						__CleanUp(false);
					})

				.OnPotentialMatch(
					operation =>
					{
						var new_binding_control = operation.selectedControl;
						cn.log("OnPotentialMatch", new_binding_control, _rebindOperation.expectedControlType);
						// 重複キーアサインのチェック
						string exist_action_binding = null;
						foreach (var a in action.actionMap.actions) // 同じアクションマップのみを対象とする
						{
							foreach (var b in a.bindings)
							{
								// 違うスキーマは対象外とする
								if (b.groups == null || !b.groups.Contains(scheme))
									continue;
								// 同じBindingは対象外とする
								if (b.effectivePath == binding.effectivePath)
									continue;
								var device_name = InputControlPath.TryGetDeviceLayout(b.effectivePath);
								var b_short_path = b.effectivePath.Replace($"<{device_name}>/", "");
								var new_binding_short_path = InputControlPath.ToHumanReadableString(new_binding_control.path, InputControlPath.HumanReadableStringOptions.OmitDevice);
								if (_IsBindingConflicting(b_short_path, new_binding_short_path))
								{
									cn.log("重複キー検出。", "既存登録キー=", b.effectivePath, "入力されたキー=", new_binding_short_path);
									exist_action_binding = b.isPartOfComposite ? $"{a.name}/{b.name}" : a.name; ;
									break;
								}
							}
						}
						if (exist_action_binding != null)
						{
							operation.Cancel();
							cn.log("_PerformInteractiveRebinding RESTART");
							_PerformInteractiveRebinding(action, binding, scheme); // 再帰的に再スタート
							_tmRebindMessage.text += $"<color=red>\n重複キー({exist_action_binding})を検出しました。違うキーを設定してください。</color>";
						}
					});
			// デバイス名は山かっこ
			if (_mouseDeviceExcludeActions.Contains(action.name))
				_rebindOperation.WithControlsExcluding("<Mouse>");
			if (scheme == _schemaKeymou)
				_rebindOperation.WithControlsExcluding("<Gamepad>")
					.WithControlsExcluding("<Keyboard>/leftMeta") // windowsキーを無効にし、フォールバックされたanykeyも無効にする
					.WithControlsExcluding("<Keyboard>/rightMeta")
					.WithControlsExcluding("<Keyboard>/anyKey");
			if (scheme == _schemaPad)
				_rebindOperation.WithControlsExcluding("<Keyboard>").WithControlsExcluding("<Mouse>");
			_rebindOperation.Start();
			void __CleanUp(bool isCancelled)
			{
				overlay.SetActive(false);
				// refresh text
				if (!isCancelled)
				{
					binding = action.bindings[binding_idx];
					var tm = _bindingToTMPro[_GetKey(action, binding)];
					_SetBindingLabel(tm, action, binding);
					_rebindOperation?.Dispose();
					_rebindOperation = null;
					StartCoroutine(__DelayEnable()); // キーボードは問題なし、スイッチコンだと決定キーおすともう一度始まってしまうため、1フレーム待つ
					IEnumerator __DelayEnable()
					{
						yield return null;
						_isBusy = false;
					}
				}
				// キャンセル時
				else
				{
					_isBusy = false;
				}
				_actionMap.Enable();
				// var module = EventSystem.current.GetComponent<InputSystemUIInputModule>();
				// var actionsAsset = module.actionsAsset;
				// var navigateAction = actionsAsset.FindAction("Navigate");
				// var bindings = navigateAction.bindings;
				// for (int i = 0; i < bindings.Count; i++)
				// {
				// 	var b = bindings[i];
				// 	Debug.Log($"[{i}] name: {b.name}, path: {b.effectivePath}, override: {b.overridePath}, isPartOfComposite: {b.isPartOfComposite}");
				// }
			}
		}
		
		bool _IsBindingConflicting(string pathA, string pathB)
		{
			return pathA == pathB || pathA.StartsWith(pathB + "/") || pathB.StartsWith(pathA + "/");
		}

		string _GetKey(InputAction a, InputBinding b) => $"{a.actionMap.name}/{b.action}|{b.name}|{b.path}|{b.groups}";

		void _SetBindingLabel(TMPro.TextMeshProUGUI tm, InputAction a, InputBinding b)
		{
			var binding_idx = _GetBindingIndex(a, b);
			var display = a.GetBindingDisplayString(binding_idx);
			var sprite_tag = "";
			// effectivepathはキーボードの場合、キー印字と取得できるIDが一致しない。GetBindingDisplayStringは一致する。
			if (b.groups.Contains(_schemaKeymou))
				sprite_tag = $"<sprite name=\"{display}\">";
			else
			{
				// GetBindingDisplayStringを使うと、AとかBとかキーボタンがキーボードと被る。
				// b.effectivepathは"Submit"などの仮想パスが取得されてしまう。

				// デバイス未接続時はマッチしなくなる。
				// 例えば<Gamepad>/{Submit}から、/SwitchProControllerHID/ButtonEastを取得するにはデバイスを接続してないと無理そう。
				// デバイス未接続時はアイコン非表示にしたり対応すれば良さそう。
				var c = a.controls.FirstOrDefault(c => InputControlPath.Matches(b.effectivePath, c));
				if (c != null)
				{
					var splits = c.path.Split('/', System.StringSplitOptions.RemoveEmptyEntries);
					if (1 < splits.Length)
					{
						var path = string.Join("/", splits, 1, splits.Length - 1);
						sprite_tag = $"<sprite name=\"{path}\">"; // c.nameは末尾のみ取得
					}
				}
			}
			if (b.isPartOfComposite) // comporite partじゃなければb.nameはnull
				tm.text = $"   - Action: {a.name}/{b.name.ToUpperInvariant()}, Binding: {display} {sprite_tag}";
			else
				tm.text = $"   - Action: {a.name}, Binding: {display} {sprite_tag}";
			LayoutRebuilder.ForceRebuildLayoutImmediate(tm.rectTransform);
		}

		int _GetBindingIndex(InputAction a, InputBinding b)
		{
			var binding_id = b.id;
			return a.bindings.IndexOf(x => x.id == binding_id);
		}
	}
}
