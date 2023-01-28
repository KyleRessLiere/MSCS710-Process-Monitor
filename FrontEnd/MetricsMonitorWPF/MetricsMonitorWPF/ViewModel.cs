using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetricsMonitorWPF {
    public class ViewModel {
        public ViewModel(string name, List<string> things) {
            Name = name;

        }

        public string Name { get; set; }
    }
}
