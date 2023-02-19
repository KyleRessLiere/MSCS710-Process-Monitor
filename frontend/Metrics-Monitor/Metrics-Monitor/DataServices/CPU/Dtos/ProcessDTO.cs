using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetricsMonitorClient.DataServices.CPU.Dtos {
    public class ProcessDTO {
        public int PollId { get; set; }
        public int ProcessId { get; set; }
        public string ProcessName { get; set; }
        public string ProcessStatus { get; set; }
        public decimal CpuPercent { get; set; }
        public int NumThread { get; set; }
        public decimal MemoryUsage { get; set; }

    }
}
