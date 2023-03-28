using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetricsMonitorClient.Models.CPU
{
    public class CPUD : ReactiveObject
    {
        private int count;
        public int Count
        {
            get { return count; }
            set { this.RaiseAndSetIfChanged(ref count, value); }
        }


        private decimal usage;
        public decimal Usage
        {
            get { return usage; }
            set { this.RaiseAndSetIfChanged(ref usage, value); }
        }

        private string info;
        public string Info
        {
            get { return info; }
            set { this.RaiseAndSetIfChanged(ref info, value); }
        }
    }
}
