using Avalonia.Threading;
using log4net;
using MetricsMonitorClient.DataServices.CPU;
using MetricsMonitorClient.DataServices.Memory;
using MetricsMonitorClient.DataServices.MonitorSystem;
using Microsoft.VisualBasic;
using NUnit.Framework.Constraints;
using Org.BouncyCastle.Asn1.Crmf;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Reactive;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using ResourceTabIndex = MetricsMonitorClient.MMConstants.ResourceTabIndex;
using Timer = System.Timers.Timer;

namespace MetricsMonitorClient.ViewModels
{
    public class MainWindowViewModel : ViewModelBase {
        private readonly ILog _logger;
        #region Constructor
        public MainWindowViewModel() {
            CPUViewModel =  WorkspaceFactory.CreateWorkspace<CPUViewModel>();
            MemoryViewModel = WorkspaceFactory.CreateWorkspace<MemoryViewModel>();
            StorageViewModel = WorkspaceFactory.CreateWorkspace<StorageViewModel>();
            HomeViewModel = WorkspaceFactory.CreateWorkspace<HomeViewModel>();
            NetworkViewModel = WorkspaceFactory.CreateWorkspace<NetworkViewModel>();
            ProcessViewModel = WorkspaceFactory.CreateWorkspace<ProcessViewModel>();
            ResourceText = "Overview";
            uiClock = new Timer(MMConstants.SystemClockInterval);
            uiClock.Elapsed += RunClock;
            uiClock.Start();
            SingleCycleLock = new SemaphoreSlim(1, 1);
           _logger =  log4net.LogManager.GetLogger(typeof(MainWindowViewModel));
            ClockEnabled = true;

        }


        #endregion Constructor
        #region Properties

        private long _clockCycle;
        public long ClockCycle {
            get { return _clockCycle; }
            private set { this.RaiseAndSetIfChanged(ref _clockCycle, value); }
        }

        CPUViewModel CPUViewModel { get; set; }
        MemoryViewModel MemoryViewModel { get; set; }
        StorageViewModel StorageViewModel { get; set; }
        ProcessViewModel ProcessViewModel { get; set; }
        NetworkViewModel NetworkViewModel { get; set; }
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


        private Timer uiClock { get; set; }
        #endregion Properties
        #region Methods
        public void UpdateUI() {
            switch ((ResourceTabIndex)SelectedResourceIndex) {
                case ResourceTabIndex.Overview:
                    ResourceText = "Overview";
                    break;
                case ResourceTabIndex.CPU:
                    ResourceText = "CPU";
                    break;
                case ResourceTabIndex.Memory:
                    ResourceText = "Memory";
                    break;
                case ResourceTabIndex.Storage:
                    ResourceText = "Storage";
                    break;
                case ResourceTabIndex.Network:
                    ResourceText = "Network";
                    break;
                case ResourceTabIndex.Processes:
                    ResourceText = "Processes";
                    break;
                default:
                    ResourceText= string.Empty;
                    break;
            }

        }
        #endregion Methods
        #region System
        private SemaphoreSlim SingleCycleLock;
        private void RunClock(object sender, ElapsedEventArgs e) {
            SingleCycleLock.Wait();
            if (ClockEnabled == false) { return; }


            if ((sender is Timer) == false) { return; }
            try {
                switch ((ResourceTabIndex)selecetedResourceIndex) {
                    case ResourceTabIndex.Overview:
                        HomeViewModel.TickClock();
                        Console.WriteLine("Tick " + ClockCycle);
                        break;
                    case ResourceTabIndex.CPU:
                        CPUViewModel.TickClock();
                        Console.WriteLine("Tick " + ClockCycle);
                        break;
                    case ResourceTabIndex.Memory:
                        MemoryViewModel.TickClock();
                        Console.WriteLine("Tick " + ClockCycle);
                        break;
                    case ResourceTabIndex.Storage:
                        StorageViewModel.TickClock();
                        Console.WriteLine("Tick " + ClockCycle);
                        break;
                    case ResourceTabIndex.Network:
                        NetworkViewModel.TickClock();
                        Console.WriteLine("Tick " + ClockCycle);
                        break;
                    case ResourceTabIndex.Processes:
                        ProcessViewModel.TickClock();
                        Console.WriteLine("Tick " + ClockCycle);
                        break;
                    default:
                        break;
                }
                SingleCycleLock.Release(1);
            } catch (Exception ex) {
                _logger.Error(ex);
                //var messageBoxStandardWindow = MessageBox.Avalonia.MessageBoxManager
                //.GetMessageBoxStandardWindow("title", "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed...");
                //messageBoxStandardWindow.Show();
            }
        }
           private bool _clockEnabled;
           public bool ClockEnabled {
            get { return _clockEnabled; }
            set { this.RaiseAndSetIfChanged(ref _clockEnabled, value); }
           }
           public void TogglePolling() {
            if (ClockEnabled) {
                uiClock.Stop();
                ClockEnabled = false;
            } else {
                uiClock.Start();
                ClockEnabled = true;
            }
        }
        #endregion

    }

}
