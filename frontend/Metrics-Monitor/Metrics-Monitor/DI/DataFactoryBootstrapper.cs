using MetricsMonitorClient.DataServices.CPU;
using MetricsMonitorClient.DataServices.MonitorSystem;
using Splat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetricsMonitorClient.DI
{
    public class DataFactoryBootstrapper {
        
        public DataFactoryBootstrapper() {
            Locator.CurrentMutable.RegisterLazySingleton(() => new CPUDataFactory(), typeof(ICPUDataFactory));
            Locator.CurrentMutable.RegisterLazySingleton(() => new MonitorSystemFactory(), typeof(IMonitorSystemFactory));
        }
    }
}
