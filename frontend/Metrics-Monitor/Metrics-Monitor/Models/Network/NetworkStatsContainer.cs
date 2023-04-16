using MetricsMonitorClient.DataServices.Network;
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

        private int _statusId;
        public int StatusId {
            get { return _statusId; }
            set { 
                _statusId = value;
                Status = NetworkMapper.NetStatIdToName(value);
            }
        }
        public string Status { get; set; }


        protected override void NotifyUi() {
            this.RaisePropertyChanged(nameof(Status));
            base.NotifyUi();
        }
    }
}
