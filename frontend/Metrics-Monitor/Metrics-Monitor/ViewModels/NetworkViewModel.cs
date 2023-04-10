using Avalonia.Collections;
using JetBrains.Annotations;
using log4net;
using MetricsMonitorClient.DataServices.Network;
using MetricsMonitorClient.DataServices.Network.Dtos;
using MetricsMonitorClient.Models.Network;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MetricsMonitorClient.ViewModels {
    public class NetworkViewModel : ViewModelBase {
        private readonly INetworkFactory _factory;
        private readonly ILog _logger;
        private readonly SemaphoreSlim _clockLock;
        public NetworkViewModel(INetworkFactory factory, ILog logger) {
            _factory = factory;
            _logger = logger;
            _clockLock = new SemaphoreSlim(1, 1);
            this.PropertyChanged += NetworkViewModel_PropertyChanged;
        }

        private void NetworkViewModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e) {
            if(string.Equals(e.PropertyName, nameof(ClockCycle))) {
                UpdateUiData();
            }
        }

        public AvaloniaList<NetworkStatsContainer> StatsContainers { get; private set; }
        public bool IsInitialized { get; set; }
        public int InterfaceCount { get; set; }



        private long _clockCycle;
        public long ClockCycle {
            get { return _clockCycle; }
            set { this.RaiseAndSetIfChanged(ref _clockCycle, value); }
        }

        public void TickClock() {
            _clockLock.Wait();
            ClockCycle = ClockCycle + 1;
            _clockLock.Release();
        }

        public void UpdateUiData() {
            try {
                var pollSet = Task.Run(() => _factory.GetLatestNetworkPollAsync()).Result;
                if(pollSet == null || !pollSet.Any()) { return; }
                var pollList = pollSet.ToList();
                if (!IsInitialized) {
                    InitData(pollList);
                    IsInitialized = true;
                    return;
                }

                UpdateDataSets(pollList);
            }catch(Exception ex) {
                _logger.Error(ex);
                throw;
            }

        }

        public void InitData(List<NetworkDto> pollSet) {
            InterfaceCount = pollSet.Count();
            StatsContainers = new AvaloniaList<NetworkStatsContainer>();
            for(int i = 0; i < InterfaceCount; i++) {
                var container = new NetworkStatsContainer(pollSet[i].network_interface);
                container.AddAndUpdate(pollSet[i].network_speed);
                container.Status = pollSet[i].network_status;
                StatsContainers.Add(container);
            }
            this.RaisePropertyChanged(nameof(StatsContainers));
        }


        public void UpdateDataSets(List<NetworkDto> pollSet) {
            for (int i = 0; i < InterfaceCount; i++) {
                StatsContainers[i].AddAndUpdate(pollSet[i].network_speed);
                StatsContainers[i].Status = pollSet[i].network_status;
            }
            this.RaisePropertyChanged(nameof(StatsContainers));
        }




    }
}
