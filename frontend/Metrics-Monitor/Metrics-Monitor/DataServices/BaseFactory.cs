using Castle.Core.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetricsMonitorClient.DataServices {
    public class BaseFactory {
        protected readonly ILogger _log;

        public BaseFactory(ILogger log) { }

    }
}
