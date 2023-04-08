using MetricsMonitorClient.DataServices.Storage.Dtos;
using System.Threading.Tasks;

namespace MetricsMonitorClient.DataServices.Storage {
    public interface IStorageFactory {
        Task<StorageDto> GetLatestStoragePollAsync();
    }
}