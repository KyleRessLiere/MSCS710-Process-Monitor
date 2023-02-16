using MetricsMonitorClient.Models.CPU;
using System.Collections.Generic;

namespace MetricsMonitorClient.DataServices.CPU
{
    public interface ICPUDataFactory {
        CPUDataFactory Instance { get; }

        IEnumerable<CPUD> GetAllRecords();
    }
}