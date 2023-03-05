using MetricsMonitorClient.DataServices.Memory.Dtos;
using System.Collections.Generic;

namespace MetricsMonitorClient.DataServices.Memory {
    public interface IMemoryFactory {
        int Index { get; }
        List<MemoryUsagePollDto> MemoryUsagePolls { get; set; }

        MemoryUsagePollDto GetLatestMemoryPoll();
    }
}