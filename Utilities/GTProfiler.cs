using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Gtpx.ModelSync.CAD.Utilities
{
    /// <summary>
    /// This object can track 
    ///   1. How much time it takes to run a block of code
    ///   2. Memory usage around a block of code
    ///   3. Accumulated values
    ///   4. Fixed values
    /// </summary>
    public class GTProfiler
    {
        [Flags]
        public enum GTProfOptions { None = 0, Memory = 1, Time = 2, Values = 4, Stats = 8, All = 15 };

        // you can have multiple stop watches (_timerId is the default one)
        private readonly int _timerId = 0;
        static private List<Stopwatch> _sw = new List<Stopwatch>();

        // When you CatchTime, you save the length of time, and the number of times CatchTime was called
        static private Dictionary<string, long> _timings = new Dictionary<string, long>();
        static private Dictionary<string, long> _timingCounts = new Dictionary<string, long>();

        // When you call Remember you save the memory usage between instances of calls for the given key
        static private Dictionary<string, (long, long)> _memory = new Dictionary<string, (long, long)>();

        // Save stats for an item by name. For each key, remember its count, sum, min, max    
        static private Dictionary<string, (int, double, double, double)> _stats = new Dictionary<string, (int, double, double, double)>();

        // Save latest value (by name)
        static private Dictionary<string, double> _values = new Dictionary<string, double>();

        /// <summary>
        /// Set to false to turn off all stop watches
        /// </summary>
        static public bool TimerEnabled { get; set; } = true;

        /// <summary>
        /// If you are using multiple GTPProfiler() objects, you can give each one a unique id 
        /// </summary>
        public GTProfiler(int timerId = 0)
        {
            _timerId = timerId;
        }

        ~GTProfiler()
        {
            StopCollectingTime();
        }

        public void StopCollectingTime(int timerId = -1)
        {
            if (!TimerEnabled) return;
            if (timerId < 0) timerId = _timerId;
            if (timerId >= _sw.Count) RestartTimer(timerId);
            _sw[timerId].Stop();
        }

        public void RestartTimer(int timerId = -1)
        {
            if (!TimerEnabled) return;
            if (timerId < 0) timerId = _timerId;
            while (timerId >= _sw.Count)
            {
                _sw.Add(new Stopwatch());
            }
            _sw[timerId].Restart();
        }

        /// <summary>
        /// Accumulate a value, and keep the sum of its values 
        /// </summary>
        public void Accum(string name, double value)
        {
            if (!_stats.TryGetValue(name, out var data))
            {
                //             ct  sum    min    max   
                _stats[name] = (1, value, value, value);
            }
            else
            {
                data.Item1++; // ct
                data.Item2 += value; // sum
                if (value < data.Item3) data.Item3 = value; // min
                if (value > data.Item4) data.Item4 = value; // max
                _stats[name] = data;
            }
        }

        /// <summary>
        /// Save a value by name. 
        /// </summary>
        public void SaveValue(string name, int value)
        {
            _values[name] = value;
        }

        /// <summary>
        /// Save the amount of time since last RestartTimer() was called.
        /// After it saves the time, it restarts the timer to ready it for the next CatchTime()
        /// </summary>
        public void CatchTime(string timerId, int level = -1)
        {
            if (!TimerEnabled) return;
            if (level < 0) level = _timerId;
            if (level >= _sw.Count) RestartTimer(level);
            _sw[level].Stop();
            if (!_timings.ContainsKey(timerId))
            {
                _timings[timerId] = 0;
                _timingCounts[timerId] = 0;
            }
            _timings[timerId] += (_sw[level].ElapsedMilliseconds);
            _timingCounts[timerId]++;
            _sw[level].Restart();
        }

        /// <summary>
        /// Call before the block of code you want to get the memory count, and again after the block of code.
        /// You must call CatchMemory() in pairs of two, the second one saves the diff from the first..
        /// </summary>
        /// <param name="memoryId">The name of the memory you wish to track</param>
        public void CatchMemory(string memoryId)
        {
            var currentProcess = Process.GetCurrentProcess();
            if (!_memory.TryGetValue(memoryId, out var data))
            {
                _memory[memoryId] = (currentProcess.WorkingSet64, 0);
            }
            else if (data.Item1 != 0)
            {
                var diff = currentProcess.WorkingSet64 - data.Item1;
                Accum(memoryId, diff);  // we want stats on memory collection
                data.Item2 = data.Item2 + diff;
                data.Item1 = 0; // reset memory mark for next time Remember() is called      
                _memory[memoryId] = data;
            }
            else // get ready for next time we call
            {
                data.Item1 = currentProcess.WorkingSet64;
                _memory[memoryId] = data;
            }
        }

        /// <summary>
        /// Catch Time and get memory snapshot. 
        /// </summary>
        public void CatchTimeAndMemory(string timerId, string memoryId = "", int level = -1)
        {
            if (memoryId == "") memoryId = timerId;
            CatchTime(timerId, level);
            CatchMemory(memoryId);
        }

        public List<string> ToStrings(GTProfOptions options = GTProfOptions.All)
        {
            var msgs = new List<string>();
            if ((options & GTProfOptions.Time) == GTProfOptions.Time)
            {
                foreach (var kvp in _timings)
                {
                    // add 999 to force a round up
                    msgs.Add($"{((kvp.Value + 999) / 1000L)}, seconds, {_timingCounts[kvp.Key]}, hits, {kvp.Key}");
                }
            }

            if ((options & GTProfOptions.Stats) == GTProfOptions.Stats)
            {
                foreach (var kvp in _stats)
                {
                    if (!_memory.ContainsKey(kvp.Key)) // memory collection also gets stored in _stats, and we will report memory usage below
                    {
                        // ct  sum    min    max    avg
                        msgs.Add($"{kvp.Key} : ct : {kvp.Value.Item1:N0} sum: {kvp.Value.Item2:N0} min: {kvp.Value.Item3:N0} max: {kvp.Value.Item4:N0} avg: {(kvp.Value.Item2 / kvp.Value.Item1):N0}");
                    }
                }
            }

            if ((options & GTProfOptions.Values) == GTProfOptions.Values)
            {
                foreach (var kvp in _values)
                {
                    msgs.Add($"{kvp.Key} = {kvp.Value:N0}");
                }
            }

            if ((options & GTProfOptions.Memory) == GTProfOptions.Memory)
            {
                foreach (var memory in _memory)
                {
                    var hasStats = _stats.TryGetValue(memory.Key, out var data);
                    if (!hasStats || data.Item1 == 1)
                    {
                        msgs.Add($"{memory.Key} : {memory.Value.Item2:N0} bytes");
                    }
                    else
                    {
                        msgs.Add($"{memory.Key} : {(data.Item2 / data.Item1):N0} bytes avg. (collected {data.Item1:N0} times; {data.Item2:N0} bytes ttl; {data.Item3:N0} bytes min; {data.Item4:N0} bytes max)");
                    }
                }
                var currentProcess = Process.GetCurrentProcess();
                var memoryUsage = currentProcess.WorkingSet64;
                msgs.Add($"App Memory Usage: {memoryUsage:N0} bytes");
            }

            return msgs;
        }
    }
}