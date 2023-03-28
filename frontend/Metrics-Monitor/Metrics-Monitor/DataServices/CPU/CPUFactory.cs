using log4net;
using MetricsMonitorClient.DataServices.CPU.Dtos;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MetricsMonitorClient.DataServices.CPU {
    public class CPUFactory : ICPUFactory {
        private readonly ILog _logger;
        public CPUFactory(ILog logger) {
            this._logger = logger;
        }


        public async Task<CPUDto> GetLatestCPUPollAsync() {
            try {
                using (var client = new HttpClient()) {
                    var response = await client.GetAsync(MMConstants.BaseApiUrl + "/cpu");

                    if (response?.IsSuccessStatusCode ?? false) {
                        var responseContent = await response.Content.ReadAsStringAsync();

                        var result = JsonConvert.DeserializeObject<List<CPUDto>>(responseContent);
                        return result.OrderByDescending(p => p.poll_id).FirstOrDefault();
                    }

                    throw new HttpRequestException("An error occurred making a get request");
                }
            } catch (Exception ex) {
                _logger.Error(ex);
                throw;
            }
        }

        public async Task<IEnumerable<CPUDto>> GetAllCPUPollsAsync() {
            try {
                using (var client = new HttpClient()) {
                    var response = await client.GetAsync(MMConstants.BaseApiUrl + "/cpu");

                    if (response?.IsSuccessStatusCode ?? false) {
                        var responseContent = await response.Content.ReadAsStringAsync();

                        var result = JsonConvert.DeserializeObject<List<CPUDto>>(responseContent);

                        return result;
                    }

                    throw new HttpRequestException("An error occurred making a get request");
                }
            } catch (Exception ex) {
                _logger.Error(ex);
                throw;
            }
        }
    }
    }
