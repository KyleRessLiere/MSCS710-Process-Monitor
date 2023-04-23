using log4net;
using MetricsMonitorClient.DataServices.CPU.Dtos;
using MetricsMonitorClient.DataServices.Process.Dtos;
using MetricsMonitorClient.Models.Process;
using Newtonsoft.Json;
using NUnit.Framework.Constraints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MetricsMonitorClient.DataServices.Process
{
    public class ProcessFactory : IProcessFactory {

        private readonly ILog _logger;

        public ProcessFactory(ILog logger) {
            this._logger = logger;
        }
        public async Task<List<ProcessPoll>> GetRunningProcessesAsync() {
            try {
                using (var client = new HttpClient()) {
                    var response = await client.GetAsync(MMConstants.BaseApiUrl + "/processes/latest");

                    if (response?.IsSuccessStatusCode ?? false) {
                        var responseContent = await response.Content.ReadAsStringAsync();

                        var result = JsonConvert.DeserializeObject<List<ProcessDto>>(responseContent);

                        var mappedResult = result.Select(p => p.ToModel()).ToList();

                        return mappedResult;
                    }

                    throw new HttpRequestException("An error occured making a get request to the Latest Processes endpoint.");
                }
            } catch (Exception ex) {
                _logger.Error(ex);
                throw;
            }
        }

    }
}
