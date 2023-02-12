using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetricsMonitorClient.DataServices.MonitorSystem.Dtos
{
    public class PollDTO
    {
        public int PollId { get; set; }
        public int PollRate { get; set; }
        public string OperatingSystem { get; set; }
        public string OperatingSystemVersion { get; set; }
        public string PollType { get; set; }
        public DateTime Time { get; set; }
    }
}
