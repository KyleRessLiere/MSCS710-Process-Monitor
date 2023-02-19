using MetricsMonitorClient.DataServices.CPU;
using MetricsMonitorClient.DataServices.CPU.Dtos;
using MetricsMonitorClient.DataServices.MonitorSystem.Dtos;
using MetricsMonitorClient.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace MetricsMonitorClient.DataServices.MonitorSystem {
    public class MonitorSystemFactory : IMonitorSystemFactory {
        private MonitorSystemFactory _instance;
        public MonitorSystemFactory Instance {
            get {
                if (_instance == null) {
                    _instance = new MonitorSystemFactory();
                }
                return _instance;
            }
        }



        public IEnumerable<PollDTO> GetAllRecords() {
            try {
                List<PollDTO>? polls = new List<PollDTO>();
                using (StreamReader r = new StreamReader(@"C:\MM_TestData\polls.json")) {   //example file is in source control
                    string json = r.ReadToEnd();
                    var dbItems = JsonConvert.DeserializeObject<List<PollDTO>>(json);
                    if (dbItems.Any()) {
                       polls.AddRange(dbItems);
                    }
                }
            return polls;
            } catch (Exception ex) {
                throw;
            }
        }



    }
}
