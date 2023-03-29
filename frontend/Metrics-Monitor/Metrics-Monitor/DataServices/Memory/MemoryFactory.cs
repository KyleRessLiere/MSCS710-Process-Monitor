using log4net;
using MetricsMonitorClient.DataServices.Memory.Dtos;
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
        private readonly ILog _logger;
        public MemoryFactory(ILog logger) {
            this._logger = logger;
        }
        public async Task<MemoryUsagePollDto> GetLatestMemoryPollAsync() {
            try {
                using (var client = new HttpClient()) {

                    var response = await client.GetAsync(MMConstants.BaseApiUrl + "/memory/latest");

                    if (response?.IsSuccessStatusCode ?? false) {
                        var responseContent = await response.Content.ReadAsStringAsync();

                        var result = JsonConvert.DeserializeObject<MemoryUsagePollDto>(responseContent);

                        return result;
                    }

                   throw new HttpRequestException("An Error occured making a get request");
                }
            } catch(Exception ex) {
                _logger.Error(ex);
                throw;
            }
        }



        public async Task<IEnumerable<MemoryUsagePollDto>> GetAllMemoryPollsAsync() {
            try {
                using (var client = new HttpClient()) {

                    var response = await client.GetAsync(MMConstants.BaseApiUrl + "/memory");

                    if (response?.IsSuccessStatusCode ?? false) {
                        var responseContent = await response.Content.ReadAsStringAsync();

                        var result = JsonConvert.DeserializeObject<List<MemoryUsagePollDto>>(responseContent);

                        return result.OrderByDescending(p => p.poll_id);
                    }

                    throw new HttpRequestException("An Error occured making a get request");
                }
            } catch (Exception ex) {
                _logger.Error(ex);
                throw;
            }
        }
    }
}
