using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetricsMonitorClient.DataServices.CPU.Dtos
{
    public class CPUDto : IDto {
        public int cpu_count_physical { get; set; }
        public double[] cpu_count_virtual { get; set; }
        public long cpu_ctx_switches { get; set; }
        public int cpu_id { get; set; }
        public double cpu_percent { get; set; }
        public double[] cpu_percentage_per_core { get; set; }
        public long interrupts { get; set; }
        public long soft_interrupts { get; set;}
        public long syscalls { get; set; }
        public int poll_id { get; set; }
    }
}
