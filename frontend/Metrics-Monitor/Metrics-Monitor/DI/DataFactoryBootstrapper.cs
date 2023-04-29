using Castle.Core.Logging;
using Castle.Services.Logging.Log4netIntegration;
using log4net;
using MetricsMonitorClient.DataServices.CPU;
using MetricsMonitorClient.DataServices.Memory;
using MetricsMonitorClient.DataServices.MonitorSystem;
using MetricsMonitorClient.DataServices.Network;
using MetricsMonitorClient.DataServices.Process;
using MetricsMonitorClient.DataServices.Storage;
using Splat;

namespace MetricsMonitorClient.DI {
    public class DataFactoryBootstrapper {
        public DataFactoryBootstrapper() {

            //Splat's DI handling isnt the greatest, so this is needed to ensure that dependencies are resolved in the correct order
            Locator.CurrentMutable.RegisterLazySingleton(() => {
                var logger = log4net.LogManager.GetLogger(typeof(ICPUFactory));
                return new CPUFactory(logger);
            },typeof(ICPUFactory));
         
            Locator.CurrentMutable.RegisterLazySingleton(() => {
                var logger = log4net.LogManager.GetLogger(typeof(IMemoryFactory));
                return new MemoryFactory(logger);
                },
                typeof(IMemoryFactory));
           
            Locator.CurrentMutable.RegisterLazySingleton(() => {
                var logger = log4net.LogManager.GetLogger(typeof(IStorageFactory));
                return new StorageFactory(logger);
            }, typeof(IStorageFactory));
         
            Locator.CurrentMutable.RegisterLazySingleton(() => {
                var logger = log4net.LogManager.GetLogger(typeof(INetworkFactory));
                return new NetworkFactory(logger);
            },typeof(INetworkFactory));

            Locator.CurrentMutable.RegisterLazySingleton(() => {
                var logger = log4net.LogManager.GetLogger(typeof(IProcessFactory));
                return new ProcessFactory(logger);
            }, typeof(IProcessFactory));

            Locator.CurrentMutable.RegisterLazySingleton(() => {
                var logger = log4net.LogManager.GetLogger(typeof(IMonitorSystemFactory));
                return new MonitorSystemFactory(logger);
            }, typeof(IMonitorSystemFactory));
        }
    }
}
