using Avalonia.Collections;
using MetricsMonitorClient.DataServices.MonitorSystem;
using MetricsMonitorClient.DataServices.MonitorSystem.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetricsMonitorClient.ViewModels {
    public class HomeViewModel : ViewModelBase {
        private readonly IMonitorSystemFactory _factory;
        #region Constructor
        public HomeViewModel(IMonitorSystemFactory factory) {
            _factory = factory;
            Polls = new AvaloniaList<PollDTO>();
            Load();
        }

        #endregion Constructor
        #region Properties
        public AvaloniaList<PollDTO> Polls { get; private set; }
        #endregion Properties
        #region Methods
        public void Load() {
          //var polls =   _factory.GetAllRecords();
          //  if (!polls.Any()) { return; }
          //  Polls.Clear();
          //  Polls.AddRange(polls);
        }
        #endregion Methods

    }
}
