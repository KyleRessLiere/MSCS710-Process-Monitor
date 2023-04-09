using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using log4net;
using MetricsMonitorClient.DataServices.Storage;
using ReactiveUI;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
namespace MetricsMonitorClient.ViewModels {
    public class StorageViewModel : ViewModelBase {
        private readonly IStorageFactory _factory;
        private readonly ILog _logger;
        private readonly SemaphoreSlim _clockLock;

        public StorageViewModel(IStorageFactory factory, ILog logger) {
            this._factory = factory;
            this._logger = logger;
            _clockLock = new SemaphoreSlim(1, 1);
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

        private double _diskPercentage;
        public double DiskPercentage {
            get { return _diskPercentage; }
            set { this.RaiseAndSetIfChanged(ref _diskPercentage, value); }
        }

        private long _diskFree;
        public long DiskFree {
            get { return _diskFree; }
            set { this.RaiseAndSetIfChanged(ref _diskFree, value); }
        }
        private long _diskTotal;
        public long DiskTotal {
            get { return _diskTotal; }
            set { this.RaiseAndSetIfChanged(ref _diskTotal, value); }
        }
        private long _diskUsed;
        public long DiskUsed {
            get { return _diskUsed; }
            set { this.RaiseAndSetIfChanged(ref _diskUsed, value); }
        }
        
        public IEnumerable<ISeries> StorageUsagePieChart { get; set; }

        #endregion Properties


        #region Methods
        public void Refresh() {
            try {
                var poll = Task.Run(() => _factory.GetLatestStoragePollAsync()).Result;
                if (poll == null) { return; }

                DiskPercentage = poll.disk_percentage;
                DiskFree = poll.disk_free;
                DiskTotal = poll.disk_total;
                DiskUsed = poll.disk_used;
                
                StorageUsagePieChart = new ISeries[]{
                    new PieSeries<long> { Values = new long[] { DiskFree }, Name = "Free Space" },
                    new PieSeries<long> { Values = new long[] { DiskUsed }, Name = "Used Space" },
                };

                this.RaisePropertyChanged(nameof(StorageUsagePieChart));

            }catch(Exception ex) {
                _logger.Error(ex);
            }
        }

        public void TickClock() {
            _clockLock.Wait();
            Clock = Clock + 1;
            _clockLock.Release();
        }
        #endregion Methods
    }
}
