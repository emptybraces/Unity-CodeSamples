using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using Cysharp.Threading.Tasks;
using System.IO;

namespace EmptyBraces.TextParser
{
	public static class Word
	{
		static string[] k_Split = new[] { "\t", " " };
		public static Dictionary<string, object> Data = new(256);
		static List<string> _tmpStringList = new(128);
		public static async UniTask<bool> LoadFromFile(string path, bool useSpan)
		{
			try
			{
				string text;
				using (var stream = new StreamReader(path, System.Text.Encoding.UTF8))
				{
					text = await stream.ReadToEndAsync();
				}
				if (string.IsNullOrEmpty(text))
				{
					Debug.LogError("Failed read." + path);
					return false;
				}
				if (useSpan)
					LoadFromTextWithSpan(text);
				else
					LoadFromText(text);
				cn.log("Complete read.");
			}
			catch (Exception e)
			{
				Debug.LogError("Exception: " + e.Message);
				return false;
			}
			return true;
		}

		public static void LoadFromText(string textData)
		{
			Data.Clear();
			_tmpStringList.Clear();
			int line_no = 0;
			string last_key = "";
			var lines = textData.Trim().Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
			foreach (var line in lines)
			{
				++line_no;
				if (line.StartsWith("/") || line.Trim() == "")
					continue;
				var splits = line.Split(k_Split, StringSplitOptions.RemoveEmptyEntries);
				if (0 == splits.Length)
				{
					cn.loge("found empty line.", line, line_no);
					continue;
				}
				// 配列用
				else if (1 == splits.Length)
				{
					_tmpStringList.Add(splits[0]);
					continue;
				}
				else if (2 < splits.Length)
				{
					cn.loge("ignore multiple context.", line, line_no);
				}
				// 直前が配列終わりだった場合、配列を格納する
				_AddWordArray(last_key, _tmpStringList);
				last_key = splits[0];
				Data.Add(splits[0], splits[1].Replace("\\n", "\n"));
				cn.log("add | ", splits[0], splits[1]);
			}
			// 直前が配列終わりだった場合、配列を格納する
			_AddWordArray(last_key, _tmpStringList);
		}

		public static void LoadFromTextWithSpan(string textData)
		{
			Data.Clear();
			_tmpStringList.Clear();
			var textData_span = textData.AsSpan();
			var s_idx = 0;
			var line_no = 0;
			string last_key = "";
			while (s_idx < textData.Length)
			{
				// セパレータで区切られた１カラムを探す
				var e_idx = textData.IndexOf(Environment.NewLine, s_idx);
				// 最後のカラムを処理するためにendIndexの操作
				if (e_idx == -1)
					e_idx = textData.Length;
				// 部分文字列をReadOnlySpan<char>で受け取る
				var line = textData_span[s_idx..e_idx];
				// 次のカラムを探すためs_idx更新
				s_idx = e_idx + 2; /*改行コードは2charある様子*/

				++line_no;
				if (line.StartsWith("//") || line.IsEmpty)
					continue;
				// keyの検出
				cn.log(line_no);
				var key_idx = line.IndexOfAny('\t', ' ');
				Assert.IsFalse(key_idx == -1);
				// 先頭がタブまたは空白の場合は配列型
				if (key_idx == 0)
				{
					// keyは直前のものを使用し、valueを検出する
					_tmpStringList.Add(line.Trim().ToString());
				}
				// keyとvalueが一対一のもの
				else
				{
					// 直前が配列終わりだった場合、配列を格納する
					_AddWordArray(last_key, _tmpStringList);

					var key = line[..key_idx];
					last_key = key.ToString();
					cn.logBlue(key_idx, last_key);

					// valueの検出
					var value_span = line[(key_idx + 1)..].Trim();
					var value = value_span.ToString();
					// 格納
					Data.Add(last_key, value);
					cn.log("add | ", last_key, value);
				}
			}
			// 直前が配列終わりだった場合、配列を格納する
			_AddWordArray(last_key, _tmpStringList);
		}
		static void _AddWordArray(string lastKey, List<string> values)
		{
			if (0 < values.Count)
			{
				values.Insert(0, (string)Data[lastKey]);
				Data[lastKey] = values.ToArray();
				values.Clear();
			}
		}

		static string[] _error;
		static string[] Error => _error ??= new string[2] { "unknown id", "unknown id" };
		public static string Get(string id) => Data.TryGetValue(id, out var s) ? (string)s : $"unknown id:{id}";
		public static string Get<T>(string id, T t1) => Data.TryGetValue(id, out var s) ? string.Format((string)s, t1) : $"unknown id:{id}";
		public static string Get<T1, T2>(string id, T1 t1, T2 t2) => Data.TryGetValue(id, out var s) ? string.Format((string)s, t1, t2) : $"unknown id:{id}";
		public static string Get<T1, T2, T3>(string id, T1 t1, T2 t2, T3 t3) => Data.TryGetValue(id, out var s) ? string.Format((string)s, t1, t2, t3) : $"unknown id:{id}";
		public static string Get(string id, params object[] args) => Data.TryGetValue(id, out var s) ? string.Format((string)s, args) : $"unknown id:{id}";
		public static string[] GetArray(string id) => Data.TryGetValue(id, out var s) ? (string[])s : Error;
		// -------------
		static object[] _a = new object[9];
		public static string Get<T1, T2, T3, T4>(string id, T1 t1, T2 t2, T3 t3, T4 t4) => Data.TryGetValue(id, out var s) ? string.Format((string)s, _Format(t1, t2, t3, t4)) : $"unknown id:{id}";
		public static string Get<T1, T2, T3, T4, T5>(string id, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5) => Data.TryGetValue(id, out var s) ? string.Format((string)s, _Format(t1, t2, t3, t4, t5)) : $"unknown id:{id}";
		public static string Get<T1, T2, T3, T4, T5, T6>(string id, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6) => Data.TryGetValue(id, out var s) ? string.Format((string)s, _Format(t1, t2, t3, t4, t5, t6)) : $"unknown id:{id}";
		public static string Get<T1, T2, T3, T4, T5, T6, T7>(string id, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7) => Data.TryGetValue(id, out var s) ? string.Format((string)s, _Format(t1, t2, t3, t4, t5, t6, t7)) : $"unknown id:{id}";
		public static string Get<T1, T2, T3, T4, T5, T6, T7, T8>(string id, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8) => Data.TryGetValue(id, out var s) ? string.Format((string)s, _Format(t1, t2, t3, t4, t5, t6, t7, t8)) : $"unknown id:{id}";
		public static string Get<T1, T2, T3, T4, T5, T6, T7, T8, T9>(string id, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9) => Data.TryGetValue(id, out var s) ? string.Format((string)s, _Format(t1, t2, t3, t4, t5, t6, t7, t8, t9)) : $"unknown id:{id}";
		static object[] _Format<T0, T1, T2, T3>(T0 t0, T1 t1, T2 t2, T3 t3) { _a[0] = t0; _a[1] = t1; _a[2] = t2; _a[3] = t3; return _a; }
		static object[] _Format<T0, T1, T2, T3, T4>(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4) { _a[0] = t0; _a[1] = t1; _a[2] = t2; _a[3] = t3; _a[4] = t4; return _a; }
		static object[] _Format<T0, T1, T2, T3, T4, T5>(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5) { _a[0] = t0; _a[1] = t1; _a[2] = t2; _a[3] = t3; _a[4] = t4; _a[5] = t5; return _a; }
		static object[] _Format<T0, T1, T2, T3, T4, T5, T6>(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6) { _a[0] = t0; _a[1] = t1; _a[2] = t2; _a[3] = t3; _a[4] = t4; _a[5] = t5; _a[6] = t6; return _a; }
		static object[] _Format<T0, T1, T2, T3, T4, T5, T6, T7>(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7) { _a[0] = t0; _a[1] = t1; _a[2] = t2; _a[3] = t3; _a[4] = t4; _a[5] = t5; _a[6] = t6; _a[7] = t7; return _a; }
		static object[] _Format<T0, T1, T2, T3, T4, T5, T6, T7, T8>(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8) { _a[0] = t0; _a[1] = t1; _a[2] = t2; _a[3] = t3; _a[4] = t4; _a[5] = t5; _a[6] = t6; _a[7] = t7; _a[8] = t8; return _a; }

#if UNITY_EDITOR
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		static void _DomainReset()
		{
			Data.Clear();
		}
#endif
	}
}