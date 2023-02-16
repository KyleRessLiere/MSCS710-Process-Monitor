using MetricsMonitorClient.DataServices.CPU.Dtos;
using MetricsMonitorClient.Models.CPU;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetricsMonitorClient.DataServices.CPU
{
    public static class CPUDataMapper {
        public static CPUDto CPUD_ToDto(CPUD source) {
            return new CPUDto() {
                Count = source.Count,
                Usage = source.Usage,
                Info = source.Info
            };
        }
        public static CPUD CPUD_ToModel(CPUDto source) {
            CPUD target= new CPUD();
            using(var changeBlock = target.SuppressChangeNotifications()) {
                target.Count = source.Count;
                target.Info = source.Info;
                target.Usage = source.Usage;
            }
            return target;
        }

    }
}
