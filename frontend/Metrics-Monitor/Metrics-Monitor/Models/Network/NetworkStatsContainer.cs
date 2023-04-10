using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetricsMonitorClient.Models.Network {
    public  class NetworkStatsContainer : StatsContainerBase {
        public NetworkStatsContainer(string? interfaceName = null) {
            PollList = new List<double>();
            if(interfaceName == null ) { return; }
            Name = interfaceName;
        }
        public string Name { get; set; }
        public string Status { get; set; }


        protected override void NotifyUi() {
            this.RaisePropertyChanged(nameof(Status));
            base.NotifyUi();
        }
    }
}
