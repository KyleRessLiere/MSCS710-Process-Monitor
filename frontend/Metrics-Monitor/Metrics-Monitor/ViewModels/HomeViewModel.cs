using Avalonia.Collections;
using MetricsMonitorClient.DataServices.CPU.Dtos;
using MetricsMonitorClient.DataServices.Memory.Dtos;
using MetricsMonitorClient.DataServices.MonitorSystem;
using MetricsMonitorClient.DataServices.MonitorSystem.Dtos;
using MetricsMonitorClient.DataServices.Network.Dtos;
using MetricsMonitorClient.DataServices.Storage.Dtos;
using ReactiveUI;
using System.Threading;
using System.Threading.Tasks;

namespace MetricsMonitorClient.ViewModels {
    public class HomeViewModel : ViewModelBase {
        private IMonitorSystemFactory _factory;
        private readonly SemaphoreSlim _clockLock;

        #region Constructor
        public HomeViewModel(IMonitorSystemFactory factory) {
            _factory = factory;
            _clockLock = new SemaphoreSlim(1, 1);
            this.PropertyChanged += HomeViewModel_PropertyChanged;
        }

        private void HomeViewModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e) {
            if(string.Equals(e.PropertyName, nameof(ClockCycle))){
                UpdateData();
            }
        }
        #endregion Constructor
        #region Properties
        AvalonaList<CPUDto> CPUPolls { get; set; }  // graph the sum of all cores
        AvalonaList<MemoryUsagePollDto> MemoryPolls { get; set; }   //graph usage
        AvalonaList<ProcessDTO> ProcessPolls { get; set; }  //flat table with top 10 processes, group on proc name, comma seperate their PIDs
        AvaloniaList<StorageDto> StoragePolls { get; set; } // .25 size of the actual screen
        AvaloniaList<NetworkDto> NetworkPolls { get; set; } //dropdown selector for which usage to show on graph

        private long _clockCycle;
        public long ClockCycle {
            get { return _clockCycle; }
            set { this.RaiseAndSetIfChanged(ref _clockCycle, value); }
        }
        //TODO: use IMonitorFactory to get the actual polls, and just show the static data




        #endregion Properties
        #region Methods

        //init graphs




        //on clock tick, make each call asynchronously so the whole thing doesnt jam up





        public void TickClock() {
            _clockLock.Wait();
            ClockCycle = _clockCycle + 1;
            _clockLock.Release();
        }



        public void UpdateData() {
            CompositePollDto data = Task.Run(() => _factory.GetAllLatestMetricsAsync()).Result;

        }


        





        #endregion Methods

    }
}
