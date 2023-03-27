using MetricsMonitorClient.DataServices.CPU.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MetricsMonitorClient.DataServices.CPU {
    public interface ICPUFactory {
        Task<IEnumerable<CPUDto>> GetAllCPUPollsAsync();
        Task<CPUDto> GetLatestCPUPollAsync();
    }
}