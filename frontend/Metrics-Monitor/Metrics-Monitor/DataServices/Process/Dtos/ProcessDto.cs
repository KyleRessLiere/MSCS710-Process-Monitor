using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetricsMonitorClient.DataServices.Process.Dtos {
    public class ProcessDto {
        public double cpu_percent { get; set; }
        public double memory_usage { get; set; }
        public int num_thread { get; set; }
        public int poll_id { get; set; }
        public int process_id { get; set; }
        public string process_name { get; set; }
        public string process_status { get; set; }
        
    }
}
