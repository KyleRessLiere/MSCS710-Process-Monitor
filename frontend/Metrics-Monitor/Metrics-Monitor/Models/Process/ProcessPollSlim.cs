using ReactiveUI;

namespace MetricsMonitorClient.Models.Process {
    public class ProcessPollSlim : ReactiveObject{

        private string _processName;
        public string ProcessName {
            get { return _processName; }
            set { this.RaiseAndSetIfChanged(ref _processName, value); }
        }

        private double _cpuUsagePctTotal;
        public double CpuUsagePctTotal {
            get { return _cpuUsagePctTotal; }
            set { this.RaiseAndSetIfChanged(ref _cpuUsagePctTotal, value); }
        }

        private double _memoryUsagePctTotal;
        public double MemoryUsagePctTotal {
            get { return _memoryUsagePctTotal; }
            set { this.RaiseAndSetIfChanged(ref _memoryUsagePctTotal, value); }
        }


    }
}
