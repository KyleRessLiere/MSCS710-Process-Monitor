using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetricsMonitorClient.DataServices.Process {
    public class ProcessPoll {
        public int Id { get; set; }
        public double CpuPercent { get; set; }
        public double MemoryUsage { get; set; }
        public int ThreadNumber { get; set; }
        public int PollId { get; set; }
        public string ProcessName { get; set; }
        public int ProcessStatus { get; set; }
    }
}
