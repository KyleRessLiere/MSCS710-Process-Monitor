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
            Storage = 3,
            Network = 4
        };
        /// <summary>
        /// used when converting b <-> gb 
        /// </summary>
        public const double OneBillion = 1000000000.0;

    }
}
