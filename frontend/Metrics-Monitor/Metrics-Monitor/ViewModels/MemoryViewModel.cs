using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MetricsMonitorClient.ViewModels
{
    public class MemoryViewModel : ViewModelBase{
        #region Constructor
        public MemoryViewModel() {
            TestingText = "First!";
            Change();
        }
        #endregion Constructor
        #region Properties

        private string testingText;
        public string TestingText {
            get { return testingText; }
            set { this.RaiseAndSetIfChanged(ref testingText, value); }
        }
        #endregion Properties
        #region Methods
        public void Change() {
            Thread.Sleep(8000);
            TestingText = "Second!";
            Thread.Sleep(700);
            TestingText = "Third!";
            Thread.Sleep(700);
            TestingText = "Fourth!";
        }
        #endregion Methods
    }
}
