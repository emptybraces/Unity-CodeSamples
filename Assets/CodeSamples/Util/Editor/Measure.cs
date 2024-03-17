using System;
using System.Diagnostics;
namespace Emptybraces.Editor
{
	public readonly struct Measure : IDisposable
	{
		public static Stopwatch SW;
		readonly string id;
		readonly bool log;
		public Measure(string profileId, bool log = true)
		{
			SW ??= new Stopwatch();
			SW.Restart();
			UnityEngine.Profiling.Profiler.BeginSample(profileId);
			id = profileId;
			this.log = log;
		}
		void IDisposable.Dispose()
		{
			UnityEngine.Profiling.Profiler.EndSample();
			SW.Stop();
			if (log) cn.log(id, SW.Elapsed);
		}
		
		[Conditional("DEBUG")]
		public static void Start(string id)
		{
			SW ??= new Stopwatch();
			SW.Restart();
			UnityEngine.Profiling.Profiler.BeginSample(id);
		}
		[Conditional("DEBUG")]
		public static void End(string id, bool log = true)
		{
			UnityEngine.Profiling.Profiler.EndSample();
			SW.Stop();
			if (log) cn.log(id, SW.Elapsed);
		}
	}
}
