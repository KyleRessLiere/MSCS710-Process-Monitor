using Microsoft.VisualBasic;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Reactive;
using System.Text;

namespace MetricsMonitorClient.ViewModels {
    public class MainWindowViewModel : ViewModelBase {
        public string Greeting => "Welcome to Avalonia!";
        private Services.ICPUDataFactory _cpuData;
        #region Constructor
        public MainWindowViewModel() {
            CPUViewModel = new CPUViewModel();
            MemoryViewModel = new MemoryViewModel();
            StorageViewModel = new StorageViewModel();
            HomeViewModel = new HomeViewModel();
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
