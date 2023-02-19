using MetricsMonitorClient.DataServices.CPU;
using MetricsMonitorClient.DataServices.MonitorSystem;
using Microsoft.VisualBasic;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Reactive;
using System.Text;

namespace MetricsMonitorClient.ViewModels
{
    public class MainWindowViewModel : ViewModelBase {
        public string Greeting => "Welcome to Avalonia!";
        private readonly ICPUDataFactory _cpuDataFactory;
        private readonly IMonitorSystemFactory _monitorSystemFactory;
        #region Constructor
        public MainWindowViewModel(ICPUDataFactory cpuDataFactory, IMonitorSystemFactory monitorSystemFactory) {
            _cpuDataFactory = cpuDataFactory;
            _monitorSystemFactory = monitorSystemFactory;
            CPUViewModel = new CPUViewModel(_cpuDataFactory);
            MemoryViewModel = new MemoryViewModel();
            StorageViewModel = new StorageViewModel();
            HomeViewModel = new HomeViewModel(_monitorSystemFactory);
            ResourceText = "Overview";
        }
        #endregion Constructor
        #region Properties
        CPUViewModel CPUViewModel { get; set; }
        MemoryViewModel MemoryViewModel { get; set; }
        StorageViewModel StorageViewModel { get; set; }
        HomeViewModel HomeViewModel { get; set; }

        private string resourceText;
        public string ResourceText {
            get { return resourceText; }
            set { this.RaiseAndSetIfChanged(ref resourceText, value); }
        }


        private int selecetedResourceIndex;
        public int SelectedResourceIndex {
            get { return selecetedResourceIndex; }
            set { 
                this.RaiseAndSetIfChanged(ref selecetedResourceIndex, value);
                UpdateUI();
            }
        }
        #endregion Properties
        #region Methods
        public void UpdateUI() {
            switch (SelectedResourceIndex) {
                case 0:
                    ResourceText = "Overview";
                    break;
                case 1:
                    ResourceText = "Processing";
                    break;
                case 2:
                    ResourceText = "Memory";
                    break;
                case 3:
                    ResourceText = "Storage";
                    break;
                default:
                    ResourceText= string.Empty;
                    break;
            }

        }
        #endregion Methods
    }
}
