using Avalonia.Collections;
using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using MetricsMonitorClient.DataServices.Storage.Dtos;
using ReactiveUI;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetricsMonitorClient.Models.Overview
{
    /// <summary>
    /// Wrapper class to maintain all data for the storage chart
    /// </summary>
    public class StorageGraphContainer : ReactiveObject
    {
        protected readonly int _bufferSize;
        public StorageGraphContainer()
        {
            Init();
        }
        public AvaloniaList<ISeries> Graph { get; set; }
        public ObservableValue FreeAmount { get; set; }
        public ObservableValue UsedAmount { get; set; }
        public ObservableValue TotalAmount { get; set; }
        public ObservableValue PercentUsed { get; set; }
        public ObservableValue DiskUsed { get; set; }

        private string _diskTotalInfo;
        public string DiskTotalInfo
        {
            get { return _diskTotalInfo; }
            set { this.RaiseAndSetIfChanged(ref _diskTotalInfo, value); }
        }

        private string _diskUsageInfo;
        public string DiskUsageInfo
        {
            get { return _diskUsageInfo; }
            set { this.RaiseAndSetIfChanged(ref _diskUsageInfo, value); }
        }

        public void Update(StorageDto poll)
        {
            if (poll == null) { return; }

            Graph[0].Values = new ObservableValue[] { FreeAmount };
            Graph[1].Values = new ObservableValue[] { UsedAmount };

            FreeAmount.Value = poll.disk_free;
            UsedAmount.Value = poll.disk_used;
            TotalAmount.Value = poll.disk_total;
            PercentUsed.Value = poll.disk_percentage;
            DiskTotalInfo = $"Total Space: {(poll.disk_total / MMConstants.OneBillion).ToString("N2", CultureInfo.InvariantCulture)} Gb";
            DiskUsageInfo = $"Percentage Used: {poll.disk_percentage.ToString("N2", CultureInfo.InvariantCulture)}%";

        }
        public virtual void Init()
        {

            FreeAmount = new ObservableValue();
            UsedAmount = new ObservableValue();
            TotalAmount = new ObservableValue();
            PercentUsed = new ObservableValue();
            DiskUsed = new ObservableValue();

            Graph = new AvaloniaList<ISeries> {
                new PieSeries<ObservableValue> { Values = new ObservableValue[] { FreeAmount }, Name = "Free Space (Gb)", InnerRadius = 30 },
                new PieSeries<ObservableValue> { Values = new ObservableValue[] { UsedAmount }, Name = "Used Space (Gb)", InnerRadius = 30 }
            };
        }

    }
}
