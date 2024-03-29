﻿using MetricsMonitorClient.DataServices.MonitorSystem.Dtos;
using System.Threading.Tasks;

namespace MetricsMonitorClient.DataServices.MonitorSystem {
    public interface IMonitorSystemFactory {
        Task<CompositePollDto> GetAllLatestMetricsAsync();
        Task<PollDTO> GetLatestServiceInfoAsync();
        Task<bool> SetPollRate(double pollRateInSeconds);
    }
}