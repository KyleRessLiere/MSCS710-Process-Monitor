using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Logging;
using log4net;
using MetricsMonitorClient.DataServices.CPU.Dtos;
using MetricsMonitorClient.DataServices.Memory.Dtos;
using MetricsMonitorClient.Models.CPU;
using Newtonsoft.Json;
using Splat;

namespace MetricsMonitorClient.DataServices.CPU
{
    public class CPUFactory : ICPUFactory {
        private readonly ILog _logger;
        public CPUFactory(ILog logger) {
            this._logger = logger;
        }
    }
    }
