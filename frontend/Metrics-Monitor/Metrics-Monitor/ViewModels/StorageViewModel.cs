using Avalonia.Collections;
using JetBrains.Annotations;
using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using log4net;
using MetricsMonitorClient.DataServices.Storage;
using ReactiveUI;
using SkiaSharp;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Globalization;

namespace MetricsMonitorClient.ViewModels {
    public class StorageViewModel : ViewModelBase {
        private readonly IStorageFactory _factory;
        private readonly ILog _logger;
        private readonly SemaphoreSlim _clockLock;

        public StorageViewModel(IStorageFactory factory, ILog logger) {
            this._factory = factory;
            this._logger = logger;
            _clockLock = new SemaphoreSlim(1, 1);
            DiskFree = new ObservableValue();
            DiskUsed = new ObservableValue();
            InitChart();
            this.PropertyChanged += StorageViewModel_PropertyChanged;
        }

        private void StorageViewModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e) {
            if(e.PropertyName == nameof(Clock)) {
                Refresh();
            }
        }

        #region Properties

        private long _clock;
        public long Clock {
            get { return _clock; }
            set { this.RaiseAndSetIfChanged(ref _clock, value); }
        }

        private string _diskTotalInfo;
        public string DiskTotalInfo {
            get { return _diskTotalInfo; }
            set { this.RaiseAndSetIfChanged(ref _diskTotalInfo, value); }
        }

        private string _diskUsageInfo;
        public string DiskUsageInfo {
            get { return _diskUsageInfo; }
            set { this.RaiseAndSetIfChanged(ref _diskUsageInfo, value); }
        }


        private double _diskPercentage;
        public double DiskPercentage {
            get { return _diskPercentage; }
            set { this.RaiseAndSetIfChanged(ref _diskPercentage, value); }
        }

        private ObservableValue _diskFree;
        public ObservableValue DiskFree {
            get { return _diskFree; }
            set { this.RaiseAndSetIfChanged(ref _diskFree, value); }
        }
        private double _diskTotal;
        public double DiskTotal {
            get { return _diskTotal; }
            set { this.RaiseAndSetIfChanged(ref _diskTotal, value); }
        }
        private ObservableValue _diskUsed;
        public ObservableValue DiskUsed {
            get { return _diskUsed; }
            set { this.RaiseAndSetIfChanged(ref _diskUsed, value); }
        }

        private AvaloniaList<ISeries> _storageUsagePieChart;
        public AvaloniaList<ISeries> StorageUsagePieChart {
            get { return _storageUsagePieChart; }
            set { this.RaiseAndSetIfChanged(ref _storageUsagePieChart, value); }
        }


        #endregion Properties


        #region Methods
        public void Refresh() {
            try {
                var poll = Task.Run(() => _factory.GetLatestStoragePollAsync()).Result;
                if (poll == null) { return; }

                DiskPercentage = poll.disk_percentage;
                DiskTotal = (double)poll.disk_total / MMConstants.OneBillion;
                DiskFree.Value = (double)poll.disk_free / MMConstants.OneBillion; // converting to gb
                DiskUsed.Value = (double)poll.disk_used / MMConstants.OneBillion;

                DiskTotalInfo = $"Total Space: {DiskTotal.ToString("N7", CultureInfo.InvariantCulture)} Gb";
                DiskUsageInfo = $"Percentage Used: {DiskPercentage.ToString("N7", CultureInfo.InvariantCulture)}%";

            }catch(Exception ex) {
                _logger.Error(ex);
            }
        }

        public void TickClock() {
            _clockLock.Wait();
            Clock = Clock + 1;
            _clockLock.Release();
        }


        public void InitChart() {
            DiskFree.Value = 100;
            StorageUsagePieChart = new AvaloniaList<ISeries> {
                new PieSeries<ObservableValue> { Values = new ObservableValue[] { DiskFree }, Name = "Free Space (Gb)", InnerRadius = 50 },
                new PieSeries<ObservableValue> { Values = new ObservableValue[] { DiskUsed }, Name = "Used Space (Gb)", InnerRadius = 50 }
            };
        }

        #endregion Methods
      
    }
}
