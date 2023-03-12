using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetricsMonitorClient.DataServices.Memory.Dtos {
    public class MemoryUsagePollDto {

        public double total_memory { get; set; }
        public double used_memory { get; set; }
        public double available_memory { get; set; }
        public double percentage_memory { get; set; }
        public int MemoryPollId { get; set; }

    }
}
