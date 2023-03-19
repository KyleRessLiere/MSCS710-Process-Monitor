using MetricsMonitorClient.DataServices.CPU;
using MetricsMonitorClient.DataServices.Memory;
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
            Locator.CurrentMutable.Register(() => new DebugLogger(), typeof(ILogger));
            Locator.CurrentMutable.RegisterLazySingleton(() => new CPUDataFactory(), typeof(ICPUDataFactory));
            Locator.CurrentMutable.RegisterLazySingleton(() => new MonitorSystemFactory(), typeof(IMonitorSystemFactory));
            Locator.CurrentMutable.RegisterLazySingleton(() => new MemoryFactory(), typeof(IMemoryFactory));
        }
    }
}
