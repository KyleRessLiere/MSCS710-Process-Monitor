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
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace MetricsMonitorClient.DataServices.MonitorSystem {
    public class MonitorSystemFactory : IMonitorSystemFactory {

        public MonitorSystemFactory() { }


        public IEnumerable<PollDTO> GetAllRecords() {
            try {
                List<PollDTO>? polls = new List<PollDTO>();
                using (StreamReader r = new StreamReader(@"C:\SANDBOX\mcapping\MSCS710-Process-Monitor\FrontEnd\Metrics-Monitor\Metrics-Monitor\DataServices\TEST_DATA\polls.json")) {   //example file is in source control
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

        //public async Task<bool> SetPollRate(int pollRate) {
        //    try {
               
        //    }
        //}



    }
}
