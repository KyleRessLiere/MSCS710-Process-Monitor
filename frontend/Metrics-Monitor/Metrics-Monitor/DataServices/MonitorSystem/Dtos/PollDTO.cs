using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetricsMonitorClient.DataServices.MonitorSystem.Dtos
{
    public class PollDTO
    {
        public int poll_id { get; set; }
        public double poll_rate { get; set; }
        public string operating_system { get; set; }
        public string operating_system_version { get; set; }
        public string poll_type { get; set; }
        public DateTime time { get; set; }
    }
}
