using LiveChartsCore;
using MetricsMonitorClient;
using MetricsMonitorClient.DataServices.Memory;
using MetricsMonitorClient.DataServices.Memory.Dtos;
using MetricsMonitorClient.ViewModels;
using Moq;
using NP.Concepts.Behaviors;
using NP.Utilities;
using System.Collections.ObjectModel;
using ILog = log4net.ILog;
namespace MetricsMonitorClientTests.ViewModels {
    public class MemoryViewModelTests
    {
        private readonly Mock<IMemoryFactory> _factory;
        private readonly Mock<ILog> _logger;
        public MemoryViewModelTests()
        {
            _factory = new Mock<IMemoryFactory>();
            _logger = new Mock<ILog>();
        }

        [TearDown]
        public void TearDown()
        {
            _factory.Reset();
           
            _logger.Reset();
        }

        //[TearDown]
        //public void TearDown() {
        //    _factory.Reset();
        //    _logger.Reset();
        //}




        [Test]
        public void MemoryViewModel_GraphsInitOnLoad()
        {
            var memDto = new MemoryUsagePollDto { available_memory = 1.1, memory_id = 1, percentage_used = 11.2, poll_id = 1, total_memory = 32.1, used_memory = 11.0 };
            var result = new List<MemoryUsagePollDto> { memDto };
            _factory.Setup(f => f.GetAllMemoryPollsAsync()).ReturnsAsync(result);
            var vm = new MemoryViewModel(_factory.Object, _logger.Object);

            Assert.That(vm.UsagePercentageGraph.GetType(), Is.EqualTo(typeof(ObservableCollection<ISeries>)), "UsagePercentageGraph Is Created:");
            Assert.That(vm.AvailableMemoryGraph.GetType(), Is.EqualTo(typeof(ObservableCollection<ISeries>)), "AvailableMemoryGraph Is Created:");
            Assert.That(vm.TotalMemoryGraph.GetType(), Is.EqualTo(typeof(ObservableCollection<ISeries>)), "TotalMemoryGraph Is Created:");
            Assert.That(vm.UsedMemoryGraph.GetType(), Is.EqualTo(typeof(ObservableCollection<ISeries>)), "UsedMemoryGraph Is Created:");

            Assert.That(vm.UsagePercentageGraph[0].Values.Count(), Is.EqualTo(1), "UsagePercentageGraph Dataset is the right length: ");
            Assert.That(vm.AvailableMemoryGraph[0].Values.Count(), Is.EqualTo(1), "AvailableMemoryGraph Dataset is the right length: ");
            Assert.That(vm.TotalMemoryGraph[0].Values.Count(), Is.EqualTo(1), "TotalMemoryGraph Dataset is the right length: ");
            Assert.That(vm.UsedMemoryGraph[0].Values.Count(), Is.EqualTo(1), "UsedMemoryGraph Dataset is the right length: ");

        }
        /// <summary>
        /// Test validates that the viewmodel is able to deal with a malfunctioning main clock and can maintain proper operation without issue
        /// </summary>
        [TestCase (128, ExpectedResult = MMConstants.PollBufferSize)]
        [TestCase (64, ExpectedResult = MMConstants.PollBufferSize)]
        [TestCase (32, ExpectedResult = MMConstants.PollBufferSize)]
        public int MemoryViewModel_RogueClockThreads_DoNot_OverFillTheBuffer(int threadCount) {
           
            var memDto = new MemoryUsagePollDto { available_memory = 1.1, memory_id = 1, percentage_used = 11.2, poll_id = 1, total_memory = 32.1, used_memory = 11.0 };
            _factory.Setup(f => f.GetLatestMemoryPollAsync()).ReturnsAsync(memDto);
            var vm = new MemoryViewModel(_factory.Object, _logger.Object);


            Parallel.ForEach(Enumerable.Range(0, threadCount), n => vm.TickClock());

            return vm.UsagePolls.Count;
        }

    }
}
