using log4net;
using MetricsMonitorClient.DataServices.MonitorSystem;
using ReactiveUI;
using Splat;
using System;
using System.Globalization;
using System.Threading.Tasks;
using System.Timers;
using ResourceTabIndex = MetricsMonitorClient.MMConstants.ResourceTabIndex;
using Timer = System.Timers.Timer;

namespace MetricsMonitorClient.ViewModels {
    public class MainWindowViewModel : ViewModelBase {
        private readonly ILog _logger;
        private readonly IMonitorSystemFactory _factory;
        #region Constructor
        public MainWindowViewModel() {

            CPUViewModel =  WorkspaceFactory.CreateWorkspace<CPUViewModel>();
            MemoryViewModel = WorkspaceFactory.CreateWorkspace<MemoryViewModel>();
            StorageViewModel = WorkspaceFactory.CreateWorkspace<StorageViewModel>();
            HomeViewModel = WorkspaceFactory.CreateWorkspace<HomeViewModel>();
            NetworkViewModel = WorkspaceFactory.CreateWorkspace<NetworkViewModel>();
            ProcessViewModel = WorkspaceFactory.CreateWorkspace<ProcessViewModel>();
           
            ResourceText = "Overview";
          
            StartPolling();
            HomeViewModel.ClientCurrentPollRate = SystemClockInterval;
            
            _logger =  log4net.LogManager.GetLogger(typeof(MainWindowViewModel));
            _factory = Locator.Current.GetService<IMonitorSystemFactory>();
        }

        #endregion Constructor
        #region Properties

        private long _clockCycle;
        public long ClockCycle {
            get { return _clockCycle; }
            private set { this.RaiseAndSetIfChanged(ref _clockCycle, value); }
        }
        public double SystemClockInterval { get; private set; } 

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

        private double _pollRate;
        public double PollRate {
            get { return _pollRate; }
            set {
                this.RaiseAndSetIfChanged(ref _pollRate, value);
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
        private readonly object _cycleLock = new object();
        private void RunClock(object sender, ElapsedEventArgs e) {
            try {

                lock (_cycleLock) {
                if (ClockEnabled == false) { return; }

                if ((sender is Timer) == false) { return; }
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

                }
            }catch (Exception ex) {
                Error("An uncaught error occured within Metrics Monitor.", ex);
                _logger.Error(ex);
               
            }
        }

        private bool _clockEnabled;
        public bool ClockEnabled {
         get { return _clockEnabled; }
         set { this.RaiseAndSetIfChanged(ref _clockEnabled, value); }
        }
        
        private readonly object pollingLock = new object(); 
           public void TogglePolling() {
            lock(pollingLock) {
                if (ClockEnabled) {
                    uiClock.Stop();
                    ClockEnabled = false;
                } else {
                    uiClock.Start();
                    ClockEnabled = true;
                }
            }
        }

        public void SetPollRate() {
            lock (pollingLock) {
                try {
                    if (PollRate < .5) {
                        Alert("Poll rate must be at least .5");
                        return;
                    }

                    var updateSuccessful = Task.Run(() => _factory.SetPollRate(PollRate)).Result;

                    if (updateSuccessful) {
                        SystemClockInterval = PollRate;

                    } else {
                        Alert("An error occurred while attempting to update the poll rate", "Poll Adjustment Error");
                    }

                    RestartPolling();
                } catch (Exception ex) {
                    Error("An error occurred while attempting to update the poll rate. Resetting to Default rate.", "Poll Adjustment Error", ex);
                    _logger.Error(ex);
                    RestartPolling(true);
                }
            }
        }
    
        /// <summary>
        /// Restarts the polling interval timer and sets the interval to the value of the SystemClockInterval property.
        /// </summary>
        /// <param name="defaultRate">if <code>defaultRate == true</code>, the default polling rate will be used after restarting</param>
        private void RestartPolling(bool defaultRate = false) {
            
            if (ClockEnabled) {
                uiClock.Stop();
                ClockEnabled = false;
            }

            uiClock.Elapsed -= RunClock;
        
            SystemClockInterval = defaultRate ? MMConstants.DefaultSystemClockInterval : SystemClockInterval;
            
            uiClock = new Timer(SystemClockInterval);
            uiClock.Elapsed += RunClock;
            uiClock.Start();
            ClockEnabled = true;
        }



        private void StartPolling() {
            SystemClockInterval = MMConstants.DefaultSystemClockInterval;
            
            uiClock = new Timer(SystemClockInterval);
            uiClock.Elapsed += RunClock;
            uiClock.Start();
            ClockEnabled = true;
        }

        #endregion

    }

}
