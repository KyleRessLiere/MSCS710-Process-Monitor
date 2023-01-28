using System.Collections.Generic;

namespace MetricsMonitor.Services {
    public interface ICPUDataFactory {
        IEnumerable<string> GetAllRecords();
    }
}