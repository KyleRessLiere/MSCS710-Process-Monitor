using MetricsMonitorClient.DataServices.Memory.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MetricsMonitorClient.DataServices.Memory {
    public interface IMemoryFactory {
        Task<MemoryUsagePollDto> GetLatestMemoryPollAsync();
        Task<IEnumerable<MemoryUsagePollDto>> GetAllMemoryPollsAsync();
    }
}