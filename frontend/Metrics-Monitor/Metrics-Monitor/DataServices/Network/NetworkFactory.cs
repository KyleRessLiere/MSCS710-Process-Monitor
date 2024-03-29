﻿using log4net;
using MetricsMonitorClient.DataServices.Network.Dtos;
using MetricsMonitorClient.Models.Network;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MetricsMonitorClient.DataServices.Network
{
    public class NetworkFactory : INetworkFactory {
        private readonly ILog _logger;

        public NetworkFactory(ILog logger) {
            this._logger = logger;
        }

        public async Task<IEnumerable<NetworkPoll>> GetLatestNetworkPollAsync() {
            try {
                using (var client = new HttpClient()) {

                    var response = await client.GetAsync(MMConstants.BaseApiUrl + "/networks/latest");

                    if (response?.IsSuccessStatusCode ?? false) {
                        var responseContent = await response.Content.ReadAsStringAsync();

                        var result = JsonConvert.DeserializeObject<List<NetworkDto>>(responseContent);
                        var mappedResult = result.Select(n => n.ToModel()).ToList();
                        return mappedResult;
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
