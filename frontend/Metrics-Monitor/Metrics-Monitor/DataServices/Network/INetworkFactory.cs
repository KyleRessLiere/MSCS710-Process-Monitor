using System.Collections.Generic;
using System.Threading.Tasks;
using MetricsMonitorClient.Models.Network;

namespace MetricsMonitorClient.DataServices.Network
{
    public interface INetworkFactory {
        Task<IEnumerable<NetworkPoll>> GetLatestNetworkPollAsync();
    }
}