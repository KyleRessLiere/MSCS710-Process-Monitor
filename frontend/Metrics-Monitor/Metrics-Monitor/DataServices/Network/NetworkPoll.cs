using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetricsMonitorClient.DataServices.Network
{
    public class NetworkPoll
    {
        public int Id { get; set; }
        public string Interface { get; set; }
        public double Speed { get; set; }
        public int Status { get; set; }
        public int PollId { get; set; }
    }
}
