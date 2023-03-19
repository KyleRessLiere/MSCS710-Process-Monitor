using MetricsMonitorClient.DataServices.Memory.Dtos;
using MetricsMonitorClient.DataServices.MonitorSystem.Dtos;
using Newtonsoft.Json;
using Splat;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetricsMonitorClient.DataServices.Memory {
    public class MemoryFactory : IMemoryFactory {

        public MemoryFactory(ILogger logger) {
            this._logger = logger;
        }

        private readonly ILogger _logger;

        public List<MemoryUsagePollDto> MemoryUsagePolls { get; set; }
        private int index;
        public int Index {
            get {
                index = index <= (MemoryUsagePolls.Count - 2) ? index++ : 0;
                if(index > MemoryUsagePolls.Count - 2) {
                    index = 0;
                    return index;
                }
                return ++index;
            }
        }

        public MemoryFactory() {
            MemoryUsagePolls = new List<MemoryUsagePollDto>();
            index = 0;
            LoadMemoryPolls();
        }


        private void LoadMemoryPolls() {
            try {
                List<MemoryUsagePollDto>? polls = new List<MemoryUsagePollDto>();
                string dataLocation = "C:\\SANDBOX\\mcapping\\MSCS710-Process-Monitor\\frontend\\Metrics-Monitor\\Metrics-Monitor\\DataServices\\TEST_DATA\\MockMemData.json";
                using (StreamReader r = new StreamReader(dataLocation)) {
                    string json = r.ReadToEnd();
                    var dbItems = JsonConvert.DeserializeObject<List<MemoryUsagePollDto>>(json);
                    if (dbItems != null) {
                        MemoryUsagePolls.AddRange(dbItems);
                    }
                }
            } catch (Exception ex) {
                _logger.Write($"an error occured.\n{ex.Message}", LogLevel.Error);
                throw;
            }
        }


        public MemoryUsagePollDto GetLatestMemoryPoll() {
            if (MemoryUsagePolls != null && MemoryUsagePolls.Any()) {
                return MemoryUsagePolls[Index];
            }
            return new MemoryUsagePollDto();
        }
    }
}
