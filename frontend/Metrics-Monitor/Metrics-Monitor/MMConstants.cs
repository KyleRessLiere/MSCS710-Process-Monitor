using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetricsMonitorClient {
    public static class MMConstants {
        /// <summary>
        /// sleep period between clock ticks in milliseconds
        /// </summary>
        public const int SystemClockInterval = 500;
        public const int PollBufferSize = 15;
        public const string BaseApiUrl = "http://127.0.0.1:5000/api";
        public const int StatsContainerMaxBuffer = 100;

        public enum ResourceTabIndex {
            Overview = 0,
            CPU = 1,
            Memory = 2,
            Storage = 3
        };
        /// <summary>
        /// used when converting b <-> gb 
        /// </summary>
        public const double OneBillion = 1000000000.0;

        public const string ProcessStatus_Unkown = "?";
        public const string ProcessStatus_Stopped = "stopped";
        public const string ProcessStatus_Running = "running";

        public const int ProcessStatusId_Unknown = -1;
        public const int ProcessStatusId_Stopped = 0;
        public const int ProcessStatusId_Running = 1;


        public static Dictionary<string, int> ProcessStatus_IdToName_Map = new Dictionary<string, int> {
            { ProcessStatus_Unknown, ProcessStatusId_Unkonwn},
            { ProcessStatus_Stopped, ProcessStatusId_Stopped},
            { ProcessStatus_Running, ProcessStatusId_Running}
        };

        public static Dictionary<int, string> ProcessStatus_NameToId_Map = ProcessStatus_IdToName_Map.ToDictionary(k => k.Value, v => v.Key);

    }
}
