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

namespace MetricsMonitorClient.ViewModels
{
    public class MainWindowViewModel : ViewModelBase {
        public string Greeting => "Welcome to Avalonia!";
        private readonly ICPUDataFactory _cpuDataFactory;
        private readonly IMonitorSystemFactory _monitorSystemFactory;
        private readonly IMemoryFactory _memoryFactory;
        #region Constructor
        public MainWindowViewModel(ICPUDataFactory cpuDataFactory, IMonitorSystemFactory monitorSystemFactory, IMemoryFactory memoryFactory) {
            _cpuDataFactory = cpuDataFactory;
            _monitorSystemFactory = monitorSystemFactory;
            _memoryFactory = memoryFactory;
            CPUViewModel = new CPUViewModel(_cpuDataFactory);
            MemoryViewModel = new MemoryViewModel(_memoryFactory);
            StorageViewModel = new StorageViewModel();
            HomeViewModel = new HomeViewModel(_monitorSystemFactory);
            StartClock();
            this.PropertyChanged += MainWindowViewModel_PropertyChanged;
            ResourceText = "Overview";
        }


        #endregion Constructor
        #region Properties
        private void MainWindowViewModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e) {
           if(e.PropertyName == nameof(ClockCycle)) {
                Dispatcher.UIThread.InvokeAsync(() =>
                    Parallel.Invoke(() => MemoryViewModel.TickClock(),
                    () => CPUViewModel.TickClock()));
                Console.WriteLine("Tick " + ClockCycle);
           }
        }

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
        #region System

        private bool hasStartedClockCycling;
        private void StartClock() {
            clockLock.Wait();
            if (hasStartedClockCycling) { return; }
            Task.Run(() => RunClock());
            clockLock.Release();
        }
        private SemaphoreSlim clockLock = new SemaphoreSlim(1);
        private void RunClock() {
            
            if (hasStartedClockCycling) { return; }

            hasStartedClockCycling = true;
              
            while (true) {
                ClockCycle++;
                Thread.Sleep(MMConstants.SystemClockInterval);
            }
        }

        
        #endregion
    }
}
