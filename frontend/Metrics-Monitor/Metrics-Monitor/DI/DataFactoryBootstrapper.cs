using MetricsMonitorClient.DataServices.CPU;
using MetricsMonitorClient.DataServices.Memory;
using MetricsMonitorClient.DataServices.MonitorSystem;
using Splat;

namespace MetricsMonitorClient.DI {
    public class DataFactoryBootstrapper {
        
        public DataFactoryBootstrapper() {
            Locator.CurrentMutable.Register(() => new DebugLogger(), typeof(ILogger));
            Locator.CurrentMutable.RegisterLazySingleton(() => new CPUDataFactory(), typeof(ICPUDataFactory));
            Locator.CurrentMutable.RegisterLazySingleton(() =>  new MonitorSystemFactory(), typeof(IMonitorSystemFactory));
            Locator.CurrentMutable.RegisterLazySingleton(() => {
                var logger = Locator.Current.GetService<ILogger>();//Splat's DI handling isnt the greatest, so this is needed to ensure that dependencies are resolved in the correct order
                return new MemoryFactory(logger);
                },
                typeof(IMemoryFactory));
        }
    }
}
