using MetricsMonitorClient.DataServices.MonitorSystem.Dtos;
using System.Collections.Generic;

namespace MetricsMonitorClient.DataServices.MonitorSystem {
    public interface IMonitorSystemFactory {
        MonitorSystemFactory Instance { get; }

        IEnumerable<PollDTO> GetAllRecords();
    }
}