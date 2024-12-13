
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UniVRM10;

namespace Emptybraces
{
	[RequireComponent(typeof(Vrm10Instance))]
	public class VRM10ExpressionController : MonoBehaviour
	{
		Vrm10Instance _vrm;

		[SerializeField, Range(0, 1)] float _happy;
		[SerializeField, Range(0, 1)] float _angry;
		[SerializeField, Range(0, 1)] float _sad;
		[SerializeField, Range(0, 1)] float _relaxed;
		[SerializeField, Range(0, 1)] float _surprised;
		List<ExpressionKey> _keys;
		List<float> _values;
		float _scrollBarPos;
		void Start()
		{
			_vrm = GetComponent<Vrm10Instance>();
			var weights = _vrm.Runtime.Expression.GetWeights();
			_keys = weights.Select(e => e.Key).ToList();
			_values = weights.Select(e => e.Value).ToList();
		}

		void OnGUI()
		{
			if (!Application.isPlaying || _values == null)
				return;
			// 開始位置
			const float k_sx = 10;
			const float k_sy = 10;
			const float k_label_width = 60;
			const float k_slider_width = 70;
			// 項目サイズ
			const float k_single_height = 20;
			// ビューポートサイズ
			var vp_height = Screen.height - k_sy;
			// コンテンツサイズ
			var content_height = _values.Count * k_single_height;
			// スクロールバーサイズ(0-1)
			var bar_size = Mathf.Min(1, vp_height / content_height);
			// スクロールバーの稼働
			var scrollable_range = content_height - vp_height;
			var scroll_bar_pos_n = bar_size < 1 ? (_scrollBarPos / (1 - bar_size)) : 0;
			// スクロールバーオフセット量
			var scroll_y = -scrollable_range * scroll_bar_pos_n;
			for (int i = 0; i < _values.Count; i++)
			{
				var rect = new Rect(k_sx, k_sy + k_single_height * i + scroll_y, k_label_width, k_single_height);
				GUI.Label(rect, _keys[i].Name);
				rect.x += k_label_width;
				rect.y += 5; // 微調整
				UnityEditor.EditorGUI.BeginChangeCheck();
				_values[i] = GUI.HorizontalSlider(rect, _values[i], 0, 1);
				if (UnityEditor.EditorGUI.EndChangeCheck())
				{
					_vrm.Runtime.Expression.SetWeight(_keys[i], _values[i]);
				}
			}
			_scrollBarPos = GUI.VerticalScrollbar(new Rect(k_label_width + k_slider_width + 10, k_sy, 5, vp_height), _scrollBarPos, bar_size, 0, 1);
		}
	}
}
