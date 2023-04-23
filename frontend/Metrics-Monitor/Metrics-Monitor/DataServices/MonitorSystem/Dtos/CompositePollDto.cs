using MetricsMonitorClient.DataServices.CPU.Dtos;
using MetricsMonitorClient.DataServices.Memory.Dtos;
using MetricsMonitorClient.DataServices.Network.Dtos;
using MetricsMonitorClient.DataServices.Process.Dtos;
using MetricsMonitorClient.DataServices.Storage.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetricsMonitorClient.DataServices.MonitorSystem.Dtos {
    public class CompositePollDto {
        public CPUDto cpu { get; set; }
        public StorageDto disk { get; set; }
        public MemoryUsagePollDto memory { get; set; }
        public IEnumerable<NetworkDto> network { get; set; }
        public IEnumerable<ProcessDto> processes { get; set; }

    }
}
