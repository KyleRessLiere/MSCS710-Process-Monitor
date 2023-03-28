using MetricsMonitorClient.DataServices;
using MetricsMonitorClient.DataServices.CPU.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetricsMonitorClient.Models.CPU {
    public class CpuStatsContainer : StatsContainerBase {
        public CpuStatsContainer() {
            PollList = new List<double>();
        }
       
    }
}
