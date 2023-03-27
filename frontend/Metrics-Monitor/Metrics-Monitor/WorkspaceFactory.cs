using log4net;
using MetricsMonitorClient.DataServices.Memory;
using Splat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;

namespace MetricsMonitorClient {
    public class WorkspaceFactory {

        private static ILog _log;

        public static ILog Log {
            get {
                if( _log == null ) {
                    _log = log4net.LogManager.GetLogger(typeof(WorkspaceFactory));
                }
                return _log;
            }
        }

        private static readonly object _lock = new object();

        /// <summary>
        /// Uses reflection to create classes and inject them with the required dependencies
        /// </summary>
        /// <typeparam name="T">Desired type of returned object</typeparam>
        /// <returns> object of type T</returns>
        public static T CreateWorkspace<T>() {
            try {
                T createdObject;
                lock (_lock) {
                    var constructors = typeof(T).GetConstructors();
                    var constructor = constructors.FirstOrDefault();

                    if (constructor == null) {
                        createdObject = default(T);
                        if (createdObject == null) {
                            throw new NullReferenceException($" an error occurred while attempting to resolve {typeof(T).FullName}");
                        }
                        return createdObject;
                    }


                    Dictionary<int, object?> dependencies = new Dictionary<int, object?>();

                    foreach (var param in constructor.GetParameters()) {

                        if (param.ParameterType == typeof(ILog)) { //logger will not resolve from DI pack
                            var logger = log4net.LogManager.GetLogger(typeof(T));
                            dependencies.Add(param.Position, logger);
                            continue;
                        }

                        object dependency = Locator.Current.GetService(param.ParameterType);
                        dependencies.Add(param.Position, dependency);
                    }

                    var orderedDependencies = dependencies.OrderBy(dependency => dependency.Key).Select(d => d.Value).ToArray();

                    createdObject = (T)constructor.Invoke(orderedDependencies);




                    return (T)createdObject;
                }
           
            }catch(Exception ex) {
                Log.Error(ex);
                throw;
            }
        }


    }
}
