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
using Org.BouncyCastle.Asn1.BC;
using System.Runtime.CompilerServices;
using Castle.Core.Logging;
using log4net;
using System.Globalization;

namespace MetricsMonitorClient.ViewModels
{
    public class MemoryViewModel : ViewModelBase{
        private readonly IMemoryFactory _memoryFactory;
        private readonly ILog _logger;
        #region Constructor

        public MemoryViewModel(IMemoryFactory memoryFactory, ILog logger) {
            _memoryFactory = memoryFactory;
            _logger = logger;
            UsagePolls = new AvaloniaList<MemoryUsagePollDto>();
            ClockLock = new SemaphoreSlim(1, 1);
            InitGraph();
        }
        #endregion Constructor
        #region Properties

        public SemaphoreSlim ClockLock { get; private set; }
        public AvaloniaList<MemoryUsagePollDto> UsagePolls { get; }



        //Current usage values
        private string _currentUsedPct;
        public string CurrentUsedPct {
            get { return _currentUsedPct; }
            set { this.RaiseAndSetIfChanged(ref _currentUsedPct, value); }
        }

        private string _currentMemoryAvailable;
        public string CurrentMemoryAvailable {
            get { return _currentMemoryAvailable; }
            set { this.RaiseAndSetIfChanged(ref _currentMemoryAvailable, value); }
        }

        private string _currentTotal;
        public string CurrentTotal {
            get { return _currentTotal; }
            set { this.RaiseAndSetIfChanged(ref _currentTotal, value); }
        }

        private string _currentUsedAmt;
        public string CurrentUsedAmt {
            get { return _currentUsedAmt; }
            set { this.RaiseAndSetIfChanged(ref _currentUsedAmt, value); }
        }


        //graph data
        private ObservableCollection<ISeries> _usagePercentageGraph;
        public ObservableCollection<ISeries> UsagePercentageGraph {
            get { return _usagePercentageGraph; }
            set { this.RaiseAndSetIfChanged(ref _usagePercentageGraph, value); }
        }

        private ObservableCollection<ISeries> _availableMemoryGraph;
        public ObservableCollection<ISeries> AvailableMemoryGraph {
            get { return _availableMemoryGraph; }
            set { this.RaiseAndSetIfChanged(ref _availableMemoryGraph, value); }
        }

        private ObservableCollection<ISeries> _totalMemoryGraph;
        public ObservableCollection<ISeries> TotalMemoryGraph {
            get { return _totalMemoryGraph; }
            set { this.RaiseAndSetIfChanged(ref _totalMemoryGraph, value); }
        }

        private ObservableCollection<ISeries> _usedMemoryGraph;
        public ObservableCollection<ISeries> UsedMemoryGraph {
            get { return _usedMemoryGraph; }
            set { this.RaiseAndSetIfChanged(ref _usedMemoryGraph, value); }
        }

        public Axis[] YAxesPct { get; set; } =
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


        public Axis[] YAxesGb { get; set; } =
        {
        new Axis
        {
            Name = "Amount (Gb)",
            NamePadding = new LiveChartsCore.Drawing.Padding(0, 5),
            LabelsPaint = new SolidColorPaint
            {
                Color = SKColors.AliceBlue,
            },
            Labeler = Labelers.SevenRepresentativeDigits,
            MinLimit = 0.0,
            MaxLimit = 50
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


        private void UpdateUIData() {
            
            var poll = Task.Run(() => _memoryFactory.GetLatestMemoryPollAsync()).Result as MemoryUsagePollDto;

            if (poll == null) { return; }

            UpdateCurrentStats(poll);

            UsagePolls.Add(poll);

            if (!UsagePolls.Any()) { return; }

            if (UsagePolls.Count > MMConstants.PollBufferSize) { UsagePolls.RemoveRange(0, 1); }

            UpdateGraphs();

        }

        public void  TickClock() {
            ClockLock.Wait();
            UpdateUIData();
            ClockLock.Release();
        }

        public void InitGraph() {
            try {
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

                AvailableMemoryGraph = new ObservableCollection<ISeries> {
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

                TotalMemoryGraph = new ObservableCollection<ISeries> {
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

                UsedMemoryGraph = new ObservableCollection<ISeries> {
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
                AvailableMemoryGraph[0].Values = new ObservableValue[MMConstants.PollBufferSize].AsEnumerable();
                TotalMemoryGraph[0].Values = new ObservableValue[MMConstants.PollBufferSize].AsEnumerable();
                UsedMemoryGraph[0].Values = new ObservableValue[MMConstants.PollBufferSize].AsEnumerable();

                PreloadGraphsAndLabels();
            }catch(Exception ex) {
                _logger.Error(ex);
            }


        }
        public void PreloadGraphsAndLabels() {
            var polls = Task.Run(() => _memoryFactory.GetAllMemoryPollsAsync()).Result;
            var pollDataSelection = polls.Take(MMConstants.PollBufferSize);

            UsagePercentageGraph[0].Values = pollDataSelection.Select(u => new ObservableValue { Value = u.percentage_used });
            AvailableMemoryGraph[0].Values = pollDataSelection.Select(u => new ObservableValue { Value = u.available_memory });
            TotalMemoryGraph[0].Values = pollDataSelection.Select(u => new ObservableValue { Value = u.total_memory });
            UsedMemoryGraph[0].Values = pollDataSelection.Select(u => new ObservableValue { Value = u.used_memory });

            UpdateCurrentStats(pollDataSelection.FirstOrDefault());

        }
        public void UpdateCurrentStats(MemoryUsagePollDto poll) {
            CurrentUsedPct = $"Memory Usage: {(poll.percentage_used / 100.0).ToString("P", CultureInfo.InvariantCulture)}";
            CurrentMemoryAvailable = $"Available Memory: {poll.available_memory}";
            CurrentTotal = $"Total Memory: {poll.total_memory}";
            CurrentUsedAmt = $"Used Memory: {poll.used_memory}";
        }
       
        public void UpdateGraphs() {
            UsagePercentageGraph[0].Values = UsagePolls.OrderByDescending(m => m.memory_id ).Select(u => new ObservableValue { Value = u.percentage_used });
            AvailableMemoryGraph[0].Values = UsagePolls.OrderByDescending(m => m.memory_id).Select(u => new ObservableValue { Value = u.available_memory });
            TotalMemoryGraph[0].Values = UsagePolls.OrderByDescending(m => m.memory_id).Select(u => new ObservableValue { Value = u.total_memory });
            UsedMemoryGraph[0].Values = UsagePolls.OrderByDescending(m => m.memory_id).Select(u => new ObservableValue { Value = u.used_memory });

        }
        #endregion Methods
    }
}
