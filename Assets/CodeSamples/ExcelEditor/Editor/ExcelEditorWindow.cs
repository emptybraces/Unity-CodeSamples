using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using MiniExcelLibs;
using UnityEditor;
using UnityEngine;

namespace Emptybraces.Editor
{
	public class ExcelEditorWindow : EditorWindow
	{
		Vector2 _scroll;
		string _filepath;
		int _rowMax = 10, _colMax = 10;
		GUIContent _iconSearchFile;
		GUIStyle _guiStyleTableHeaderRow;
		GUIStyle _guiStyleTableHeaderColumn;
		GUIStyle _guiStyleSheet, _guiStyleSheetCurrent;
		(MessageType type, string msg) _helpBoxInfo;
		DataTable[] _tables;
		string[] _sheets;
		int _currentSheetIdx;
		[MenuItem("Window/ExcelEditor")]
		public static void Open()
		{
			var window = EditorWindow.GetWindow<ExcelEditorWindow>("Excel Editor");
			window.ShowPopup();
		}

		void OnEnable()
		{
			_ValidatePath(_filepath);
			_guiStyleTableHeaderRow = null;
			_guiStyleTableHeaderColumn = null;
			_guiStyleSheet = null;
			_guiStyleSheetCurrent = null;
		}

		void OnGUI()
		{
			_guiStyleTableHeaderRow ??= new GUIStyle(EditorStyles.label)
			{
				fontStyle = FontStyle.Bold,
				alignment = TextAnchor.MiddleRight,
				normal = { textColor = Color.grey }
			};
			_guiStyleTableHeaderColumn ??= new GUIStyle(EditorStyles.label)
			{
				fontStyle = FontStyle.Bold,
				alignment = TextAnchor.LowerCenter,
				normal = { textColor = Color.grey }
			};
			_guiStyleSheet ??= new GUIStyle(EditorStyles.miniButton)
			{
				normal = { textColor = Color.white }
			};
			_guiStyleSheetCurrent ??= new GUIStyle(EditorStyles.miniButton)
			{
				normal = { textColor = Color.green }
			};
			var sx = 10;
			var w = position.width - 20;
			var r = new Rect(sx, 10, w, EditorGUIUtility.singleLineHeight * 1.5f);
			GUI.Box(r, "File");
			// ファイルパス
			r.y += r.height;
			r.width = w * 0.7f;
			r.height = EditorGUIUtility.singleLineHeight;
			using (var scope = new EditorGUI.ChangeCheckScope())
			{
				_filepath = GUI.TextField(r, _filepath);
				if (scope.changed)
				{
					_ValidatePath(_filepath);
				}
			}
			r.x += r.width;
			r.width = 50;
			// ファイル検索ダイアログボタン
			if (GUI.Button(r, _iconSearchFile ??= EditorGUIUtility.IconContent("d_Folder Icon")))
			{
				var s = EditorUtility.OpenFilePanel("Search .xlsx or .csv", Application.dataPath, "xlsx,csv");
				if (s.Length != 0)
					_ValidatePath(_filepath = s);
			}
			// ロードボタン
			r.x += r.width;
			r.width = w * .1f;
			GUI.enabled = _helpBoxInfo.type == MessageType.Info;
			if (GUI.Button(r, "Load"))
			{
				_currentSheetIdx = 0;
				if (!_filepath.EndsWith(".csv"))
					_sheets = MiniExcel.GetSheetNames(_filepath).ToArray();
				else
					_sheets = new string[1] { "csv" };
				_tables = new DataTable[_sheets.Length];
				for (int i = 0; i < _tables.Length; ++i)
					_tables[i] = MiniExcel.QueryAsDataTable(_filepath, false, !_filepath.EndsWith(".csv") ? _sheets[i] : null);
			}
			// セーブボタン
			r.x += r.width;
			GUI.enabled = _tables != null;
			if (GUI.Button(r, "Save"))
			{
				if (_filepath.EndsWith(".csv"))
				{
					MiniExcel.SaveAs(_filepath, _tables[0], false, overwriteFile: true);
				}
				else if (_filepath.EndsWith(".xlsx"))
				{
					var sheets = new Dictionary<string, object>();
					for (int i = 0; i < _tables.Length; ++i)
						sheets.Add(_sheets[i], _tables[i]);
					MiniExcel.SaveAs(_filepath, sheets, false, overwriteFile: true);
					AssetDatabase.Refresh();
				}
			}

			// ヘルプボックス
			GUI.enabled = true;
			if (_helpBoxInfo.type != MessageType.None)
			{
				r.x = sx;
				r.y += r.height;
				r.width = w;
				r.height = EditorGUIUtility.singleLineHeight * 2;
				EditorGUI.HelpBox(r, _helpBoxInfo.msg, _helpBoxInfo.type);
			}

			// マトリクスデータの表示
			r.y += r.height + 20;
			r.width = w;
			r.height = EditorGUIUtility.singleLineHeight * 1.5f;
			GUI.Box(r, "Data");
			if (_tables == null)
				return;
			r.y += r.height;
			// // セルの内容
			// using (var scope = new EditorGUI.ChangeCheckScope())
			// {
			// 	r.height = EditorGUIUtility.singleLineHeight * 2;
			// 	GUI.TextArea(r, "");
			// 	r.y += r.height;
			// }
			var text_size = new Vector2(100, EditorGUIUtility.singleLineHeight);
			var table = _tables[_currentSheetIdx];
			// ウィンドウ高さの下限か、データ行数分か
			var row_header_width = 30;
			var column_header_height = text_size.y;
			var data_width = text_size.x * table.Columns.Count + row_header_width;
			var data_height = text_size.y * table.Rows.Count + column_header_height;
			var sheet_button_height = 30;
			var scrollbar_h_offset = 20;
			var sy = r.y;
			r.height = Mathf.Min(position.height - r.y - sheet_button_height - scrollbar_h_offset, data_height + 20/*スクロールバーの分？*/);
			using (var scrollScope = new GUI.ScrollViewScope(r, _scroll, new Rect(r.x, r.y, data_width, data_height)))
			{
				_scroll = scrollScope.scrollPosition;
				// カラムヘッダ
				for (int ic = 0; ic < table.Columns.Count; ++ic)
				{
					// AA, AB...の様にはならない。
					var d = ic % 26;
					var char_ = (char)('A' + d);
					var s = (ic / 26).ToString() + char_;
					GUI.Label(new Rect(r.x + row_header_width + ic * text_size.x, r.y, text_size.x, column_header_height), s, _guiStyleTableHeaderColumn);
				}
				r.y += text_size.y;
				for (int ir = 0; ir < table.Rows.Count; ++ir)
				{
					// 行ヘッダ
					GUI.Label(new Rect(r.x, r.y, row_header_width, text_size.y), (ir + 1).ToString(), _guiStyleTableHeaderRow);
					// 行データ
					for (int ic = 0; ic < table.Columns.Count; ++ic)
					{
						table.Rows[ir][ic] = GUI.TextField(new Rect(r.x + 30 + ic * text_size.x, r.y, text_size.x, text_size.y), table.Rows[ir][ic].ToString());
					}
					r.y += text_size.y;
				}
			}

			// シート切替ボタン
			r.x = sx;
			r.y = sy + r.height + scrollbar_h_offset;
			r.width = 100;
			r.height = sheet_button_height;
			for (int i = 0; i < _sheets.Length; i++)
			{
				r.x = sx + i * r.width;
				GUI.enabled = _currentSheetIdx != i;
				if (GUI.Button(r, _sheets[i], _currentSheetIdx == i ? _guiStyleSheetCurrent : _guiStyleSheet))
				{
					_currentSheetIdx = i;
				}
			}
		}

		void _ValidatePath(string path)
		{
			if (!_filepath.EndsWith(".xlsx") && !_filepath.EndsWith(".csv"))
				_helpBoxInfo = (MessageType.Error, "Only supported file is .xlsx or .csv.");
			else if (!File.Exists(_filepath))
				_helpBoxInfo = (MessageType.Warning, "Specified file path is not exists.");
			else
				_helpBoxInfo = (MessageType.Info, "File is correct. Press Load button.");
		}


		IEnumerable<Dictionary<int, object>> ConvertToIntIndexRows(IEnumerable<object> rows)
		{
			ICollection<string> keys = null;
			var isFirst = true;
			foreach (IDictionary<string, object> r in rows)
			{
				if (isFirst)
				{
					keys = r.Keys;
					isFirst = false;
				}

				var dic = new Dictionary<int, object>();
				var index = 0;
				foreach (var key in keys)
					dic[index++] = r[key];
				yield return dic;
			}
		}
	}
}