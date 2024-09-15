using System;
using UnityEngine;
using TMPro;
namespace Emptybraces.CoordInterchange
{
	public class Main : MonoBehaviour
	{
		[SerializeField] Transform _cursor3d;
		[SerializeField] GameObject _3dPlane;
		[SerializeField] RectTransform _overlayCanvasCursor;
		[SerializeField] RectTransform _overlayCanvasTargetRect;
		[SerializeField] RectTransform _screenSpaceCanvasCursor;
		[SerializeField] RectTransform _screenSpaceCanvasTargetRect;
		[SerializeField] UnityEngine.UI.Text _uiText;
		Canvas _screenSpaceCanvas;
		[SerializeField] TMP_Text _tmCoord;
		[SerializeField] float _boxExtent = .5f;
		int _idx = 0;
		(string name, Action update, Action<bool> enterExit)[] _procs;

		void Start()
		{
			_screenSpaceCanvas = _screenSpaceCanvasCursor.GetComponentInParent<Canvas>(true);
			_procs = new (string, Action, Action<bool>)[] {
				("１．マウス位置をワールド位置へ変換", () => {
					var sp = Input.mousePosition;
					sp.z = Mathf.Lerp(1, 10, Mathf.InverseLerp(-1, 1, Mathf.Sin(Time.time * 2)));
					var wp = Camera.main.ScreenToWorldPoint(sp);
					_cursor3d.position = wp;
				}, null),
				("２．マウス位置でレイキャストしてワールド位置へ変換", () => {
					if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out var hit, 100, LayerMask.GetMask("raycast")))
					{
						_cursor3d.position = hit.point + hit.normal * _boxExtent;
						_cursor3d.forward = hit.normal;
						_tmCoord.color = Color.red;
					}
				}, isEnter => _3dPlane.SetActive(isEnter)),
				("３．マウス位置をオーバーレイキャンバス位置へ変換", () => {
					if (RectTransformUtility.RectangleContainsScreenPoint(_overlayCanvasTargetRect, Input.mousePosition, null))
					{
						if (RectTransformUtility.ScreenPointToLocalPointInRectangle(_overlayCanvasTargetRect, Input.mousePosition, null, out var point))
						{
							_overlayCanvasCursor.anchoredPosition = point;
							_tmCoord.color = Color.red;
						}
					}
				}, isEnter => __OverlayCanvasEnable(isEnter)),
				("４．マウス位置をスクリーンスペースキャンバス位置へ変換", () => {
					if (RectTransformUtility.RectangleContainsScreenPoint(_screenSpaceCanvasTargetRect, Input.mousePosition, _screenSpaceCanvas.worldCamera))
					{
						if (RectTransformUtility.ScreenPointToWorldPointInRectangle(_screenSpaceCanvasTargetRect, Input.mousePosition, _screenSpaceCanvas.worldCamera, out var point))
						{
							_screenSpaceCanvasCursor.position = point;
							_tmCoord.color = Color.red;
						}
					}
				}, isEnter => __ScreenSpaceCanvasEnable(isEnter)),
				("５．ワールド位置をオーバーレイキャンバス位置へ変換", () => {
					// カーソル3dをランダムに動かす
					_cursor3d.position = __RandomMove(new Vector3(11, 6, 7), .5f);
					// カーソル2dの位置を3dと同期させる
					var sp = Camera.main.WorldToScreenPoint(_cursor3d.position);
					if (RectTransformUtility.RectangleContainsScreenPoint(_overlayCanvasTargetRect, sp, null))
					{
						if (RectTransformUtility.ScreenPointToLocalPointInRectangle(_overlayCanvasTargetRect, sp, null, out var point))
						{
							_overlayCanvasCursor.anchoredPosition = point;
							_tmCoord.color = Color.red;
						}
					}
				}, isEnter => __OverlayCanvasEnable(isEnter)),
				("６．ワールド位置をスクリーンスペースキャンバス位置へ変換", () => {
					// カーソル3dをランダムに動かす
					_cursor3d.position = __RandomMove(new Vector3(7, 3, 4), .5f);
					// カーソル2dの位置を3dと同期させる
					var sp = _screenSpaceCanvas.worldCamera.WorldToScreenPoint(_cursor3d.position);
					if (RectTransformUtility.RectangleContainsScreenPoint(_screenSpaceCanvasTargetRect, sp, _screenSpaceCanvas.worldCamera))
					{
						if (RectTransformUtility.ScreenPointToWorldPointInRectangle(_screenSpaceCanvasTargetRect, sp, _screenSpaceCanvas.worldCamera, out var point))
						{
							_screenSpaceCanvasCursor.position = point;
							_tmCoord.color = Color.red;
						}
					}
				}, isEnter => __ScreenSpaceCanvasEnable(isEnter)),
				("７．オーバーレイキャンバスからワールド位置へ変換", () => {
					// カーソル2dをランダムに動かす
					_overlayCanvasCursor.anchoredPosition = __RandomMove(new Vector3(600, 300, 0), .5f);
					// カーソル3dの位置を2dと同期させる
					var sp = RectTransformUtility.WorldToScreenPoint(null, _overlayCanvasCursor.position);
					if (RectTransformUtility.RectangleContainsScreenPoint(_overlayCanvasTargetRect, sp, null))
					{
						if (RectTransformUtility.ScreenPointToWorldPointInRectangle(_overlayCanvasTargetRect, sp, Camera.main, out var point))
						{
							_cursor3d.position = point;
							_tmCoord.color = Color.red;
						}
					}

				}, isEnter => __OverlayCanvasEnable(isEnter)),
				("８．スクリーンスペースキャンバスからワールド位置へ変換", () => {
					// カーソル2dをランダムに動かす
					_screenSpaceCanvasCursor.anchoredPosition = __RandomMove(new Vector3(600, 350, 0), .5f);
					// カーソル3dの位置を2dと同期させる
					var sp = RectTransformUtility.WorldToScreenPoint(_screenSpaceCanvas.worldCamera, _screenSpaceCanvasCursor.position);
					if (RectTransformUtility.RectangleContainsScreenPoint(_screenSpaceCanvasTargetRect, sp, _screenSpaceCanvas.worldCamera))
					{
						if (RectTransformUtility.ScreenPointToWorldPointInRectangle(_screenSpaceCanvasTargetRect, sp, _screenSpaceCanvas.worldCamera, out var point))
						{
							_cursor3d.position = point;
							_tmCoord.color = Color.red;
						}
					}
					_screenSpaceCanvas.planeDistance = Mathf.Lerp(1, 10, Mathf.InverseLerp(-1, 1, Mathf.Sin(Time.time * 2)));
				}, isEnter => {
					__ScreenSpaceCanvasEnable(isEnter);
					_screenSpaceCanvas.planeDistance = 1;
				}),
			};
			void __OverlayCanvasEnable(bool enabled) => _overlayCanvasCursor.GetComponentInParent<Canvas>(true).gameObject.SetActive(enabled);
			void __ScreenSpaceCanvasEnable(bool enabled) => _screenSpaceCanvasCursor.GetComponentInParent<Canvas>(true).gameObject.SetActive(enabled);
			Vector3 __RandomMove(Vector3 power, float speed)
			{
				var time = Time.time * speed;
				return new Vector3(
					(Mathf.PerlinNoise(time, time + .1f) * 2 - 1f) * power.x,
					(Mathf.PerlinNoise(time + .2f, time + .3f) * 2 - 1f) * power.y / 2,
					(Mathf.PerlinNoise(time + .4f, time + .5f) * 2 - 1f) * power.z
				);
			}
		}

		void Update()
		{
			if (Input.GetMouseButtonDown(1))
			{
				_procs[_idx].enterExit?.Invoke(false);
				_idx = (_idx + 1) % _procs.Length;
				_procs[_idx].enterExit?.Invoke(true);
				_uiText.text = _procs[_idx].name;
			}
			_tmCoord.color = Color.white;
			_procs[_idx].update?.Invoke();
			// マウス追従テキスト
			_tmCoord.rectTransform.anchorMin = _tmCoord.rectTransform.anchorMax = new Vector2(Input.mousePosition.x / Screen.width, Input.mousePosition.y / Screen.height);
			_tmCoord.alignment = _tmCoord.rectTransform.anchorMin.x < 0.5f ? (TextAlignmentOptions)HorizontalAlignmentOptions.Right : (TextAlignmentOptions)HorizontalAlignmentOptions.Left;
			_tmCoord.alignment |= _tmCoord.rectTransform.anchorMin.y < 0.5f ? (TextAlignmentOptions)VerticalAlignmentOptions.Top : (TextAlignmentOptions)VerticalAlignmentOptions.Bottom;
			_tmCoord.SetText("{0}:{1}", (int)Input.mousePosition.x, (int)Input.mousePosition.y);
		}
	}
}
