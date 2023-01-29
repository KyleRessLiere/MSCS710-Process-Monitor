using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetricsMonitor.ViewModels {
    public class MainWindowViewModel : BindableBase {
        private Services.ICPUDataFactory _cpuData;
        #region Constructor
        public MainWindowViewModel(Services.ICPUDataFactory cpuData) {
            _cpuData = cpuData;
            //CPUViewModel = new CPUViewModel();
            SetToDefaultView();
        }
        #endregion Constructor
        #region Commands
        public DelegateCommand<string> SetActiveTabCommand => new DelegateCommand<string>((tabName) => SetActiveTab(tabName));
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
        NetworkViewModel NetworkViewModel { get; set; }
        StorageViewModel StorageViewModel { get; set; }

        private bool isHomeViewActive;
        public bool IsHomeViewActive {
            get { return isHomeViewActive; }
            set { SetProperty(ref isHomeViewActive, value, nameof(IsHomeViewActive)); }
        }

        private bool isCPUViewActive;
        public bool IsCPUViewActive {
            get { return isCPUViewActive; }
            set { SetProperty(ref isCPUViewActive, value, nameof(IsCPUViewActive)); }
        }

        private bool isMemoryViewActive;
        public bool IsMemoryViewActive {
            get { return isMemoryViewActive; }
            set { SetProperty(ref isMemoryViewActive, value, nameof(IsMemoryViewActive)); }
        }

        private bool isStorageViewActive;
        public bool IsStorageViewActive {
            get { return isStorageViewActive; }
            set { SetProperty(ref isStorageViewActive, value, nameof(IsStorageViewActive)); }
        }

        private bool isNetworkViewActive;
        public bool IsNetworkViewActive {
            get { return isNetworkViewActive; }
            set { SetProperty(ref isNetworkViewActive, value, nameof(IsNetworkViewActive)); }
        }

        private string titleText;
        public string TitleText {
            get { return titleText; }
            set { SetProperty(ref titleText, value, nameof(TitleText)); }
        }
        #endregion Properties
        #region Methods
        public void SetActiveTab(string tabName) {
            if ((tabName == null) ||  (tabName is string) == false) { return; }
            
            SetToDefaultView();

            switch (tabName) {
                case MMConstants.CPU_Tab_Name:
                    TitleText = "CPU Metrics";
                    IsCPUViewActive =  true;
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