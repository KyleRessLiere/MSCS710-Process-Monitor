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

namespace MetricsMonitorClient.Models {
    public abstract class ChartContainerBase : ReactiveObject {
        protected readonly int _bufferSize;

        public ChartContainerBase(string graphName, string yAxisName, string xAxisName, int xMax = 100, int yMax = 100) {
            _bufferSize = MMConstants.PollBufferSize;
            GraphName = graphName;
            YAxisName = yAxisName;
            XAxisName = xAxisName;
            YMax = yMax;
            XMax = xMax;
            InitGraphAndAxes();
        }
        public ChartContainerBase() { }
       
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
        private ISeries _graph;
        public ISeries Graph {
            get { return _graph; }
            set { this.RaiseAndSetIfChanged(ref _graph, value); }
        }

        private Axis[] _yAxis;
        public Axis[] YAxis {
            get { return _yAxis; }
            set { this.RaiseAndSetIfChanged(ref _yAxis, value); }
        }

        private Axis[] _xAxis;
        public Axis[] XAxis {
            get { return _xAxis; }
            set { this.RaiseAndSetIfChanged(ref _xAxis, value); }
        }

        private double _currentValue;
        public double CurrentValue {
            get { return _currentValue; }
            set { this.RaiseAndSetIfChanged(ref _currentValue, value); }
        }
        /// <summary>
        /// Stored list of the values backing the graph
        /// </summary>
        public List<ObservableValue> Values { get; set; }


        //update
        public void Update(double newValue) {
            if(Values.Count  >= _bufferSize) { Values.RemoveRange(0, 1); }
           
            var newVal = new ObservableValue(newValue);
          
            Values.Add(newVal);
         
            Graph.Values = Values;

            CurrentValue = newValue;
        }

        //clear
        public void Clear() {
            Graph.Values = new ObservableValue[0];
            Values.Clear();
        }

        /// <summary>
        /// Allows the graph to be loaded with all needed data instantly instead of waiting for data to flow in over time
        /// </summary>
        /// <param name="values">values to load into the graph</param>
        public void PreLoad(List<double> values) {
            if(values == null || !values.Any()) { return; }

            var selection = values.Count >= _bufferSize ? values.TakeLast(_bufferSize).ToList() : values;

            var newValues = selection.Select(v =>  new ObservableValue(v)).ToList();

            Values.AddRange(newValues);
        }

        public virtual void InitGraphAndAxes() {
            Graph = new LineSeries<ObservableValue> {
                Name = GraphName,
                Stroke = new SolidColorPaint(SKColors.Blue) { StrokeThickness = 0 },
                ZIndex = 0,
                LineSmoothness = 0,
                EasingFunction = null,
                AnimationsSpeed = TimeSpan.Zero
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
                    MaxLimit = YMax
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
                    MaxLimit = XMax
                }
            };

        }

        //title




        //current value



        //x axis



        //y axis

    }
}
