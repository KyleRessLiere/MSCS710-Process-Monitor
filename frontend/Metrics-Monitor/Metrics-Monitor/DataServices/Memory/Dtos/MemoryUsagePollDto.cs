using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetricsMonitorClient.DataServices.Memory.Dtos {
    public class MemoryUsagePollDto {

        public decimal total_memory { get; set; }
        public decimal used_memory { get; set; }
        public decimal available_memory { get; set; }
        public decimal percentage_memory { get; set; }
        public int MemoryPollId { get; set; }

    }
}
