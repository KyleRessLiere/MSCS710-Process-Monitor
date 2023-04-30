using Avalonia.Collections;
using Avalonia.Controls;
using Avalonia.Controls.Models.TreeDataGrid;
using DynamicData;
using log4net;
using MetricsMonitorClient.DataServices.Process;
using MetricsMonitorClient.Models.Process;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MetricsMonitorClient.ViewModels
{
    public class ProcessViewModel : ViewModelBase {
        private readonly IProcessFactory _factory;
        private readonly ILog _logger;
        private readonly SemaphoreSlim _clockLock;

        #region Constructor
        public ProcessViewModel(IProcessFactory factory, ILog logger) {
            _factory = factory;
            _logger = logger;
            _clockLock = new SemaphoreSlim(1, 1);
            ProcessDataRows = new AvaloniaList<ProcessPoll>();
            this.PropertyChanged += ProcessViewModel_PropertyChanged;
        }
        #endregion Constructor

        #region Change Handling
        private void ProcessViewModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e) {
            if (string.Equals(e.PropertyName, nameof(ClockCycle))) {
                UpdateUiData();
            }
        }
        #endregion Change Handling

        #region Properties
        public HierarchicalTreeDataGridSource<ProcessPoll> TreeGridDataSource { get; set; }
        public AvaloniaList<ProcessPoll> ProcessDataRows { get; set; }
        public bool IsInitialized { get; set; }
        
        private long _clockCycle;
        public long ClockCycle {
            get { return _clockCycle; }
            set { this.RaiseAndSetIfChanged(ref _clockCycle, value); }
        }
        #endregion Properties

        #region Methods
        public void TickClock() {
            _clockLock.Wait();
            ClockCycle = ClockCycle + 1;
            _clockLock.Release();
        }

        public void UpdateUiData() {
            try {
                var procSet = Task.Run(() => _factory.GetRunningProcessesAsync()).Result;
                if(procSet == null || !procSet.Any()) { return; }

                UpdateDataSets(procSet);

                if (!IsInitialized) {
                    SetupTreeGridSource();
                    IsInitialized = true;
                    return;
                }

            } catch (Exception ex) {
                _logger.Error(ex);
            }
        }
        /// <summary>
        /// Loads data from a set of processes into the grid data set
        /// </summary>
        /// <param name="procList">list of process DTOs returned from the service</param>
        public void UpdateDataSets(List<ProcessPoll> procList) {
            try {
                if (procList == null || !procList.Any()) { return; };

            var treeItems = new List<ProcessPoll>();
            var treeItemMap = new Dictionary<string, int>();
                foreach (var proc in procList) {
                int idx;
                    if (treeItemMap.TryGetValue(proc.ProcessName, out idx) == false) {
                    var tHeader = new ProcessPoll(proc);
                    tHeader.ProcessName = proc.ProcessName;
                    treeItems.Add(tHeader);
                    idx = treeItems.Count() - 1;
                    treeItemMap.Add(proc.ProcessName, idx);
                    continue;
                }
                treeItems[idx].Processes.Add(proc);
                treeItems[idx].CpuUsagePctTotal = treeItems[idx].Processes.Sum(p => p.CpuPercent);
                treeItems[idx].MemoryUsagePctTotal = treeItems[idx].Processes.Sum(p => p.MemoryUsage);
            }
            if(treeItems == null || !treeItems.Any()) { return; };
            ProcessDataRows.Clear();
            ProcessDataRows.AddRange(treeItems.ToArray());
            }catch(Exception ex) {
                _logger.Error(ex);
            }
           
        }

        /// <summary>
        /// Initializes the structure of the treelist grid
        /// </summary>
        public void SetupTreeGridSource() {
            TreeGridDataSource = new HierarchicalTreeDataGridSource<ProcessPoll>(ProcessDataRows) {
                Columns = {
                    new HierarchicalExpanderColumn<ProcessPoll>(
                        new TextColumn<ProcessPoll, string>("Process Name", p => p.ProcessName),
                        p => p.Processes),
                    new TextColumn<ProcessPoll, double>("CPU Usage Total",
                        p => p.CpuUsagePctTotal),
                    new TextColumn<ProcessPoll, double>("Memory Usage Total",
                        p => p.MemoryUsagePctTotal),
                    new TextColumn<ProcessPoll, double>("CPU Usage", p => p.CpuPercent),
                    new TextColumn<ProcessPoll, double>("Memory Usage", p => p.MemoryUsage),
                    new TextColumn<ProcessPoll, int>("Thread Number", p => p.ThreadNumber),
                    new TextColumn<ProcessPoll, string>("Status ", p => p.ProcessStatusText)
                },
            };
            this.RaisePropertyChanged(nameof(TreeGridDataSource));
        }

        #endregion Methods
    }
}
