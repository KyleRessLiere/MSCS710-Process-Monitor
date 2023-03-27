using Avalonia.Threading;
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
        public string Greeting => "Welcome to Avalonia!";
        #region Constructor
        public MainWindowViewModel() {
            CPUViewModel =  WorkspaceFactory.CreateWorkspace<CPUViewModel>();
            MemoryViewModel = WorkspaceFactory.CreateWorkspace<MemoryViewModel>();
            StorageViewModel = WorkspaceFactory.CreateWorkspace<StorageViewModel>();
            HomeViewModel = WorkspaceFactory.CreateWorkspace<HomeViewModel>();
            ResourceText = "Overview";
            uiClock = new Timer(MMConstants.SystemClockInterval);
            uiClock.Elapsed += RunClock;
            uiClock.Start();
            SingleCycleLock = new SemaphoreSlim(1, 1);

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
                default:
                    ResourceText= string.Empty;
                    break;
            }

        }
        #endregion Methods
        #region System

        private static readonly object _lock = new object();
        private SemaphoreSlim SingleCycleLock;

        private void RunClock(object sender, ElapsedEventArgs e) {
            if((sender is Timer) == false) { return; }
            SingleCycleLock.Wait();

                switch ((ResourceTabIndex)selecetedResourceIndex) {
                    case ResourceTabIndex.Overview:
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
                        break;
                    default:
                        break;
            }
            SingleCycleLock.Release(1);

        }

    



        #endregion
    }
}
