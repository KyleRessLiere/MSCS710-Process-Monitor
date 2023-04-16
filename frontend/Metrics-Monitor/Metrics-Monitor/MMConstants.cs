using System;
using System.Collections.Generic;
using System.Collections.Immutable;
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
            Storage = 3,
            Network = 4
        };
        /// <summary>
        /// used when converting b <-> gb 
        /// </summary>
        public const double OneBillion = 1000000000.0;

        
        public const string NetworkStatus_Up = "Up";
        public const string NetworkStatus_Down = "Down";
        public const string NetworkStatus_Unknown = "?";

        public const int NetworkStatus_Unknown_Id = -1;
        public const int NetworkStatus_Down_Id = 0;
        public const int NetworkStatus_Up_Id = 1;

        public static Dictionary<string, int> NetworkStatus_NameToId_Map = new Dictionary<string, int> {
            { NetworkStatus_Down, NetworkStatus_Down_Id}
           ,{NetworkStatus_Up, NetworkStatus_Up_Id}
           ,{NetworkStatus_Unknown, NetworkStatus_Unknown_Id}
        };

        public static Dictionary<int, string> NetworkStatus_IdToName_Map = MMConstants.NetworkStatus_NameToId_Map.ToDictionary(k => k.Value, v => v.Key);

    }
}
