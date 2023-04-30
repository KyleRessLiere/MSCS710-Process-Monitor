using ReactiveUI;

namespace MetricsMonitorClient.Models.Process {
   /// <summary>
   /// Summarized records of a single process type
   /// </summary>
    public class ProcessPollSlim : ReactiveObject{

        private string _processName;
        /// <summary>
        /// Name of the process grouping
        /// </summary>
        public string ProcessName {
            get { return _processName; }
            set { this.RaiseAndSetIfChanged(ref _processName, value); }
        }

        private double _cpuUsagePctTotal;
        /// <summary>
        /// Total CPU Usage for processes of this name
        /// </summary>
        public double CpuUsagePctTotal {
            get { return _cpuUsagePctTotal; }
            set { this.RaiseAndSetIfChanged(ref _cpuUsagePctTotal, value); }
        }

        private double _memoryUsagePctTotal;
        /// <summary>
        /// Total memory usage for processes of this name
        /// </summary>
        public double MemoryUsagePctTotal {
            get { return _memoryUsagePctTotal; }
            set { this.RaiseAndSetIfChanged(ref _memoryUsagePctTotal, value); }
        }


        private int _processCount;
        /// <summary>
        /// Amount of individual processes in this grouping
        /// </summary>
        public int ProcessCount {
            get { return _processCount; }
            set { this.RaiseAndSetIfChanged(ref _processCount, value); }
        }

    }
}
