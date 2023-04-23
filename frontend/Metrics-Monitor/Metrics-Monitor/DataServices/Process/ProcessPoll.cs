using Avalonia.Collections;
using ReactiveUI;

namespace MetricsMonitorClient.DataServices.Process {
    public class ProcessPoll : ReactiveObject{

        public ProcessPoll() {
            Processes = new AvaloniaList<ProcessPoll>();
        }

        public ProcessPoll(ProcessPoll proc) {
            if(Processes == null) {
                Processes = new AvaloniaList<ProcessPoll>();
            }
            Processes.Add(proc);
            CpuUsagePctTotal = proc.CpuUsagePctTotal;
            MemoryUsagePctTotal = proc.MemoryUsagePctTotal;
        }

        public int Id { get; set; }
      
        private string _processName;
        public string ProcessName {
            get { return _processName; }
            set { this.RaiseAndSetIfChanged(ref _processName, value); }
        }


        private int _processStatus;
        public int ProcessStatus {
            get { return _processStatus; }
            set {
                this.RaiseAndSetIfChanged(ref _processStatus, value);
                ProcessStatusText = ProcessMapper.ProcessStatIdToName(value);
            }
        }


        private string _processStatusText;
        public string ProcessStatusText {
            get { return _processStatusText; }
            set {
                this.RaiseAndSetIfChanged(ref _processStatusText, value);
            }
        }

        private int _threadNumber;
        public int ThreadNumber {
            get { return _threadNumber; }
            set { this.RaiseAndSetIfChanged(ref _threadNumber, value); }
        }
        private double _cpuPercent;
        public double CpuPercent {
            get { return _cpuPercent; }
            set { this.RaiseAndSetIfChanged(ref _cpuPercent, value); }
        }

        private double _memoryUsage;
        public double MemoryUsage {
            get { return _memoryUsage; }
            set { this.RaiseAndSetIfChanged(ref _memoryUsage, value); }
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

        public int PollId { get; set; }

        public AvaloniaList<ProcessPoll> Processes { get; set; }
    }
}
