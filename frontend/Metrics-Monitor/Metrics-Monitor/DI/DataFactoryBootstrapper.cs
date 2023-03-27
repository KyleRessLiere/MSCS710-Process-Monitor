using Castle.Core.Logging;
using Castle.Services.Logging.Log4netIntegration;
using log4net;
using MetricsMonitorClient.DataServices.CPU;
using MetricsMonitorClient.DataServices.Memory;
using MetricsMonitorClient.DataServices.MonitorSystem;
using Splat;

namespace MetricsMonitorClient.DI {
    public class DataFactoryBootstrapper {
        
        public DataFactoryBootstrapper() {

            //Splat's DI handling isnt the greatest, so this is needed to ensure that dependencies are resolved in the correct order
            Locator.CurrentMutable.RegisterLazySingleton(() => {
                var logger = log4net.LogManager.GetLogger(typeof(ICPUFactory));
                return new CPUFactory(logger);
            },
                 typeof(ICPUFactory));
            Locator.CurrentMutable.RegisterLazySingleton(() => {
                var logger = log4net.LogManager.GetLogger(typeof(IMemoryFactory));
                return new MemoryFactory(logger);
                },
                typeof(IMemoryFactory));
            Locator.CurrentMutable.RegisterLazySingleton(() => new MonitorSystemFactory(), typeof(IMonitorSystemFactory));
        }
    }
}
