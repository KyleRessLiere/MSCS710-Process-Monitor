using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetricsMonitorClient.DataServices.CPU.Dtos
{
    public class CPUDto
    {
        public string Info { get; set; }

        public decimal Usage { get; set; }

        public int Count { get; set; }
    }
}
