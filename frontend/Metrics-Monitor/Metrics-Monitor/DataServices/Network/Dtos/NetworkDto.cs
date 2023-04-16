using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetricsMonitorClient.DataServices.Network.Dtos {
    public class NetworkDto {
        public int network_id { get; set; }
        public string network_interface { get; set; }
        public double network_speed { get; set; }
        public string network_status { get; set; }
        public int poll_id { get; set; }
    }
}
