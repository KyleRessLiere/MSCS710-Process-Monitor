using MetricsMonitorClient.DataServices;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetricsMonitorClient.Models {
    public class StatsContainerBase : ReactiveObject {

        #region Constructor
        public StatsContainerBase() {
            Min = double.MaxValue;
            Max = double.MinValue;
        }
        #endregion Constructor

        #region Fields
        protected virtual int _MaxBufferSize => MMConstants.StatsContainerMaxBuffer;

        #endregion Fields
        #region Properties

        public double FirstQ { get; set; }
     
        public double SecondQ { get; set; }
    
        public double ThirdQ { get; set; }
       
        public double Min { get; set; }
       
        public double Max { get; set; }
       
        public double Avg { get; set; }
        public double Current { get; set; }

        public int Id { get; set; }
        public virtual List<double> PollList { get; protected set; }
        #endregion Properties
      
        #region Methods
        public virtual void AddAndUpdate(double poll) {
            
            AddPoll(poll);

            Min = Math.Min(Min, poll);
            Max = Math.Max(Max, poll);

            FirstQ = Percentile(PollList, .25);
            SecondQ = Percentile(PollList, .50);
            ThirdQ = Percentile(PollList, .75);

            Avg = PollList.Average();

            Current = poll;

            NotifyUi();
            
        }
        
        protected void AddPoll(double poll) {
            if (PollList.Count >= _MaxBufferSize) {
                PollList.RemoveRange(0, 1);
            }

            PollList.Add(poll);
        }

        protected virtual void NotifyUi() {
            foreach(var property in propertiesToTrack) {
                this.RaisePropertyChanged(property);
            }
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
        #endregion Methods

        #region Change Tracking

        protected static string[] propertiesToTrack = new string[] {
            nameof(FirstQ),
            nameof(SecondQ),
            nameof(ThirdQ),
            nameof(Min),
            nameof(Max),
            nameof(Avg),
            nameof(Current)
        };

        #endregion Change Tracking



    }
}
