using Avalonia.Collections;
using log4net;
using MetricsMonitorClient.DataServices.MonitorSystem;
using MetricsMonitorClient.DataServices.MonitorSystem.Dtos;
using MetricsMonitorClient.DataServices.Process;
using MetricsMonitorClient.DataServices.Process.Dtos;
using MetricsMonitorClient.Models.Overview;
using MetricsMonitorClient.Models.Process;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MetricsMonitorClient.ViewModels
{
    public class HomeViewModel : ViewModelBase {
        private IMonitorSystemFactory _factory;
        private readonly ILog _logger;
        private readonly SemaphoreSlim _clockLock;

        #region Constructor
        public HomeViewModel(ILog logger, IMonitorSystemFactory factory) {
            _factory = factory;
            _logger = logger;
            _clockLock = new SemaphoreSlim(1, 1);
            this.PropertyChanged += HomeViewModel_PropertyChanged;
            InitCharts();
            ProcessDataRows = new List<ProcessPollSlim>();
        }
        #endregion Constructor
        
        #region Change Handling
        private void HomeViewModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e) {
            if(string.Equals(e.PropertyName, nameof(ClockCycle))){
                UpdateData();
            }
        }
        #endregion Change Handling
       
        #region Properties
        private ChartContainer _cpuChart;
        public ChartContainer CpuChart {
            get { return _cpuChart; }
            set { this.RaiseAndSetIfChanged(ref _cpuChart, value); }
        }

        private ChartContainer _networkChart;
        public ChartContainer NetworkChart {
            get { return _networkChart; }
            set { this.RaiseAndSetIfChanged(ref _networkChart, value); }
        }

        private ChartContainer _memoryChart;
        public ChartContainer MemoryChart {
            get { return _memoryChart; }
            set { this.RaiseAndSetIfChanged(ref _memoryChart, value); }
        }
        private StorageGraphContainer _storageData;
        public StorageGraphContainer StorageData {
            get { return _storageData; }
            set { this.RaiseAndSetIfChanged(ref _storageData, value); }

        }
        public List<ProcessPollSlim> ProcessDataRows { get; set; }

        private string _os;
        public string OS {
            get { return _os; }
            set { this.RaiseAndSetIfChanged(ref _os, value); }
        }

        private string _osVersion;
        public string OSVersion {
            get { return _osVersion; }
            set { this.RaiseAndSetIfChanged(ref _osVersion, value); }
        }

        private double _currentPollRate;
        public double CurrentPollRate {
            get { return _currentPollRate; }
            set { this.RaiseAndSetIfChanged(ref _currentPollRate, value); }
        }

        private double _clientCurrentPollRate;
        public double ClientCurrentPollRate {
            get { return _clientCurrentPollRate; }
            set { this.RaiseAndSetIfChanged(ref _clientCurrentPollRate, value); }
        }

        private long _clockCycle;
        public long ClockCycle {
            get { return _clockCycle; }
            set { this.RaiseAndSetIfChanged(ref _clockCycle, value); }
        }
        #endregion Properties
        #region Methods
        public void TickClock() {
            _clockLock.Wait();
            ClockCycle = _clockCycle + 1;
            _clockLock.Release();
        }

        public void UpdateData() {
            try {
                CompositePollDto data = Task.Run(() => _factory.GetAllLatestMetricsAsync()).Result;
                if (data == null) { return; }

                var cpuVal = data?.cpu.cpu_percent ?? 0.0;
                CpuChart.Update(cpuVal);

                //take the fastest network speed
                //TODO: add a title so you know which one it is
                var netVal = data?.network?.Where(n => n.network_status == MMConstants.NetworkStatus_Up).OrderByDescending(n => n.network_speed).FirstOrDefault().network_speed ?? 0.0;
                netVal = netVal > 0? netVal / 1000 : 0.0;
                NetworkChart.Update(netVal);

                var memVal = data?.memory?.percentage_used ?? 0;
                MemoryChart.Update(memVal);

                StorageData.Update(data?.disk);


                IEnumerable<ProcessPollSlim> procSelection = GetProcessSummaryList(data?.processes);
                ProcessDataRows.Clear();
                ProcessDataRows.AddRange(procSelection);
                this.RaisePropertyChanged(nameof(ProcessDataRows));

                UpdateSystemData(); 

            }catch(Exception ex) {
                _logger.Error(ex);
                Alert("An error occurred refreshing the Overview screen.");

            }
        }

        public void UpdateSystemData() {
            try {
                PollDTO data = Task.Run(() => _factory.GetLatestServiceInfoAsync()).Result;
                if (data == null) { return; }
                OS = data.operating_system;
                OSVersion = data.operating_system_version;
                CurrentPollRate = (data.poll_rate * 60);
            }catch(Exception ex) {
                _logger.Error(ex);
                Alert("An error occurred refreshing information about the host computer.");
            }
        }

        /// <summary>
        /// Returns summarized records of active processes. Processes of the same name are aggregated and their cpu and memory usage values are combined.
        /// </summary>
        /// <param name="data">Discrete processeses running on the host computer</param>
        /// <returns>A set of the most resource demanding process records containing n objects where n = <b>MMConstants.PollBufferSize</b> </returns>
        public static IEnumerable<ProcessPollSlim> GetProcessSummaryList(IEnumerable<ProcessDto> data) {
            if(data == null || !data.Any()) { return new List<ProcessPollSlim>(); }
          
            var procs = data.Select(p => p.ToModel()).ToList();

            // group processes by their names. For each grouping, take the name of the process and the sums of the CpuPercent values and MemoryUsage values
            var slimProcs = procs.GroupBy(p => p.ProcessName).Select(g => new ProcessPollSlim {
                ProcessName = g.Key,
                CpuUsagePctTotal = g.Select(gp => gp.CpuPercent).Sum(),
                MemoryUsagePctTotal = g.Select(gp => gp.MemoryUsage).Sum(),
                ProcessCount = g.Count()
            }).ToList();


            var procSelection = slimProcs.OrderByDescending(sp => sp.CpuUsagePctTotal).Take(MMConstants.PollBufferSize);
            return procSelection;
        }

        public void InitCharts() {
            CpuChart = new ChartContainer("CPU Usage", "Percent Usage", "Time", 100);
            NetworkChart = new ChartContainer("Network Usage (MB/s)", "Usage (MB/s)", "Time", 1000);
            MemoryChart = new ChartContainer("Memory Usage", "Percent Usage", "Time", 100);
            StorageData = new StorageGraphContainer();
        }
        #endregion Methods
    }
}
