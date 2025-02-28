#if DEBUG
#define MSG_LOG_ENABLE
#endif
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Assertions;
namespace Emptybraces
{
	public enum MsgId
	{
		OnInitedLongTime,
		OnCreated,
		OnRemoved,
	}

	public static class Msg
	{
#if MSG_LOG_ENABLE
		static readonly MsgId[] _ignoreLogMsgId = new MsgId[]
		{
			MsgId.OnCreated,
			MsgId.OnRemoved,
		};
		public static bool IsOutputError = true;
		static string _color = COLOR_NORMAL;
		const string COLOR_NORMAL = "lightblue";
		const string COLOR_RESERVE = "orange";
#endif
#if DEBUG
		static Dictionary<(int idx, int instanceId), int> _registeredTime = new Dictionary<(int, int), int>(32);
#endif
		static Dictionary<(int idx, int instanceId), HashSet<Delegate>> _items = new Dictionary<(int, int), HashSet<Delegate>>(32);
		static int _cntInvoking;
		static List<(GameObject, MsgId, Delegate)> _modifiesItemInInvokeSet = new List<(GameObject, MsgId, Delegate)>(8);
		static List<(GameObject, MsgId, Delegate)> _modifiesItemInInvokeUnset = new List<(GameObject, MsgId, Delegate)>(8);
		static Msg()
		{
		}
		public static void Invoke(MsgId id)
		{
			if (_items.TryGetValue(((int)id, 0), out var a))
			{
				__LogInvoke(id);
				++_cntInvoking;
				foreach (var i in a)
				{
					if (i is Action ev)
						ev();
#if UNITY_EDITOR
					else
					{
						cn.loge("Invoke Failed Error: Not specified arguments.");
						cn.loge("but delegate sigunature is: " + i.Method);
					}
#endif
				}
				--_cntInvoking;
			}
			else
				__LogInvokeFailed(id);
			InvokeFinalize();

		}
		public static void Invoke<T>(MsgId id, T t)
		{
			if (_items.TryGetValue(((int)id, 0), out var a))
			{
				__LogInvoke(id);
				++_cntInvoking;
				foreach (var i in a)
				{
					if (i is Action<T> ev)
						ev(t);
#if UNITY_EDITOR
					else
					{
						cn.loge("Invoke Failed Error: Invoke arguments are ", typeof(T), "but registered sigunature is " + i.Method);
					}
#endif
				}
				--_cntInvoking;
			}
			else
				__LogInvokeFailed(id);
			InvokeFinalize();
		}
		public static void Invoke<T0, T1>(MsgId id, T0 t0, T1 t1)
		{
			if (_items.TryGetValue(((int)id, 0), out var a))
			{
				__LogInvoke(id);
				++_cntInvoking;
				foreach (var i in a)
				{
					if (i is Action<T0, T1> ev)
						ev(t0, t1);
#if UNITY_EDITOR
					else
					{
						cn.loge("Invoke Failed Error: Specified arguments are ", typeof(T0), typeof(T1));
						cn.loge("but delegate sigunature is: " + i.Method);
					}
#endif
				}
				--_cntInvoking;
			}
			else
				__LogInvokeFailed(id);
			InvokeFinalize();
		}
		public static void Invoke<T0, T1, T2>(MsgId id, T0 t0, T1 t1, T2 t2)
		{
			if (_items.TryGetValue(((int)id, 0), out var a))
			{
				__LogInvoke(id);
				++_cntInvoking;
				foreach (var i in a)
				{
					if (i is Action<T0, T1, T2> ev)
						ev(t0, t1, t2);
#if UNITY_EDITOR
					else
					{
						cn.loge("Invoke Failed Error: Specified arguments are ", typeof(T0), typeof(T1), typeof(T2));
						cn.loge("but delegate sigunature is: " + i.Method);
					}
#endif
				}
				--_cntInvoking;
			}
			else
				__LogInvokeFailed(id);
			InvokeFinalize();
		}
		public static void Invoke<T0, T1, T2, T3>(MsgId id, T0 t0, T1 t1, T2 t2, T3 t3)
		{
			++_cntInvoking;
			if (_items.TryGetValue(((int)id, 0), out var a))
			{
				__LogInvoke(id);
				foreach (var i in a)
				{
					if (i is Action<T0, T1, T2, T3> ev)
						ev(t0, t1, t2, t3);
#if UNITY_EDITOR
					else
					{
						cn.loge("Invoke Failed Error: Specified arguments are ", typeof(T0), typeof(T1), typeof(T2), typeof(T3));
						cn.loge("but delegate sigunature is: " + i.Method);
					}
#endif
				}
			}
			else
				__LogInvokeFailed(id);
			InvokeFinalize();
		}
		public static void Invoke<T0, T1, T2, T3, T4>(MsgId id, T0 t0, T1 t1, T2 t2, T3 t3, T4 t4)
		{
			if (_items.TryGetValue(((int)id, 0), out var a))
			{
				__LogInvoke(id);
				++_cntInvoking;
				foreach (var i in a)
				{
					if (i is Action<T0, T1, T2, T3, T4> ev)
						ev(t0, t1, t2, t3, t4);
#if UNITY_EDITOR
					else
					{
						cn.loge("Invoke Failed Error: Specified arguments are ", typeof(T0), typeof(T1), typeof(T2), typeof(T3), typeof(T4));
						cn.loge("but delegate sigunature is: " + i.Method);
					}
#endif
				}
				--_cntInvoking;
			}
			else
				__LogInvokeFailed(id);
			InvokeFinalize();
		}
		public static void Invoke(GameObject g, MsgId id)
		{
			if (_items.TryGetValue(((int)id, g.GetInstanceID()), out var a))
			{
				__LogInvoke(id);
				++_cntInvoking;
				foreach (var i in a)
				{
					if (i is Action ev)
						ev();
#if UNITY_EDITOR
					else
					{
						cn.loge("Invoke Failed Error: Not specified arguments.");
						cn.loge("but delegate sigunature is: " + i.Method);
					}
#endif
				}
				--_cntInvoking;
			}
			else
				__LogInvokeFailed(id, g);
			InvokeFinalize();
		}
		public static void Invoke<T>(GameObject g, MsgId id, T t)
		{
			if (_items.TryGetValue(((int)id, g.GetInstanceID()), out var a))
			{
				__LogInvoke(id);
				++_cntInvoking;
				foreach (var i in a)
				{
					if (i is Action<T> ev)
						ev(t);
#if UNITY_EDITOR
					else
					{
						cn.loge("Invoke Failed Error: Specified arguments are ", typeof(T));
						cn.loge("but delegate sigunature is: " + i.Method);
					}
#endif
				}
				--_cntInvoking;
			}
			else
				__LogInvokeFailed(id, g);
			InvokeFinalize();
		}
		public static void Invoke<T0, T1>(GameObject g, MsgId id, T0 t0, T1 t1)
		{
			if (_items.TryGetValue(((int)id, g.GetInstanceID()), out var a))
			{
				__LogInvoke(id);
				++_cntInvoking;
				foreach (var i in a)
				{
					if (i is Action<T0, T1> ev)
						ev(t0, t1);
#if UNITY_EDITOR
					else
					{
						cn.loge("Invoke Failed Error: Specified arguments are ", typeof(T0), typeof(T1));
						cn.loge("but delegate sigunature is: " + i.Method);
					}
#endif
				}
				--_cntInvoking;
			}
			else
				__LogInvokeFailed(id, g);
			InvokeFinalize();
		}
		public static void Invoke<T0, T1, T2>(GameObject g, MsgId id, T0 t0, T1 t1, T2 t2)
		{
			if (_items.TryGetValue(((int)id, g.GetInstanceID()), out var a))
			{
				__LogInvoke(id);
				++_cntInvoking;
				foreach (var i in a)
				{
					if (i is Action<T0, T1, T2> ev)
						ev(t0, t1, t2);
#if UNITY_EDITOR
					else
					{
						cn.loge("Invoke Failed Error: Specified arguments are ", typeof(T0), typeof(T1), typeof(T2));
						cn.loge("but delegate sigunature is: " + i.Method);
					}
#endif
				}
				--_cntInvoking;
			}
			else
				__LogInvokeFailed(id, g);
			InvokeFinalize();
		}
		public static void Invoke<T0, T1, T2, T3>(GameObject g, MsgId id, T0 t0, T1 t1, T2 t2, T3 t3)
		{
			if (_items.TryGetValue(((int)id, g.GetInstanceID()), out var a))
			{
				__LogInvoke(id);
				++_cntInvoking;
				foreach (var i in a)
				{
					if (i is Action<T0, T1, T2, T3> ev)
						ev(t0, t1, t2, t3);
#if UNITY_EDITOR
					else
					{
						cn.loge("Invoke Failed Error: Specified arguments are ", typeof(T0), typeof(T1), typeof(T2), typeof(T3));
						cn.loge("but delegate sigunature is: " + i.Method);
					}
#endif
				}
				--_cntInvoking;
			}
			else
				__LogInvokeFailed(id, g);
			InvokeFinalize();
		}
		public static void Invoke<T0, T1, T2, T3, T4>(GameObject g, MsgId id, T0 t0, T1 t1, T2 t2, T3 t3, T4 t4)
		{
			if (_items.TryGetValue(((int)id, g.GetInstanceID()), out var a))
			{
				__LogInvoke(id);
				++_cntInvoking;
				foreach (var i in a)
				{
					if (i is Action<T0, T1, T2, T3, T4> ev)
						ev(t0, t1, t2, t3, t4);
#if UNITY_EDITOR
					else
					{
						cn.loge("Invoke Failed Error: Specified arguments are ", typeof(T0), typeof(T1), typeof(T2), typeof(T3), typeof(T4));
						cn.loge("but delegate sigunature is: " + i.Method);
					}
#endif
				}
				--_cntInvoking;
			}
			else
				__LogInvokeFailed(id, g);
			InvokeFinalize();
		}
		public static void InvokeFinalize()
		{
			if (0 < _cntInvoking)
				return;
			__StartReservedSetUnset();
			foreach (var i in _modifiesItemInInvokeSet)
				__Set(i.Item1, i.Item2, i.Item3);
			foreach (var i in _modifiesItemInInvokeUnset)
				__Unset(i.Item1, i.Item2, i.Item3);
			_modifiesItemInInvokeSet.Clear();
			_modifiesItemInInvokeUnset.Clear();
			__EndReservedSetUnset();
		}

		public static void Set(MsgId id, Action action) => __Set(null, id, (Delegate)action);
		public static void Unset(MsgId id, Action action) => __Unset(null, id, (Delegate)action);
		public static void Set<T>(MsgId id, Action<T> action) => __Set(null, id, (Delegate)action);
		public static void Unset<T>(MsgId id, Action<T> action) => __Unset(null, id, (Delegate)action);
		public static void Set<T0, T1>(MsgId id, Action<T0, T1> action) => __Set(null, id, (Delegate)action);
		public static void Unset<T0, T1>(MsgId id, Action<T0, T1> action) => __Unset(null, id, (Delegate)action);
		public static void Set<T0, T1, T2>(MsgId id, Action<T0, T1, T2> action) => __Set(null, id, (Delegate)action);
		public static void Unset<T0, T1, T2>(MsgId id, Action<T0, T1, T2> action) => __Unset(null, id, (Delegate)action);
		public static void Set<T0, T1, T2, T3>(MsgId id, Action<T0, T1, T2, T3> action) => __Set(null, id, (Delegate)action);
		public static void Unset<T0, T1, T2, T3>(MsgId id, Action<T0, T1, T2, T3> action) => __Unset(null, id, (Delegate)action);
		public static void Set<T0, T1, T2, T3, T4>(MsgId id, Action<T0, T1, T2, T3, T4> action) => __Set(null, id, (Delegate)action);
		public static void Unset<T0, T1, T2, T3, T4>(MsgId id, Action<T0, T1, T2, T3, T4> action) => __Unset(null, id, (Delegate)action);
		public static void Set(GameObject g, MsgId id, Action action) { Assert.IsNotNull(g); __Set(g, id, (Delegate)action); }
		public static void Unset(GameObject g, MsgId id, Action action) { Assert.IsNotNull(g); __Unset(g, id, (Delegate)action); }
		public static void Set<T>(GameObject g, MsgId id, Action<T> action) { Assert.IsNotNull(g); __Set(g, id, (Delegate)action); }
		public static void Unset<T>(GameObject g, MsgId id, Action<T> action) { Assert.IsNotNull(g); __Unset(g, id, (Delegate)action); }
		public static void Set<T0, T1>(GameObject g, MsgId id, Action<T0, T1> action) { Assert.IsNotNull(g); __Set(g, id, (Delegate)action); }
		public static void Unset<T0, T1>(GameObject g, MsgId id, Action<T0, T1> action) { Assert.IsNotNull(g); __Unset(g, id, (Delegate)action); }
		public static void Set<T0, T1, T2>(GameObject g, MsgId id, Action<T0, T1, T2> action) { Assert.IsNotNull(g); __Set(g, id, (Delegate)action); }
		public static void Unset<T0, T1, T2>(GameObject g, MsgId id, Action<T0, T1, T2> action) { Assert.IsNotNull(g); __Unset(g, id, (Delegate)action); }
		public static void Set<T0, T1, T2, T3>(GameObject g, MsgId id, Action<T0, T1, T2, T3> action) { Assert.IsNotNull(g); __Set(g, id, (Delegate)action); }
		public static void Unset<T0, T1, T2, T3>(GameObject g, MsgId id, Action<T0, T1, T2, T3> action) { Assert.IsNotNull(g); __Unset(g, id, (Delegate)action); }
		public static void Set<T0, T1, T2, T3, T4>(GameObject g, MsgId id, Action<T0, T1, T2, T3, T4> action) { Assert.IsNotNull(g); __Set(g, id, (Delegate)action); }
		public static void Unset<T0, T1, T2, T3, T4>(GameObject g, MsgId id, Action<T0, T1, T2, T3, T4> action) { Assert.IsNotNull(g); __Unset(g, id, (Delegate)action); }
		static void __Set(GameObject g, MsgId id, Delegate action)
		{
			// foreach 中にコレクション操作されると例外発生するので、Invoke終わった後に実行する。
			if (0 < _cntInvoking)
			{
				_modifiesItemInInvokeSet.Add((g, id, action));
				__LogSetReserve(id);
				return;
			}
			var gid = g != null ? g.GetInstanceID() : 0;
			bool r = false;
			var key = ((int)id, gid);
			if (_items.TryGetValue(key, out var set))
			{
				r = set.Add(action);
			}
			else
			{
				set = new HashSet<Delegate>();
				r = set.Add(action);
				_items.Add(key, set);
			}
#if DEBUG
			_registeredTime[key] = Time.frameCount;
#endif
			__LogSet(r, id, action);
		}
		static void __Unset(GameObject g, MsgId id, Delegate action)
		{
			// foreach 中にコレクション操作されると例外発生するので、Invoke終わった後に実行する。
			// cn.logRed(g, id, _isInvoking);
			if (0 < _cntInvoking)
			{
				_modifiesItemInInvokeUnset.Add((g, id, action));
				__LogUnsetReserve(id);
				return;
			}
			var gid = g != null ? g.GetInstanceID() : 0;
			bool r = false;
			var key = ((int)id, gid);
			if (_items.TryGetValue(key, out var set))
			{
				r = set.Remove(action);
				// インスタンスIDの参照がいる場合で、アイテムがゼロになったら削除する
				if (gid != 0 && set.Count() == 0)
				{
					_items.Remove(key);
#if DEBUG
					_registeredTime.Remove(key);
#endif
				}
			}
			__LogUnset(r, id, action);
		}

		public static void Unset(GameObject g)
		{
			var gid = g.GetInstanceID();
			bool r = false;
			foreach (var key in _items.Keys.ToArray())
			{
				if (key.Item2 == gid)
				{
					r = true;
					_items.Remove(key);
				}
			}
			__LogUnset(r, g);
		}

		static StringBuilder _sb;
		public static string GetState()
		{
#if DEBUG
			_sb ??= new StringBuilder();
			_sb.Clear();
			_sb.Append("Key Total: ").AppendLine(_items.Count.ToString());
			_sb.Append("Value Total: ").AppendLine(_items.Sum(e => e.Value.Count()).ToString());
			foreach (var i in _items.OrderBy(e => e.Key.Item1))
			{
				_sb.AppendLine($"\tno=({(MsgId)i.Key.Item1}, objid={i.Key.Item2,8}), cnt={i.Value.Count,2}, reg={Time.frameCount - _registeredTime[i.Key],5}");
				// _sb.AppendLine($"\tno=({i.Key.Item1,3}, objid={i.Key.Item2,8}), cnt={i.Value.Count,2}, reg={Time.frameCount - _registeredTime[i.Key],5}");
			}
			return _sb.ToString();
#else
			return "";
#endif
		}

		[System.Diagnostics.Conditional("MSG_LOG_ENABLE")]
		static void __StartReservedSetUnset()
		{
			int total = _modifiesItemInInvokeSet.Count + _modifiesItemInInvokeUnset.Count;
			if (0 < total)
				_color = COLOR_RESERVE;
		}
		[System.Diagnostics.Conditional("MSG_LOG_ENABLE")]
		static void __EndReservedSetUnset()
		{
			_color = COLOR_NORMAL;
		}
		[System.Diagnostics.Conditional("MSG_LOG_ENABLE")]
		static void __LogSetReserve(MsgId id)
		{
			// cn.log($"<color=orange>Msg登録予約({(int)id}.{id})</color>");
		}
		[System.Diagnostics.Conditional("MSG_LOG_ENABLE")]
		static void __LogUnsetReserve(MsgId id)
		{
			// cn.log($"<color=orange>Msg登録解除予約({(int)id}.{id})</color>");
		}
		[System.Diagnostics.Conditional("MSG_LOG_ENABLE")]
		static void __LogSet(bool result, MsgId id, Delegate action)
		{
			if (result) cn.log($"<color={_color}>Msg登録({(int)id}.{id}) 成功</color>");
			else if (IsOutputError) cn.log($"<color=yellow>Msg登録({(int)id}.{id}) 失敗</color>");
		}
		[System.Diagnostics.Conditional("MSG_LOG_ENABLE")]
		static void __LogUnset(bool result, MsgId id, Delegate action)
		{
			if (result) cn.log($"<color={_color}>Msg登録解除({(int)id}.{id}) 成功</color>");
			else if (IsOutputError) cn.log($"<color=yellow>Msg登録解除({(int)id}.{id}) 失敗</color>");
		}
		[System.Diagnostics.Conditional("MSG_LOG_ENABLE")]
		static void __LogUnset(bool result, GameObject g)
		{
			if (result) cn.log($"<color={_color}>Msg一括登録解除({g.name}) 成功</color>");
			else if (IsOutputError) cn.log($"<color=yellow>Msg一括登録解除({g.name}) 失敗</color>");
		}

		[System.Diagnostics.Conditional("MSG_LOG_ENABLE")]
		static void __LogInvoke(MsgId id)
		{
			if (!_ignoreLogMsgId.Contains(id))
				cn.log($"<color={_color}>Msg実行({(int)id}.{id})</color>");
		}
		[System.Diagnostics.Conditional("MSG_LOG_ENABLE")]
		static void __LogInvokeFailed(MsgId id, GameObject g = null)
		{
			if (IsOutputError && !_ignoreLogMsgId.Contains(id))
				cn.log($"<color=yellow>Msg実行失敗({(int)id}.{id}, {g})</color>");
		}
#if !DEBUG
		[System.Diagnostics.Conditional("NEVER_CALLED")]
#endif
		public static void Dump()
		{
			var sb = new StringBuilder();
			foreach (var kvp in _items)
			{
				sb.Clear();
				sb.Append($"{(MsgId)kvp.Key.idx}, {kvp.Key.instanceId}, {kvp.Value.ElementAt(0)}");
				foreach (var d in kvp.Value)
				{
					sb.Append($"\n - {d.Method}/{d.Target}");
				}
				cn.log(sb.ToString());
			}
		}

#if UNITY_EDITOR
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		static void _DomainReset()
		{
			_registeredTime = new Dictionary<(int, int), int>(32);
			_items = new Dictionary<(int, int), HashSet<Delegate>>(32);
			_cntInvoking = 0;
			_color = COLOR_NORMAL;
		}
#endif

	}
}
