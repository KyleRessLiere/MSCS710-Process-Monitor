using Avalonia.Collections;
using JetBrains.Annotations;
using log4net;
using MetricsMonitorClient.DataServices.Network;
using MetricsMonitorClient.Models.Network;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
            InterfaceStatsIndexDict = new Dictionary<string, int>();
            _clockLock = new SemaphoreSlim(1, 1);
            this.PropertyChanged += NetworkViewModel_PropertyChanged;
        }

        private void NetworkViewModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e) {
            if (string.Equals(e.PropertyName, nameof(ClockCycle))) {
                UpdateUiData();
            }
        }

        public AvaloniaList<NetworkStatsContainer> StatsContainers { get; private set; }
        public bool IsInitialized { get; set; }
        public int InterfaceCount { get; set; }

        public Dictionary<string, int> InterfaceStatsIndexDict {get; set;}

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

        public void InitData(List<NetworkPoll> pollSet) {
            InterfaceCount = pollSet.Count();
            StatsContainers = new AvaloniaList<NetworkStatsContainer>();

            for(int i = 0; i < InterfaceCount; i++) {
                var container = new NetworkStatsContainer(pollSet[i].Interface);
                container.AddAndUpdate(pollSet[i].Speed);
                container.StatusId = pollSet[i].Status;
                StatsContainers.Add(container);
                InterfaceStatsIndexDict.Add(pollSet[i].Interface, i);
            }
            this.RaisePropertyChanged(nameof(StatsContainers));
        }


        public void UpdateDataSets(List<NetworkPoll> pollSet) {
            var pollSetInterfaces = pollSet.Select(p => p.Interface).ToHashSet();
            var missingInterfaces = InterfaceStatsIndexDict.Keys.Where(ifc => !pollSetInterfaces.Contains(ifc)).ToList();
            
            for (int i = 0; i < pollSet.Count; i++) {
                var idx = InterfaceStatsIndexDict.TryGetValue(pollSet[i].Interface, out var intIdx) ? intIdx : -1;
                if (idx == -1) { continue; }
                StatsContainers[idx].AddAndUpdate(pollSet[i].Speed);
                StatsContainers[idx].StatusId = pollSet[i].Status;
            }

            for(int j = 0;j <  missingInterfaces.Count; j++) {
                var idx = InterfaceStatsIndexDict.TryGetValue(missingInterfaces[j], out var intIdx) ? intIdx : -1;
                if (idx == -1) { continue; }
                StatsContainers[idx].AddAndUpdate(0.0);
                StatsContainers[idx].StatusId = MMConstants.NetworkStatus_Unknown_Id;
            }

            this.RaisePropertyChanged(nameof(StatsContainers));
        }



        public NetworkPoll GetPlaceholderPollForInterface(string interfaceName, int pollId, int networkId) {
            if(interfaceName == null || pollId < 0 || networkId < 0) { return null; }
            return new NetworkPoll { Id = networkId, Interface = interfaceName, Speed = 0, Status = MMConstants.NetworkStatus_Unknown_Id, PollId = pollId };
        }
        
    }
}
