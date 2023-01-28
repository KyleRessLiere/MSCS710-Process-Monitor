using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetricsMonitorWPF {
    public class ViewModel {
        public ViewModel(string[] people, Func<object, Task<object>> value1, Func<object, Task<object>> value2) {
            People = people;
            Value1 = value1;
            Value2 = value2;
        }

        public string? FirstName { get; set; }
        public string[] People { get; }
        public Func<object, Task<object>> Value1 { get; }
        public Func<object, Task<object>> Value2 { get; }
    }
}
