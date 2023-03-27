using Avalonia.Collections;
using Castle.Core.Logging;
using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore.SkiaSharpView;
using log4net;
using MetricsMonitorClient.DataServices.CPU;
using MetricsMonitorClient.DataServices.CPU.Dtos;
using MetricsMonitorClient.Models.CPU;
using Org.BouncyCastle.Asn1.Mozilla;
using ReactiveUI;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MetricsMonitorClient.ViewModels
{
    public class CPUViewModel : ViewModelBase {
        private readonly ICPUFactory _factory;
        private readonly ILog _logger;
        #region Constructor
        public CPUViewModel(ICPUFactory factory, ILog logger) {
            _factory = factory;
            _logger = logger;
            ClockLock = new SemaphoreSlim(1, 1);
            CPUPolls = new List<CPUDto>();
        }
        #endregion Constructor

        #region Properties

        public SemaphoreSlim ClockLock { get; private set; }

        public List<CPUDto> CPUPolls { get; }

        private string _coreCountPhysical;
        public string CoreCountPhysical {
            get { return _coreCountPhysical; }
            set { this.RaiseAndSetIfChanged(ref _coreCountPhysical, value); }
        }

        private string _contextSwitches;
        public string ContextSwitches {
            get { return _contextSwitches; }
            set { this.RaiseAndSetIfChanged(ref _contextSwitches, value); }
        }

        private string _interrupts;
        public string Interrupts {
            get { return _interrupts; }
            set { this.RaiseAndSetIfChanged(ref _interrupts, value); }
        }

        private string _softInterrupts;
        public string SoftInterrupts {
            get { return _softInterrupts; }
            set { this.RaiseAndSetIfChanged(ref _softInterrupts, value); }
        }

        private string _sysCalls;
        public string SysCalls {
            get { return _sysCalls; }
            set { this.RaiseAndSetIfChanged(ref _sysCalls, value); }
        }

        private string _currentUsagePercentage;
        public string CurrentUsagePercentage {
            get { return _currentUsagePercentage; }
            set { this.RaiseAndSetIfChanged(ref _currentUsagePercentage, value); }
        }



        private double _pct;
        public double PCT {
            get { return _pct; }
            set { this.RaiseAndSetIfChanged(ref _pct, value); }
        }


        public IEnumerable

        public bool IsInitialized { get; set; }

        public int CoreCount { get; set; }

        private ObservableCollection<ISeries> _coreGraphs;
        public ObservableCollection<ISeries> CoreGraphs {
            get { return _coreGraphs; }
            set { this.RaiseAndSetIfChanged(ref _coreGraphs, value); }
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
        public void TickClock() {
            ClockLock.Wait();
            UpdateUiData();
            ClockLock.Release();
        }


        public void UpdateUiData() {
            try {
                var poll = Task.Run(() => _factory.GetLatestCPUPollAsync()).Result;
                if (poll == null) { return; }

                //UpdateCurrentStats();

                if (!IsInitialized) {
                    InitData(poll);
                    IsInitialized = true;
                }


                CPUPolls.Add(poll);

                if (!CPUPolls.Any()) { return; }

                if (CPUPolls.Count > MMConstants.PollBufferSize) { CPUPolls.RemoveRange(0, 1); }

                UpdateGraphs(poll);

            }catch(Exception ex) {
                _logger.Error(ex);
            }
            

        }

        //public void UpdateGraphs(CPUDto poll) {


        //    for (int i = 0; i < poll.cpu_percentage_per_core.Length; i++) {
        //        CoreGraphs[i].Values = newData;
        //    }
        //}


        public void UpdateGraphs(CPUDto poll) {
            var newData = CPUPolls.Select(p => p.cpu_percentage_per_core.Sum() / p.cpu_percentage_per_core.Length).ToList();

            PCT = newData[0];



        }

        public void InitGraphs() {
            CoreGraphs = new ObservableCollection<ISeries>();
            for (int i = 0; i < CoreCount; i++) {
                //CoreGraphs
                var coreGraph = new LineSeries<ObservableValue> {
                    Name = $"Core {i}",
                    Stroke = new SolidColorPaint(SKColors.Blue) { StrokeThickness = 0 },
                    ZIndex = 0,
                    LineSmoothness = 0,
                    EasingFunction = null,
                    AnimationsSpeed = TimeSpan.Zero,
                    Values = new ObservableValue[MMConstants.PollBufferSize]
                };
            };
        }




        public void InitData(CPUDto poll) {
            CoreCountPhysical = $"Physical Cores: {poll.cpu_count_physical}";
            CoreCount = poll.cpu_count_physical;
            InitGraphs();
        }


        public void UpdateCurrentStats(CPUDto poll) {
            CurrentUsagePercentage = $"Current Usage: {poll.cpu_percent}%";
            ContextSwitches = $"Context Switches: {poll.cpu_ctx_switches}";
            Interrupts = $"Interrupts: {poll.interrupts}";
            SysCalls = $"System Calls: {poll.syscalls}";
            SoftInterrupts = $"Software Interrupts: {poll.soft_interrupts}";
        } 

    }
}
