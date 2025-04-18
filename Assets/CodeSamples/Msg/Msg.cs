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
#endif
		public static Dictionary<(int id, int instanceId), int> RegisteredTime = new Dictionary<(int, int), int>(32);
		static Dictionary<(int id, int instanceId), HashSet<Delegate>> _idToDeligates = new Dictionary<(int, int), HashSet<Delegate>>(32);
		static Dictionary<MsgId, List<Delegate>> _idToInvokingDelicates = new();
		static Stack<List<Delegate>> _deligateListCaches = new();
		static Msg()
		{
			for (int i = 0; i < 2; ++i)
				_deligateListCaches.Push(new List<Delegate>());
		}
		public static void Invoke(MsgId id)
		{
			// invoke中に同じIDでInvokeしたら無限ループになるので阻止
			if (_CheckInfiniteLoop(id))
				return;
			if (_InvokePreprocess(id, 0, out var deligate_list))
			{
				for (int i = 0; i < deligate_list.Count; ++i)
				{
					var d = deligate_list[i];
					if (d is Action a)
						a.Invoke();
					else
						_LogE($"Invoke Failed Error: Not specified arguments. but delegate sigunature is: {d.Method}");
				}
				_InvokePostprocess(id, deligate_list);
			}
			else
				_LogInvokeFailed(id);
		}
		public static void Invoke<T>(MsgId id, T t)
		{
			if (_CheckInfiniteLoop(id))
				return;
			if (_InvokePreprocess(id, 0, out var deligate_list))
			{
				for (int i = 0; i < deligate_list.Count; ++i)
				{
					var d = deligate_list[i];
					if (d is Action<T> a)
						a.Invoke(t);
					else
						_LogE($"Invoke Failed Error: Specified arguments are {typeof(T)} but registered sigunature is {d.Method}");

				}
				_InvokePostprocess(id, deligate_list);
			}
			else
				_LogInvokeFailed(id);
		}
		public static void Invoke<T0, T1>(MsgId id, T0 t0, T1 t1)
		{
			if (_CheckInfiniteLoop(id))
				return;
			if (_InvokePreprocess(id, 0, out var deligate_list))
			{
				for (int i = 0; i < deligate_list.Count; ++i)
				{
					var d = deligate_list[i];
					if (d is Action<T0, T1> a)
						a.Invoke(t0, t1);
					else
						_LogE($"Invoke Failed Error: Specified arguments are {typeof(T0)}, {typeof(T1)} but registered sigunature is {d.Method}");

				}
				_InvokePostprocess(id, deligate_list);
			}
			else
				_LogInvokeFailed(id);
		}
		public static void Invoke<T0, T1, T2>(MsgId id, T0 t0, T1 t1, T2 t2)
		{
			if (_CheckInfiniteLoop(id))
				return;
			if (_InvokePreprocess(id, 0, out var deligate_list))
			{
				for (int i = 0; i < deligate_list.Count; ++i)
				{
					var d = deligate_list[i];
					if (d is Action<T0, T1, T2> a)
						a.Invoke(t0, t1, t2);
					else
						_LogE($"Invoke Failed Error: Specified arguments are {typeof(T0)}, {typeof(T1)}, {typeof(T2)} but registered sigunature is {d.Method}");

				}
				_InvokePostprocess(id, deligate_list);
			}
			else
				_LogInvokeFailed(id);
		}
		public static void Invoke<T0, T1, T2, T3>(MsgId id, T0 t0, T1 t1, T2 t2, T3 t3)
		{
			if (_CheckInfiniteLoop(id))
				return;
			if (_InvokePreprocess(id, 0, out var deligate_list))
			{
				for (int i = 0; i < deligate_list.Count; ++i)
				{
					var d = deligate_list[i];
					if (d is Action<T0, T1, T2, T3> a)
						a.Invoke(t0, t1, t2, t3);
					else
						_LogE($"Invoke Failed Error: Specified arguments are {typeof(T0)}, {typeof(T1)}, {typeof(T2)}, {typeof(T3)} but registered sigunature is {d.Method}");

				}
				_InvokePostprocess(id, deligate_list);
			}
			else
				_LogInvokeFailed(id);
		}
		public static void Invoke<T0, T1, T2, T3, T4>(MsgId id, T0 t0, T1 t1, T2 t2, T3 t3, T4 t4)
		{
			if (_CheckInfiniteLoop(id))
				return;
			if (_InvokePreprocess(id, 0, out var deligate_list))
			{
				for (int i = 0; i < deligate_list.Count; ++i)
				{
					var d = deligate_list[i];
					if (d is Action<T0, T1, T2, T3, T4> a)
						a.Invoke(t0, t1, t2, t3, t4);
					else
						_LogE($"Invoke Failed Error: Specified arguments are {typeof(T0)}, {typeof(T1)}, {typeof(T2)}, {typeof(T3)}, {typeof(T4)} but registered sigunature is {d.Method}");

				}
				_InvokePostprocess(id, deligate_list);
			}
			else
				_LogInvokeFailed(id);
		}
		public static void Invoke(GameObject g, MsgId id)
		{
			if (_CheckInfiniteLoop(id))
				return;
			if (_InvokePreprocess(id, g.GetInstanceID(), out var deligate_list))
			{
				for (int i = 0; i < deligate_list.Count; ++i)
				{
					var d = deligate_list[i];
					if (d is Action a)
						a.Invoke();
					else
						_LogE($"Invoke Failed Error: Not specified arguments. but registered sigunature is {d.Method}");

				}
				_InvokePostprocess(id, deligate_list);
			}
			else
				_LogInvokeFailed(id);
		}
		public static void Invoke<T>(GameObject g, MsgId id, T t)
		{
			if (_CheckInfiniteLoop(id))
				return;
			if (_InvokePreprocess(id, g.GetInstanceID(), out var deligate_list))
			{
				for (int i = 0; i < deligate_list.Count; ++i)
				{
					var d = deligate_list[i];
					if (d is Action<T> a)
						a.Invoke(t);
					else
						_LogE($"Invoke Failed Error: Specified arguments are {typeof(T)}, but registered sigunature is {d.Method}");

				}
				_InvokePostprocess(id, deligate_list);
			}
			else
				_LogInvokeFailed(id);
		}
		public static void Invoke<T0, T1>(GameObject g, MsgId id, T0 t0, T1 t1)
		{
			if (_CheckInfiniteLoop(id))
				return;
			if (_InvokePreprocess(id, g.GetInstanceID(), out var deligate_list))
			{
				for (int i = 0; i < deligate_list.Count; ++i)
				{
					var d = deligate_list[i];
					if (d is Action<T0, T1> a)
						a.Invoke(t0, t1);
					else
						_LogE($"Invoke Failed Error: Specified arguments are {typeof(T0)}, {typeof(T1)}, but registered sigunature is {d.Method}");

				}
				_InvokePostprocess(id, deligate_list);
			}
			else
				_LogInvokeFailed(id);
		}
		public static void Invoke<T0, T1, T2>(GameObject g, MsgId id, T0 t0, T1 t1, T2 t2)
		{
			if (_CheckInfiniteLoop(id))
				return;
			if (_InvokePreprocess(id, g.GetInstanceID(), out var deligate_list))
			{
				for (int i = 0; i < deligate_list.Count; ++i)
				{
					var d = deligate_list[i];
					if (d is Action<T0, T1, T2> a)
						a.Invoke(t0, t1, t2);
					else
						_LogE($"Invoke Failed Error: Specified arguments are {typeof(T0)}, {typeof(T1)}, {typeof(T2)}, but registered sigunature is {d.Method}");

				}
				_InvokePostprocess(id, deligate_list);
			}
			else
				_LogInvokeFailed(id);
		}
		public static void Invoke<T0, T1, T2, T3>(GameObject g, MsgId id, T0 t0, T1 t1, T2 t2, T3 t3)
		{
			if (_CheckInfiniteLoop(id))
				return;
			if (_InvokePreprocess(id, g.GetInstanceID(), out var deligate_list))
			{
				for (int i = 0; i < deligate_list.Count; ++i)
				{
					var d = deligate_list[i];
					if (d is Action<T0, T1, T2, T3> a)
						a.Invoke(t0, t1, t2, t3);
					else
						_LogE($"Invoke Failed Error: Specified arguments are {typeof(T0)}, {typeof(T1)}, {typeof(T2)}, {typeof(T3)}, but registered sigunature is {d.Method}");

				}
				_InvokePostprocess(id, deligate_list);
			}
			else
				_LogInvokeFailed(id);
		}
		public static void Invoke<T0, T1, T2, T3, T4>(GameObject g, MsgId id, T0 t0, T1 t1, T2 t2, T3 t3, T4 t4)
		{
			if (_CheckInfiniteLoop(id))
				return;
			if (_InvokePreprocess(id, g.GetInstanceID(), out var deligate_list))
			{
				for (int i = 0; i < deligate_list.Count; ++i)
				{
					var d = deligate_list[i];
					if (d is Action<T0, T1, T2, T3, T4> a)
						a.Invoke(t0, t1, t2, t3, t4);
					else
						_LogE($"Invoke Failed Error: Specified arguments are {typeof(T0)}, {typeof(T1)}, {typeof(T2)}, {typeof(T3)}, {typeof(T4)}, but registered sigunature is {d.Method}");

				}
				_InvokePostprocess(id, deligate_list);
			}
			else
				_LogInvokeFailed(id);
		}

		static bool _CheckInfiniteLoop(MsgId id)
		{
			if (_idToInvokingDelicates.ContainsKey(id))
			{
				_LogE($"Invoke Failed Error: An Invoke with the same ID({id}) was called during an ongoing Invoke. Infinite loop prevented.");
				return true;
			}
			return false;
		}
		static bool _InvokePreprocess(MsgId id, int instanceId, out List<Delegate> delegateList)
		{
			if (_idToDeligates.TryGetValue(((int)id, instanceId), out var deligates))
			{
				_LogInvoke(id);
				// リストからバッファから取得する。Invokeする１フレームだけ必要なdelegateリストは使いまわすため、バッファを用意。
				if (!_deligateListCaches.TryPop(out delegateList))
					delegateList = new();
				// 覚えておく
				_idToInvokingDelicates[id] = delegateList;
				delegateList.AddRange(deligates);
				return true;
			}
			delegateList = null;
			return false;
		}
		static void _InvokePostprocess(MsgId id, List<Delegate> delegateList)
		{
			delegateList.Clear();
			_deligateListCaches.Push(delegateList);
			_idToInvokingDelicates.Remove(id);
		}


		public static void Set(MsgId id, Action action) => _Set(null, id, (Delegate)action);
		public static void Unset(MsgId id, Action action) => _Unset(null, id, (Delegate)action);
		public static void Set<T>(MsgId id, Action<T> action) => _Set(null, id, (Delegate)action);
		public static void Unset<T>(MsgId id, Action<T> action) => _Unset(null, id, (Delegate)action);
		public static void Set<T0, T1>(MsgId id, Action<T0, T1> action) => _Set(null, id, (Delegate)action);
		public static void Unset<T0, T1>(MsgId id, Action<T0, T1> action) => _Unset(null, id, (Delegate)action);
		public static void Set<T0, T1, T2>(MsgId id, Action<T0, T1, T2> action) => _Set(null, id, (Delegate)action);
		public static void Unset<T0, T1, T2>(MsgId id, Action<T0, T1, T2> action) => _Unset(null, id, (Delegate)action);
		public static void Set<T0, T1, T2, T3>(MsgId id, Action<T0, T1, T2, T3> action) => _Set(null, id, (Delegate)action);
		public static void Unset<T0, T1, T2, T3>(MsgId id, Action<T0, T1, T2, T3> action) => _Unset(null, id, (Delegate)action);
		public static void Set<T0, T1, T2, T3, T4>(MsgId id, Action<T0, T1, T2, T3, T4> action) => _Set(null, id, (Delegate)action);
		public static void Unset<T0, T1, T2, T3, T4>(MsgId id, Action<T0, T1, T2, T3, T4> action) => _Unset(null, id, (Delegate)action);
		public static void Set(GameObject g, MsgId id, Action action) { Assert.IsNotNull(g); _Set(g, id, (Delegate)action); }
		public static void Unset(GameObject g, MsgId id, Action action) { Assert.IsNotNull(g); _Unset(g, id, (Delegate)action); }
		public static void Set<T>(GameObject g, MsgId id, Action<T> action) { Assert.IsNotNull(g); _Set(g, id, (Delegate)action); }
		public static void Unset<T>(GameObject g, MsgId id, Action<T> action) { Assert.IsNotNull(g); _Unset(g, id, (Delegate)action); }
		public static void Set<T0, T1>(GameObject g, MsgId id, Action<T0, T1> action) { Assert.IsNotNull(g); _Set(g, id, (Delegate)action); }
		public static void Unset<T0, T1>(GameObject g, MsgId id, Action<T0, T1> action) { Assert.IsNotNull(g); _Unset(g, id, (Delegate)action); }
		public static void Set<T0, T1, T2>(GameObject g, MsgId id, Action<T0, T1, T2> action) { Assert.IsNotNull(g); _Set(g, id, (Delegate)action); }
		public static void Unset<T0, T1, T2>(GameObject g, MsgId id, Action<T0, T1, T2> action) { Assert.IsNotNull(g); _Unset(g, id, (Delegate)action); }
		public static void Set<T0, T1, T2, T3>(GameObject g, MsgId id, Action<T0, T1, T2, T3> action) { Assert.IsNotNull(g); _Set(g, id, (Delegate)action); }
		public static void Unset<T0, T1, T2, T3>(GameObject g, MsgId id, Action<T0, T1, T2, T3> action) { Assert.IsNotNull(g); _Unset(g, id, (Delegate)action); }
		public static void Set<T0, T1, T2, T3, T4>(GameObject g, MsgId id, Action<T0, T1, T2, T3, T4> action) { Assert.IsNotNull(g); _Set(g, id, (Delegate)action); }
		public static void Unset<T0, T1, T2, T3, T4>(GameObject g, MsgId id, Action<T0, T1, T2, T3, T4> action) { Assert.IsNotNull(g); _Unset(g, id, (Delegate)action); }
		static void _Set(GameObject g, MsgId id, Delegate action)
		{
			// // Invoke中に呼び出された場合、呼び出し中リストに加える
			// if (_idToInvokingDelicates.TryGetValue(id, out var invoking_list) && !invoking_list.Contains(action))
			// {
			// }
			var gid = g != null ? g.GetInstanceID() : 0;
			var key = ((int)id, gid);
			if (!_idToDeligates.TryGetValue(key, out var set))
				_idToDeligates.Add(key, set = new HashSet<Delegate>());
			var r = set.Add(action);
			_LogSet(r, id, action);
			RegisteredTime[key] = Time.frameCount;
		}

		static void _Unset(GameObject g, MsgId id, Delegate action)
		{
			var gid = g != null ? g.GetInstanceID() : 0;
			bool r = false;
			var key = ((int)id, gid);
			if (_idToDeligates.TryGetValue(key, out var set))
			{
				r = set.Remove(action);
				// 登録がゼロになったら削除する
				if (set.Count() == 0)
				{
					_idToDeligates.Remove(key);
					RegisteredTime.Remove(key);
				}
			}
			_LogUnset(r, id, action);
		}

		public static void Unset(GameObject g)
		{
			var gid = g.GetInstanceID();
			bool r = false;
			foreach (var key in _idToDeligates.Keys.ToArray())
			{
				if (key.Item2 == gid)
				{
					r = true;
					_idToDeligates.Remove(key);
				}
			}
			_LogUnset(r, g);
		}

		static StringBuilder _sb;
		public static string GetState()
		{
#if DEBUG
			_sb ??= new StringBuilder();
			_sb.Clear();
			_sb.Append("Key Total: ").AppendLine(_idToDeligates.Count.ToString());
			_sb.Append("Value Total: ").AppendLine(_idToDeligates.Sum(e => e.Value.Count()).ToString());
			foreach (var i in _idToDeligates.OrderBy(e => e.Key.Item1))
			{
				_sb.AppendLine($"\tno=({(MsgId)i.Key.Item1}, objid={i.Key.Item2,8}), cnt={i.Value.Count,2}, reg={Time.frameCount - RegisteredTime[i.Key],5}");
				// _sb.AppendLine($"\tno=({i.Key.Item1,3}, objid={i.Key.Item2,8}), cnt={i.Value.Count,2}, reg={Time.frameCount - _registeredTime[i.Key],5}");
			}
			return _sb.ToString();
#else
			return "";
#endif
		}

		[System.Diagnostics.Conditional("MSG_LOG_ENABLE")]
		static void _LogSet(bool result, MsgId id, Delegate action)
		{
			if (result) Debug.Log($"<color=lightblue>Msg: Register({(int)id}.{id}) Success</color>");
			else if (IsOutputError) Debug.Log($"<color=yellow>Msg: Register({(int)id}.{id}) Failed</color>");
		}
		[System.Diagnostics.Conditional("MSG_LOG_ENABLE")]
		static void _LogUnset(bool result, MsgId id, Delegate action)
		{
			if (result) Debug.Log($"<color=lightblue>Msg: Unregister({(int)id}.{id}) Success</color>");
			else if (IsOutputError) Debug.Log($"<color=yellow>Msg: Unregister({(int)id}.{id}) Failed</color>");
		}
		[System.Diagnostics.Conditional("MSG_LOG_ENABLE")]
		static void _LogUnset(bool result, GameObject g)
		{
			if (result) Debug.Log($"<color=lightblue>Msg: Batch Unregister({g.name}) Success</color>");
			else if (IsOutputError) Debug.Log($"<color=yellow>Msg: Batch Unregister({g.name}) Failed</color>");
		}

		[System.Diagnostics.Conditional("MSG_LOG_ENABLE")]
		static void _LogInvoke(MsgId id)
		{
			if (!_ignoreLogMsgId.Contains(id))
				Debug.Log($"<color=lightblue>Msg: Invoke({(int)id}.{id})</color>");
		}
		[System.Diagnostics.Conditional("MSG_LOG_ENABLE")]
		static void _LogInvokeFailed(MsgId id, GameObject g = null)
		{
			if (IsOutputError && !_ignoreLogMsgId.Contains(id))
				Debug.Log($"<color=yellow>Msg: Invoke Failed({(int)id}.{id}, {g})</color>");
		}

		[System.Diagnostics.Conditional("DEBUG")] static void _Log(string s) => Debug.Log(s);
		[System.Diagnostics.Conditional("DEBUG")] static void _LogE(string s) => Debug.LogError(s);
#if !DEBUG
		[System.Diagnostics.Conditional("NEVER_CALLED")]
#endif
		public static void Dump()
		{
			var sb = new StringBuilder();
			foreach (var kvp in _idToDeligates)
			{
				sb.Clear();
				sb.Append($"{(MsgId)kvp.Key.id}, {kvp.Key.instanceId}, {kvp.Value.ElementAt(0)}");
				foreach (var d in kvp.Value)
				{
					sb.Append($"\n - {d.Method}/{d.Target}");
				}
				Debug.Log(sb.ToString());
			}
		}

#if UNITY_EDITOR
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		static void _DomainReset()
		{
			RegisteredTime = new Dictionary<(int, int), int>(32);
			_idToDeligates = new Dictionary<(int, int), HashSet<Delegate>>(32);
			_idToInvokingDelicates = new();
			_deligateListCaches = new();
		}
#endif

	}
}
