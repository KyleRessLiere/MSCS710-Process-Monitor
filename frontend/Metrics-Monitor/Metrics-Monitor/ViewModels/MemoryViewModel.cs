using Avalonia.Collections;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore;
using MetricsMonitorClient.DataServices.Memory;
using MetricsMonitorClient.DataServices.Memory.Dtos;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;
using LiveChartsCore.Defaults;
using System.Collections.ObjectModel;
using Avalonia.Media;
using LiveChartsCore.Drawing;
using NUnit.Framework.Constraints;

namespace MetricsMonitorClient.ViewModels
{
    public class MemoryViewModel : ViewModelBase{
        private readonly IMemoryFactory _memoryFactory;
        #region Constructor

        public MemoryViewModel(IMemoryFactory memoryFactory) {
            _memoryFactory = memoryFactory;
            UsagePolls = new AvaloniaList<MemoryUsagePollDto>();
            ClockLock = new SemaphoreSlim(1, 1);
            InitGraph();
        }
        #endregion Constructor
        #region Properties

        public SemaphoreSlim ClockLock { get; private set; }
        public AvaloniaList<MemoryUsagePollDto> UsagePolls { get; }

        private string _usageLabel;
        public string UsageLabel {
            get { return _usageLabel; }
            set { this.RaiseAndSetIfChanged(ref _usageLabel, value); }
        }

        private string _availableLabel;
        public string AvailableLabel {
            get { return _availableLabel; }
            set { this.RaiseAndSetIfChanged(ref _availableLabel, value); }
        }

        private string _totalLabel;
        public string TotalLabel {
            get { return _totalLabel; }
            set { this.RaiseAndSetIfChanged(ref _totalLabel, value); }
        }

        private string _usedLabel;
        public string UsedLabel {
            get { return _usedLabel; }
            set { this.RaiseAndSetIfChanged(ref _usedLabel, value); }
        }

        private ObservableCollection<ISeries> _usagePercentageGraph;
        public ObservableCollection<ISeries> UsagePercentageGraph {
            get { return _usagePercentageGraph; }
            set { this.RaiseAndSetIfChanged(ref _usagePercentageGraph, value); }
        }



        private ObservableCollection<ISeries> _availablePercentageGraph;
        public ObservableCollection<ISeries> AvailablePercentageGraph {
            get { return _availablePercentageGraph; }
            set { this.RaiseAndSetIfChanged(ref _availablePercentageGraph, value); }
        }

        private ObservableCollection<ISeries> _totalPercentageGraph;
        public ObservableCollection<ISeries> TotalPercentageGraph {
            get { return _totalPercentageGraph; }
            set { this.RaiseAndSetIfChanged(ref _totalPercentageGraph, value); }
        }

        private ObservableCollection<ISeries> _usedPercentageGraph;
        public ObservableCollection<ISeries> UsedPercentageGraph {
            get { return _usedPercentageGraph; }
            set { this.RaiseAndSetIfChanged(ref _usedPercentageGraph, value); }
        }

        public Axis[] YAxes { get; set; } =
        {
        new Axis
        {
            Name = "Percentage",
            NamePadding = new LiveChartsCore.Drawing.Padding(0, 5),

            LabelsPaint = new SolidColorPaint
            {
                Color = SKColors.AliceBlue,
            },

            Labeler = Labelers.SevenRepresentativeDigits,
            MinLimit = 0.0,
            MaxLimit = 100
        }
    };


        public Axis[] XAxes { get; set; } =
    {
        new Axis
        {
            LabelsPaint = new SolidColorPaint
            {
                Color = SKColors.AliceBlue,
            },

            Labeler = Labelers.SevenRepresentativeDigits,
            IsInverted = true
        }
    };
        #endregion Properties
        #region Methods


        private void GetLatestPoll() {
            
            var poll = Task.Run(() => _memoryFactory.GetLatestMemoryPollAsync()).Result;

            if (poll == null) { return; }

            UpdateCurrentStats(poll);

            UsagePolls.Add(poll);

            if (!UsagePolls.Any()) { return; }

            if (UsagePolls.Count > MMConstants.PollBufferSize) { UsagePolls.RemoveRange(0, 1); }

            UpdateGraphs();

        }

        public void  TickClock() {
            ClockLock.Wait();
            GetLatestPoll();
            ClockLock.Release();
        }

        public void InitGraph() {
            UsagePercentageGraph = new ObservableCollection<ISeries>
            {
                new LineSeries<ObservableValue>
                {
                    Name = "Memory Usage Percentage",
                    Stroke = new SolidColorPaint(SKColors.Blue) { StrokeThickness = 0 },
                    ZIndex = 0,
                    LineSmoothness = 0,
                    EasingFunction = null,
                    AnimationsSpeed = TimeSpan.Zero
                }

            };
            
            AvailablePercentageGraph = new ObservableCollection<ISeries> {
                new LineSeries<ObservableValue>
                {
                    Name = "Available Memory",
                    Stroke = new SolidColorPaint(SKColors.Red) { StrokeThickness = 0 },
                    ZIndex = 2,
                    LineSmoothness = 0,
                    EasingFunction = null,
                    AnimationsSpeed = TimeSpan.Zero
                },
            };
           
            TotalPercentageGraph = new ObservableCollection<ISeries> {
                   new LineSeries<ObservableValue>
                {
                    Name = "Total Memory",
                    Stroke = new SolidColorPaint(SKColors.Green) { StrokeThickness = 0 },
                    ZIndex = 1,
                    LineSmoothness = 0,
                    EasingFunction = null,
                    AnimationsSpeed = TimeSpan.Zero
                }
            };

            UsedPercentageGraph = new ObservableCollection<ISeries> {
                new LineSeries<ObservableValue>
                {
                    Name = "Used Memory",
                    Stroke = new SolidColorPaint(SKColors.Yellow) { StrokeThickness = 0 },
                    ZIndex = 3,
                    LineSmoothness = 0,
                    EasingFunction = null,
                    AnimationsSpeed = TimeSpan.Zero
                }
            };
            UsagePercentageGraph[0].Values = new ObservableValue[MMConstants.PollBufferSize].AsEnumerable();
            AvailablePercentageGraph[0].Values = new ObservableValue[MMConstants.PollBufferSize].AsEnumerable();
            TotalPercentageGraph[0].Values = new ObservableValue[MMConstants.PollBufferSize].AsEnumerable();
            UsedPercentageGraph[0].Values = new ObservableValue[MMConstants.PollBufferSize].AsEnumerable();
        }


        public void UpdateGraphs() {
            UsagePercentageGraph[0].Values = UsagePolls.Select(u => new ObservableValue { Value = u.percentage_memory });
            AvailablePercentageGraph[0].Values = UsagePolls.Select(u => new ObservableValue { Value = u.available_memory });
            TotalPercentageGraph[0].Values = UsagePolls.Select(u => new ObservableValue { Value = u.total_memory });
            UsedPercentageGraph[0].Values = UsagePolls.Select(u => new ObservableValue { Value = u.used_memory });

        }

        public void UpdateCurrentStats(MemoryUsagePollDto poll) {

        }

        #endregion Methods
    }
}
