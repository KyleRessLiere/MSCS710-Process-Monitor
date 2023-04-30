using MetricsMonitorClient.DataServices.Network.Dtos;
using MetricsMonitorClient.Models.Network;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetricsMonitorClient.DataServices.Network
{
    public static class NetworkMapper {

        public static NetworkPoll ToModel(this NetworkDto source) {
            NetworkPoll target = new NetworkPoll();
            target.Id = source.network_id;
            target.Interface = source.network_interface;
            target.Speed = source.network_speed;
            target.Status = NetStatNameToId(source.network_status);
            target.PollId = source.poll_id;
            return target;
        }

        public static int NetStatNameToId(string status) => MMConstants.NetworkStatus_NameToId_Map.TryGetValue(status, out var netStatus) ? netStatus : MMConstants.NetworkStatus_Unknown_Id;

        public static string NetStatIdToName(int id) => MMConstants.NetworkStatus_IdToName_Map.TryGetValue(id, out var netStatus) ? netStatus : MMConstants.NetworkStatus_Unknown;

    }
}
