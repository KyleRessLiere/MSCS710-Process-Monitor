﻿using System.Collections.Generic;
using System.Threading.Tasks;

namespace MetricsMonitorClient.DataServices.Network
{
    public interface INetworkFactory {
        Task<IEnumerable<NetworkPoll>> GetLatestNetworkPollAsync();
    }
}