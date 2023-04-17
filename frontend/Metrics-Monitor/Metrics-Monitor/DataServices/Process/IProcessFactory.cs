using System.Collections.Generic;
using System.Threading.Tasks;

namespace MetricsMonitorClient.DataServices.Process {
    public interface IProcessFactory {
        Task<List<ProcessPoll>> GetRunningProcessesAsync();
    }
}