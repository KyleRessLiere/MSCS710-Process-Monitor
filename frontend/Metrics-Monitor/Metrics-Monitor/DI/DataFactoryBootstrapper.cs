using log4net;
using MetricsMonitorClient.DataServices.CPU;
using MetricsMonitorClient.DataServices.Memory;
using MetricsMonitorClient.DataServices.MonitorSystem;
using Splat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MetricsMonitorClient.DI
{
    public class DataFactoryBootstrapper {
        
        public DataFactoryBootstrapper() {
            Locator.CurrentMutable.Register(() => new DebugLogger(), typeof(ILogger));
            Locator.CurrentMutable.RegisterLazySingleton(() => ObjectFactory.CreateObject<ICPUDataFactory>(), typeof(ICPUDataFactory));
            Locator.CurrentMutable.RegisterLazySingleton(() => ObjectFactory.CreateObject<IMonitorSystemFactory>(), typeof(IMonitorSystemFactory));
            Locator.CurrentMutable.RegisterLazySingleton(() => ObjectFactory.CreateObject<IMemoryFactory>(), typeof(IMemoryFactory));
        }
    }
}
