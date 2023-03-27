using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Logging;
using log4net.Core;
using log4net.Repository.Hierarchy;
using MetricsMonitorClient.DataServices.CPU.Dtos;
using MetricsMonitorClient.Models.CPU;

namespace MetricsMonitorClient.DataServices.CPU
{
    public class CPUDataFactory : ICPUDataFactory {

        public CPUDataFactory() { }
        public IEnumerable<CPUD> GetAllRecords() {
            try {
                var dtoList = new List<CPUDto> {
                    new CPUDto() { Info = "Text1", Usage = .333M, Count = 2 },
                    new CPUDto() { Info = "Text2", Usage = .333M, Count = 2 },
                    new CPUDto() { Info = "Text3", Usage = .333M, Count = 2 },
                    new CPUDto() { Info = "Text4", Usage = .333M, Count = 2 },
                    new CPUDto() { Info = "Text5", Usage = .333M, Count = 2 },
                };

                var models = dtoList.Select(d => CPUDataMapper.CPUD_ToModel(d)).ToList();
                return models;
            }catch(Exception ex) {
                throw;
            }
        }
    }
}
