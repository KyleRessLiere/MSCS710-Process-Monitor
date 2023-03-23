﻿using MetricsMonitorClient.DataServices.Memory.Dtos;
using MetricsMonitorClient.DataServices.MonitorSystem.Dtos;
using Newtonsoft.Json;
using Splat;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MetricsMonitorClient.DataServices.Memory {
    public class MemoryFactory : IMemoryFactory {

        public MemoryFactory(ILogger logger) {
            this._logger = logger;
        }



        private readonly ILogger _logger;
        public MemoryFactory() {
        }

        public async Task<MemoryUsagePollDto> GetLatestMemoryPollAsync() {
            try {
                using (var client = new HttpClient()) {

                    var response = await client.GetAsync(MMConstants.BaseApiUrl + "/memory");

                    if (response?.IsSuccessStatusCode ?? false) {
                        var responseContent = await response.Content.ReadAsStringAsync();

                        var result = JsonConvert.DeserializeObject<List<MemoryUsagePollDto>>(responseContent);

                        return result.OrderByDescending(p => p.poll_id).FirstOrDefault();
                    }

                   throw new HttpRequestException("An Error occured making a get request");
                }
            } catch(Exception ex) {
                _logger.Write($"an error occurred.\n{ex.Message}", LogLevel.Error);
                throw;
            }
        }
    }
}
