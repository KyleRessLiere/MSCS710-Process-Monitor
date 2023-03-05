using Avalonia.Collections;
using MetricsMonitorClient.DataServices.Memory;
using MetricsMonitorClient.DataServices.Memory.Dtos;
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
        private readonly IMemoryFactory _memoryFactory;
        #region Constructor

        public MemoryViewModel(IMemoryFactory memoryFactory) {
            _memoryFactory = memoryFactory;
            UsagePolls = new AvaloniaList<MemoryUsagePollDto>();
            Change();
        }
        #endregion Constructor
        #region Properties

        public AvaloniaList<MemoryUsagePollDto> UsagePolls { get; private set; }

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

        public void GetLatestPoll() {
            var poll = _memoryFactory.GetLatestMemoryPoll();
            if (poll != null) {
                UsagePolls.Add(poll);
            }
        }

        public void TickClock() {
            GetLatestPoll();
        }

        #endregion Methods
    }
}
