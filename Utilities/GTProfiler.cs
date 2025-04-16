using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;


namespace Gtpx.ModelSync.CAD.Utilities
{

    public class ProfilerStats
    {
        public string Key { get; set; }
        public long HitCount { get; set; }
        public long Milliseconds { get; set; }
        public long Memory { get; set; } 
        public long ParameterCount { get; set; }
        public List<string> ElementIds { get; set; } = new List<string>();
    }

    public class AccumStats
    {
        public int Count { get; set; }
        public double Sum { get; set; }
        public double Min { get; set; }
        public double Max { get; set; }
        public double Avg { get { return (Count > 0) ? Sum / Count : Sum; } }

        public AccumStats(double value)
        {
            Count = 1;
            Sum = value;
            Min = value;
            Max = value;
        }

        public AccumStats()
        {
            Count = 0;
            Sum = 0;
            Min = 0;
            Max = 0;
        }
    }

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
        private int _timerId = 0;
        static private List<Stopwatch> _sw = new List<Stopwatch>();

        // When you CatchTime, you save the length of time in milliseconds, and the number of times CatchTime was called
        static private Dictionary<string, long> _timings = new Dictionary<string, long>();
        static private Dictionary<string, long> _timingCounts = new Dictionary<string, long>();

        // When you call Remember you save the memory usage between instances of calls for the given key
        static private Dictionary<string, (long, long)> _memory = new Dictionary<string, (long, long)>();

        // Save stats for an item by name. For each key, remember its count, sum, min, max    
        static private Dictionary<string, AccumStats> _stats = new Dictionary<string, AccumStats>();

        // Save latest value (by name)
        static private Dictionary<string, double> _values = new Dictionary<string, double>();

        static private Dictionary<string, List<string>> _templateIdToElementId = new Dictionary<string, List<string>>();

        /// <summary>
        /// Set to false to turn off all stop watches
        /// </summary>
        static public bool TimerEnabled { get; set; } = true;

        private List<ProfilerStats> _statsCache = new List<ProfilerStats>();
        public void Reset()
        {
            StopCollectingTime();
            _statsCache = new List<ProfilerStats>();
            _sw = new List<Stopwatch>();
            _timings = new Dictionary<string, long>();
            _timingCounts = new Dictionary<string, long>();
            _memory = new Dictionary<string, (long, long)>();
            _stats = new Dictionary<string, AccumStats>();
            _values = new Dictionary<string, double>();
            _templateIdToElementId = new Dictionary<string, List<string>>();
            TimerEnabled = true;
        }

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

        static public void AddElementId(string key, string elementId)
        {
            if (_templateIdToElementId.ContainsKey(key))
            {
                _templateIdToElementId[key].Add(elementId);                
            }
            else
            {
                _templateIdToElementId[key] = new List<string> { elementId };
            }
        }

        /// <summary>
        /// Accumulate a value, and keep the sum of its values 
        /// </summary>
        static public void Accum(string name, double value)
        {
            if (!_stats.TryGetValue(name, out var data))
            {
                _stats[name] = new AccumStats(value);
            }
            else
            {
                data.Count++; // ct
                data.Sum += value; // sum
                if (value < data.Min) data.Min = value; // min
                if (value > data.Max) data.Max = value; // max
                _stats[name] = data;
            }
        }

        /// <summary>
        /// Save a value by name. 
        /// </summary>
        static public void SaveValue(string name, int value)
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
        static public void CatchMemory(string memoryId)
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

        public List<ProfilerStats> SortedList()
        {
            // Create a sorted list sorted by Milliseconds
            var sortedByMS = (from entry in _timings orderby entry.Value descending select entry).ToList<KeyValuePair<string, long>>();
            for (int i = 0; i < sortedByMS.Count; i++)
            {
                var key = sortedByMS[i].Key;
                var memory = 0L;
                if (_stats.TryGetValue(key, out var stats))
                {
                    memory = (long)stats.Sum;
                }

                // take of the word ElementExtractor.
                var templateId = key.Contains('.') ? key.Substring(key.IndexOf('.') + 1) : key;
                
                if (i < _statsCache.Count)
                {
                    _statsCache[i].Key = key;
                    _statsCache[i].Milliseconds = sortedByMS[i].Value;
                    _statsCache[i].Memory = memory;
                    _statsCache[i].HitCount = _timingCounts[key];
                    _statsCache[i].ParameterCount = _stats.ContainsKey($"Parameters.{templateId}") ? (long) _stats[$"Parameters.{templateId}"].Avg : -1;
                    _statsCache[i].ElementIds = _templateIdToElementId.ContainsKey(key) ? _templateIdToElementId[key] : new List<string>();
                }
                else
                {
                    _statsCache.Add(new ProfilerStats {
                        Key = key,
                        Milliseconds = sortedByMS[i].Value,
                        Memory = memory,
                        HitCount = _timingCounts[key],
                        ParameterCount = _stats.ContainsKey($"Parameters.{templateId}") ? (long)_stats[$"Parameters.{templateId}"].Avg : -1,
                        ElementIds = _templateIdToElementId.ContainsKey(key) ? _templateIdToElementId[key] : new List<string>()
                    });
                }
            }
            return _statsCache;
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
                        var accum = kvp.Value;
                        msgs.Add($"{kvp.Key} : ct : {accum.Count:N0} sum: {accum.Sum:N0} min: {accum.Min:N0} max: {accum.Max:N0} avg: {accum.Avg:N0}");
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
                    if (!hasStats || data.Count == 1)
                    {
                        msgs.Add($"{memory.Key} : {memory.Value.Item2:N0} bytes");
                    }
                    else
                    {
                        msgs.Add($"{memory.Key} : {(data.Avg):N0} bytes avg. (collected {data.Count:N0} times; {data.Sum:N0} bytes ttl; {data.Min:N0} bytes min; {data.Max:N0} bytes max)");
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