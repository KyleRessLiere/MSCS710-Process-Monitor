using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetricsMonitorClient.DataServices.Storage.Dtos
{
    public class StorageDto {
        public long disk_free { get; set; }
        public int disk_id { get; set; }
        public double disk_percentage { get; set; }
        public long disk_total { get; set; }
        public long disk_used { get; set; }
        public int poll_id {get;set;}
    }
}
