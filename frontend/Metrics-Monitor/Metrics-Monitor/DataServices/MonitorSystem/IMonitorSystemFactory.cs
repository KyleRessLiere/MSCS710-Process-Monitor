using MetricsMonitorClient.DataServices.MonitorSystem.Dtos;
using System.Threading.Tasks;

namespace MetricsMonitorClient.DataServices.MonitorSystem {
    public interface IMonitorSystemFactory {
        Task<CompositePollDto> GetAllLatestMetricsAsync();
    }
}