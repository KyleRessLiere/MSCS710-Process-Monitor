using MetricsMonitorClient.DataServices;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetricsMonitorClient.Models {
    public class StatsContainerBase : ReactiveObject {

        protected virtual int _MaxBufferSize => MMConstants.StatsContainerMaxBuffer;
        
        public double _firstQ;
        public double FirstQ {
            get { return _firstQ; }
            set { this.RaiseAndSetIfChanged(ref _firstQ, value); }
        }
        private double _secondQ;
        public double SecondQ {
            get { return _secondQ; }
            set { this.RaiseAndSetIfChanged(ref _secondQ, value); }
        }
        private double _thirdQ;
        public double ThirdQ {
            get { return _thirdQ; }
            set { this.RaiseAndSetIfChanged(ref _thirdQ, value); }
        }
        private double _min;
        public double Min {
            get { return _min; }
            set { this.RaiseAndSetIfChanged(ref _min, value); }
        }
        private double _max;
        public double Max {
            get { return _max; }
            set { this.RaiseAndSetIfChanged(ref _max, value); }
        }

        private double _avg;
        public double Avg {
            get { return _avg; }
            set { this.RaiseAndSetIfChanged(ref _avg, value); }
        }
        private double _current;
        public double Current {
            get { return _current; }
            set { this.RaiseAndSetIfChanged(ref _current, value); }
        }

        public int Id { get; set; }
        public virtual void AddAndUpdate(double poll) {
            //TODO: do this stuff right, redo percentiles and avg
            
            AddPoll(poll);

            Min = Math.Min(Min, poll);
            Max = Math.Max(Max, poll);

            double nPlusOne = (double)PollList.Count + 1.0;

            PollList.Sort();

            FirstQ = Percentile(PollList, .25);
            SecondQ = Percentile(PollList, .50);
            ThirdQ = Percentile(PollList, .75);

            Avg = PollList.Average();

            Current = poll;
        }
        
        protected void AddPoll(double poll) {

            if (PollList.Count >= _MaxBufferSize) {
                PollList.RemoveRange(0, 1);
            }

            PollList.Add(poll);
           
        }


        protected static double Percentile(IEnumerable<double> seq, double percentile) {
            var elements = seq.ToArray();
            Array.Sort(elements);
            double realIndex = percentile * (elements.Length - 1);
            int index = (int)realIndex;
            double frac = realIndex - index;
            if (index + 1 < elements.Length)
                return elements[index] * (1 - frac) + elements[index + 1] * frac;
            else
                return elements[index];
        }

        public virtual List<double> PollList { get; protected set; }


        public StatsContainerBase() {
            Min = double.MaxValue;
            Max = double.MinValue;
        }
    }
}
