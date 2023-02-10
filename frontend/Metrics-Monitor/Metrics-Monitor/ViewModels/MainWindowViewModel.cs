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
            //CPUViewModel = new CPUViewModel();
            SetToDefaultView();
        }
        #endregion Constructor
        #region Commands
        public ReactiveCommand<string, Unit> SetActiveTabCommand =>  ReactiveCommand.Create<string>(SetActiveTab);
        #endregion Commands
        #region Fields
        public string HomeScreenName => MMConstants.Home_Screen_Name;
        public string CPUTabName => MMConstants.CPU_Tab_Name;
        public string MemoryTabName => MMConstants.Memory_Tab_Name;
        public string StorageTabName => MMConstants.Storage_Tab_Name;
        public string NetworkTabName => MMConstants.Network_Tab_Name;
        #endregion Fields
        #region Properties

        CPUViewModel CPUViewModel { get; set; }
        MemoryViewModel MemoryViewModel { get; set; }
        StorageViewModel StorageViewModel { get; set; }

        private bool isHomeViewActive;
        public bool IsHomeViewActive {
            get { return isHomeViewActive; }
            set { this.RaiseAndSetIfChanged(ref isHomeViewActive, value); }
        }

        private bool isCPUViewActive;
        public bool IsCPUViewActive {
            get { return isCPUViewActive; }
            set { this.RaiseAndSetIfChanged(ref isCPUViewActive, value); }
        }


        private bool isMemoryViewActive;
        public bool IsMemoryViewActive {
            get { return isMemoryViewActive; }
            set { this.RaiseAndSetIfChanged(ref isMemoryViewActive, value); }
        }

        private bool isStorageViewActive;
        public bool IsStorageViewActive {
            get { return isStorageViewActive; }
            set { this.RaiseAndSetIfChanged(ref isStorageViewActive, value); }
        }

        private bool isNetworkViewActive;
        public bool IsNetworkViewActive {
            get { return isNetworkViewActive; }
            set { this.RaiseAndSetIfChanged(ref isNetworkViewActive, value); }
        }

        private string titleText;
        public string TitleText {
            get { return titleText; }
            set { this.RaiseAndSetIfChanged(ref titleText, value); }
        }
        #endregion Properties
        #region Methods
        public void SetActiveTab(string tabName) {
            if ((tabName == null) || (tabName is string) == false) { return; }

            SetToDefaultView();

            switch (tabName) {
                case MMConstants.CPU_Tab_Name:
                    TitleText = "CPU Metrics";
                    IsCPUViewActive = true;
                    break;
                case MMConstants.Memory_Tab_Name:
                    TitleText = "Memory Metrics";
                    IsMemoryViewActive = true;
                    break;
                case MMConstants.Storage_Tab_Name:
                    TitleText = "Storage Metrics";
                    IsStorageViewActive = true;
                    break;
                case MMConstants.Network_Tab_Name:
                    TitleText = "Network Metrics";
                    IsNetworkViewActive = true;
                    break;
                case MMConstants.Home_Screen_Name:
                    TitleText = "System Overview";
                    IsHomeViewActive = true;
                    break;
                default:
                    break;
            }

        }
        public void SetToDefaultView() {
            TitleText = "System Overview";
            IsCPUViewActive = false;
            IsMemoryViewActive = false;
            IsStorageViewActive = false;
            IsNetworkViewActive = false;
            IsHomeViewActive = true;
        }
        #endregion Methods













    }
}
