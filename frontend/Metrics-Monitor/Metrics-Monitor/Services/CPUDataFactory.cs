using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetricsMonitorClient.Services
{
    public class CPUDataFactory : ICPUDataFactory {
        private CPUDataFactory _instance;
        public CPUDataFactory Instance {
            get {
                if (_instance == null) {
                    _instance = new CPUDataFactory();
                }
                return _instance;
            }
        }

        public IEnumerable<string> GetAllRecords() {
            return new List<string> { "data1", "data2" };
        }
    }
}
