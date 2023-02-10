using System.Collections.Generic;

namespace MetricsMonitorClient.Services {
    public interface ICPUDataFactory {
        CPUDataFactory Instance { get; }

        IEnumerable<string> GetAllRecords();
    }
}