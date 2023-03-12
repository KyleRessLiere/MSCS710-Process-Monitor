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


        private string testingText;
        public string TestingText {
            get { return testingText; }
            set { this.RaiseAndSetIfChanged(ref testingText, value); }
        }

        private ObservableCollection<ISeries> _memoryPercentageGraph;
        public ObservableCollection<ISeries> MemoryPercentageGraph {
            get { return _memoryPercentageGraph; }
            set { this.RaiseAndSetIfChanged(ref _memoryPercentageGraph, value); }
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

            Labeler = Labelers.SevenRepresentativeDigits 
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

            Labeler = Labelers.SevenRepresentativeDigits
        }
    };
        #endregion Properties
        #region Methods


        private void GetLatestPoll() {
            
            var poll = _memoryFactory.GetLatestMemoryPoll();

            if (poll == null) { return; }

            UsagePolls.Add(poll);

            if (!UsagePolls.Any()) { return; }

            if (UsagePolls.Count > MMConstants.PollBufferSize) { UsagePolls.RemoveRange(0, 1); }

            UpdateGraphs();

        }

        public void TickClock() {
            ClockLock.Wait();
            GetLatestPoll();
            ClockLock.Release();
        }

        public void InitGraph() {
            MemoryPercentageGraph = new ObservableCollection<ISeries>
           {
                new LineSeries<ObservableValue>
                {
                    Name = "Memory Usage Percentage",
                    Stroke = new SolidColorPaint(SKColors.Blue) { StrokeThickness = 0 },
                    ZIndex = 0,
                    LineSmoothness = 0,
                    EasingFunction = null,
                    AnimationsSpeed = null
                },
                new LineSeries<ObservableValue>
                {
                    Name = "Available Memory",
                    Stroke = new SolidColorPaint(SKColors.Red) { StrokeThickness = 0 },
                    ZIndex = 2,
                    LineSmoothness = 0,
                    EasingFunction = null,
                    AnimationsSpeed = null
                },
                new LineSeries<ObservableValue>
                {
                    Name = "Total Memory",
                    Stroke = new SolidColorPaint(SKColors.Green) { StrokeThickness = 0 },
                    ZIndex = 1,
                    LineSmoothness = 0,
                    EasingFunction = null,
                    AnimationsSpeed = null
                },
                new LineSeries<ObservableValue>
                {
                    Name = "Used Memory",
                    Stroke = new SolidColorPaint(SKColors.Yellow) { StrokeThickness = 0 },
                    ZIndex = 3,
                    LineSmoothness = 0,
                    EasingFunction = null,
                    AnimationsSpeed = null
                }
            };
        }


        public void UpdateGraphs() {
           if(MemoryPercentageGraph == null || MemoryPercentageGraph.Count < 4 ) return;

            MemoryPercentageGraph[0].Values = UsagePolls.Select(u => new ObservableValue { Value = u.percentage_memory });
            MemoryPercentageGraph[1].Values = UsagePolls.Select(u => new ObservableValue { Value = u.available_memory });
            MemoryPercentageGraph[2].Values = UsagePolls.Select(u => new ObservableValue { Value = u.total_memory });
            MemoryPercentageGraph[3].Values = UsagePolls.Select(u => new ObservableValue { Value = u.used_memory });
        }

        #endregion Methods
    }
}
