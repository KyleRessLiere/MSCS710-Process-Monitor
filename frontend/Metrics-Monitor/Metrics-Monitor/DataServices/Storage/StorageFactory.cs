using log4net;
using MetricsMonitorClient.DataServices.Storage.Dtos;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MetricsMonitorClient.DataServices.Storage {
    public class StorageFactory : IStorageFactory {

        private readonly ILog _logger;

        public StorageFactory(ILog logger) {
            this._logger = logger;
        }

        public async Task<StorageDto> GetLatestStoragePollAsync() {
            try {
                using (var client = new HttpClient()) {
                    var response = await client.GetAsync(MMConstants.BaseApiUrl + "/disks/latest");

                    if (response?.IsSuccessStatusCode ?? false) {
                        var responseContent = await response.Content.ReadAsStringAsync();

                        var result = JsonConvert.DeserializeObject<StorageDto>(responseContent);

                        return result;
                    }


                    throw new HttpRequestException("An error occured making a get request");

                }
            } catch (Exception ex) {
                _logger.Error(ex);
                throw;
            }
        }
    }
}
