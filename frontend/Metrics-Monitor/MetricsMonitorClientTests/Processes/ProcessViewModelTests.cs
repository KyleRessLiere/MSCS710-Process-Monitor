using log4net;
using MetricsMonitorClient.DataServices.Process;
using MetricsMonitorClient.Models.Process;
using MetricsMonitorClient.ViewModels;
using Moq;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetricsMonitorClientTests.Processes
{
    public class ProcessViewModelTests {
        private readonly Mock<IProcessFactory> _factory;
        private readonly Mock<ILog> _logger;


        public ProcessViewModelTests() {
            _factory = new Mock<IProcessFactory>();
            _logger = new Mock<ILog>();
        }


        [TearDown]
        public void TearDown() {
            _factory.Reset();
            _logger.Reset();
        }


        [Test]
        public void ProcessViewModel_WillGroupProcPolls_ByName() {
            var procList = new List<ProcessPoll> {
                new ProcessPoll{ ProcessName = "a"},
                new ProcessPoll{ ProcessName = "b"},
                new ProcessPoll{ ProcessName = "a"},
                new ProcessPoll{ ProcessName = "b"},
            };

            _factory.Setup(f => f.GetRunningProcessesAsync()).ReturnsAsync(procList);

            var vm = new ProcessViewModel(_factory.Object, _logger.Object);


            vm.UpdateUiData();
            vm.UpdateUiData();
            vm.UpdateUiData();
            vm.UpdateUiData();
            vm.UpdateUiData();
            vm.UpdateUiData();
            vm.UpdateUiData();
            vm.UpdateUiData();



            Assert.That(vm.ProcessDataRows.Count, Is.EqualTo(2), "Made too many groups");
            
            var procsA = vm.ProcessDataRows.Where(p => p.ProcessName == "a").First();
            var procsB = vm.ProcessDataRows.Where(p => p.ProcessName == "b").First();

            Assert.That(procsA.Processes.Any(p => p.ProcessName != "a") == false, "Did not sort correctly");
            Assert.That(procsB.Processes.Any(p => p.ProcessName != "b") == false, "Did not sort correctly");

        }



        [Test]
        public void ProcessViewModel_WillGroupProcPolls_ByName_AndAddPercents() {
            var procList = new List<ProcessPoll> {
                new ProcessPoll{ ProcessName = "a", MemoryUsage = .1, CpuPercent = .1},
                new ProcessPoll{ ProcessName = "b", MemoryUsage = .2, CpuPercent = .2},
                new ProcessPoll{ ProcessName = "a", MemoryUsage = .1, CpuPercent = .1},
                new ProcessPoll{ ProcessName = "b", MemoryUsage = .2, CpuPercent = .2},
            };

            _factory.Setup(f => f.GetRunningProcessesAsync()).ReturnsAsync(procList);

            var vm = new ProcessViewModel(_factory.Object, _logger.Object);


            vm.UpdateUiData();
            vm.UpdateUiData();
            vm.UpdateUiData();
            vm.UpdateUiData();
            vm.UpdateUiData();
            vm.UpdateUiData();
            vm.UpdateUiData();
            vm.UpdateUiData();



            Assert.That(vm.ProcessDataRows.Count, Is.EqualTo(2), "Made too many groups");

            var procsA = vm.ProcessDataRows.Where(p => p.ProcessName == "a").First();
            var procsB = vm.ProcessDataRows.Where(p => p.ProcessName == "b").First();

            Assert.That(procsA.CpuUsagePctTotal == .2, "Did not sort correctly");
            Assert.That(procsA.MemoryUsagePctTotal == .2, "Did not sort correctly");

            Assert.That(procsB.CpuUsagePctTotal == .4, "Did not sort correctly");
            Assert.That(procsB.MemoryUsagePctTotal == .4, "Did not sort correctly");

        }
    }
}
