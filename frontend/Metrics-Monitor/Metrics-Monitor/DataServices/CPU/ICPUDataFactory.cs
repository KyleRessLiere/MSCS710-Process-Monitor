using MetricsMonitorClient.Models.CPU;
using System.Collections.Generic;

namespace MetricsMonitorClient.DataServices.CPU
{
    public interface ICPUDataFactory {
        IEnumerable<CPUD> GetAllRecords();
    }
}