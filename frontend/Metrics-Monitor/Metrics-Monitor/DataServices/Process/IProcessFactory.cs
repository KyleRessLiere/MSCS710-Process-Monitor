using System.Collections.Generic;
using System.Threading.Tasks;
using MetricsMonitorClient.Models.Process;

namespace MetricsMonitorClient.DataServices.Process
{
    public interface IProcessFactory {
        Task<List<ProcessPoll>> GetRunningProcessesAsync();
    }
}