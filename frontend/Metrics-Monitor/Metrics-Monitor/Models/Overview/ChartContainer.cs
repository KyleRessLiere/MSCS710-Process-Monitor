using Avalonia.Collections;
using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using ReactiveUI;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetricsMonitorClient.Models.Overview
{
    /// <summary>
    /// Wrapper class to maintain all data and UI components of LvCharts graphs
    /// </summary>
    public class ChartContainer : ReactiveObject
    {
        public static readonly int BufferSize = MMConstants.PollBufferSize;

        #region Constructors
        /// <param name="graphName">Title to display for the graph</param>
        /// <param name="yAxisName">Label to show for the Y axis</param>
        /// <param name="xAxisName">Label to show for the X axis</param>
        /// <param name="startingYMax">Height of the graph</param>
        public ChartContainer(string graphName, string yAxisName, string xAxisName, int startingYMax = 100)
        {
            Values = new AvaloniaList<ObservableValue>();
            GraphName = graphName;
            YAxisName = yAxisName;
            XAxisName = xAxisName;
            YMax = startingYMax;
            InitGraphAndAxes();
        }
        public ChartContainer()
        {
            Values = new AvaloniaList<ObservableValue>();
        }

        #endregion Constructors

        #region Properties

        public string GraphName { get; set; }
        public string YAxisName { get; set; }
        public string XAxisName { get; set; }
        /// <summary>
        /// upper bound for the Y axis of the graph
        /// </summary>
        public int YMax { get; set; }

        /// <summary>
        /// upper bound for the X axis of the graph
        /// </summary>
        public int XMax { get; set; }


        //series
        private ObservableCollection<ISeries> _graph;
        public ObservableCollection<ISeries> Graph
        {
            get { return _graph; }
            set { this.RaiseAndSetIfChanged(ref _graph, value); }
        }

        private Axis[] _yAxis;
        public Axis[] YAxis
        {
            get { return _yAxis; }
            set { this.RaiseAndSetIfChanged(ref _yAxis, value); }
        }

        private Axis[] _xAxis;
        public Axis[] XAxis
        {
            get { return _xAxis; }
            set { this.RaiseAndSetIfChanged(ref _xAxis, value); }
        }

        private double _currentValue;
        public double CurrentValue
        {
            get { return _currentValue; }
            set { this.RaiseAndSetIfChanged(ref _currentValue, value); }
        }
        /// <summary>
        /// Stored list of the values backing the graph
        /// </summary>
        public AvaloniaList<ObservableValue> Values { get; set; }

        #endregion Properties

        #region Methods
        public void Update(double newValue)
        {
            if (Values.Count >= BufferSize) { Values.RemoveRange(0, 1); }

            var newVal = new ObservableValue(newValue);

            Values.Add(newVal);

            Graph[0].Values = Values;
            var currentMax = Values.Max(v => v.Value);
            YAxis[0].MaxLimit = currentMax * 1.2;
            CurrentValue = newValue;
        }

        public virtual void InitGraphAndAxes()
        {
            Graph = new ObservableCollection<ISeries> {
                new LineSeries<ObservableValue>
                {
                    Name = GraphName,
                    Stroke = new SolidColorPaint(SKColors.Red) { StrokeThickness = 0 },
                    ZIndex = 2,
                    LineSmoothness = 0,
                    EasingFunction = null,
                    AnimationsSpeed = TimeSpan.Zero
                },
            };

            var yName = string.IsNullOrEmpty(YAxisName) ? "Amount" : YAxisName;
            var xName = string.IsNullOrEmpty(XAxisName) ? "Time" : XAxisName;

            YAxis = new Axis[] {
                new Axis {
                    Name = yName,
                    NamePadding = new LiveChartsCore.Drawing.Padding(0, 5),
                    LabelsPaint = new SolidColorPaint {
                        Color = SKColors.AliceBlue,
                    },
                    Labeler = Labelers.SevenRepresentativeDigits,
                    MinLimit = 0.0,
                    MaxLimit = YMax == 0 ? 100 : YMax
                }
            };

            XAxis = new Axis[] {
                new Axis {
                    Name = xName,
                    NamePadding = new LiveChartsCore.Drawing.Padding(0, 5),
                    LabelsPaint = new SolidColorPaint {
                        Color = SKColors.AliceBlue,
                    },
                    Labeler = Labelers.SevenRepresentativeDigits,
                    MinLimit = 0.0,
                    MaxLimit = BufferSize,
                    IsInverted = true
                }
            };

        }
        #endregion Methods

    }
}
