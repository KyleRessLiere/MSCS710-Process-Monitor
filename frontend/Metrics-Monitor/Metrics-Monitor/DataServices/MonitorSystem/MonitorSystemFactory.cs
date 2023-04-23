using log4net;
using MetricsMonitorClient.DataServices.CPU;
using MetricsMonitorClient.DataServices.CPU.Dtos;
using MetricsMonitorClient.DataServices.MonitorSystem.Dtos;
using MetricsMonitorClient.DataServices.Network.Dtos;
using MetricsMonitorClient.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace MetricsMonitorClient.DataServices.MonitorSystem {
    public class MonitorSystemFactory : IMonitorSystemFactory {
        private readonly ILog _logger;

        public MonitorSystemFactory(ILog logger) {
            this._logger = logger;
        }

        public async Task<CompositePollDto> GetAllLatestMetricsAsync() {
            try {
                using (var client = new HttpClient()) {

                    var response = await client.GetAsync(MMConstants.BaseApiUrl + "/metrics/latest");

                    if (response?.IsSuccessStatusCode ?? false) {
                        var responseContent = await response.Content.ReadAsStringAsync();

                        var result = JsonConvert.DeserializeObject<CompositePollDto>(responseContent);
                        return result;
                    }

                    throw new HttpRequestException("An error occured making a get request.");
                }
            } catch (Exception ex) {
                _logger.Error(ex);
                throw;
            }
        }




    }
}
