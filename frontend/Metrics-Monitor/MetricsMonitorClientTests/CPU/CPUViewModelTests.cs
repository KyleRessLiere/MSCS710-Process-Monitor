using LiveChartsCore;
using MetricsMonitorClient;
using MetricsMonitorClient.DataServices.CPU;
using MetricsMonitorClient.DataServices.CPU.Dtos;
using MetricsMonitorClient.DataServices.Memory;
using MetricsMonitorClient.DataServices.Memory.Dtos;
using MetricsMonitorClient.Models.CPU;
using MetricsMonitorClient.ViewModels;
using Moq;
using NP.Concepts.Behaviors;
using NP.Utilities;
using NUnit.Framework.Constraints;
using System.Collections.ObjectModel;
using ILog = log4net.ILog;
namespace MetricsMonitorClientTests.ViewModels {
    public class CPUViewModelTests {
        private readonly Mock<ICPUFactory> _factory;
        private readonly Mock<ILog> _logger;
        public CPUViewModelTests() {
            _factory = new Mock<ICPUFactory>();
            _logger = new Mock<ILog>();
        }

        [TearDown]
        public void TearDown() {
            _factory.Reset();
            _logger.Reset();
        }

        //[Test]
        //public void MemoryViewModel_GraphsInitOnLoad() {
        //    var cpuDtos = new List<CPUDto>();


        //    var cpuDto1 = new CPUDto() { cpu_count_physical = 4,
        //        cpu_count_virtual = 8,
        //        cpu_ctx_switches = 934,
        //        cpu_id = 1,
        //        cpu_percent = 3.4,
        //        cpu_percentage_per_core = [3.3, 3.1, 99, 924],
        //        interrupts = 6,
        //        soft_interrupts = 66,
        //        syscalls = 453,
        //        poll_id = 444
        //    };

        //    var cpuDto1 = new CPUDto() {
        //        cpu_count_physical = 4,
        //        cpu_count_virtual = 8,
        //        cpu_ctx_switches = 934,
        //        cpu_id = 1,
        //        cpu_percent = 3.4,
        //        cpu_percentage_per_core = [3.3, 3.1, 99, 924],
        //        interrupts = 6,
        //        soft_interrupts = 66,
        //        syscalls = 453,
        //        poll_id = 444
        //    };

        //    var cpuDto1 = new CPUDto() {
        //        cpu_count_physical = 4,
        //        cpu_count_virtual = 8,
        //        cpu_ctx_switches = 934,
        //        cpu_id = 1,
        //        cpu_percent = 3.4,
        //        cpu_percentage_per_core = [3.3, 3.1, 99, 924],
        //        interrupts = 6,
        //        soft_interrupts = 66,
        //        syscalls = 453,
        //        poll_id = 444
        //    };
        //    var cpuDto1 = new CPUDto() {
        //        cpu_count_physical = 4,
        //        cpu_count_virtual = 8,
        //        cpu_ctx_switches = 934,
        //        cpu_id = 1,
        //        cpu_percent = 3.4,
        //        cpu_percentage_per_core = [3.3, 3.1, 99, 924],
        //        interrupts = 6,
        //        soft_interrupts = 66,
        //        syscalls = 453,
        //        poll_id = 444
        //    };


        //    var result = new List<MemoryUsagePollDto> { memDto };
        //    _factory.Setup(f => f.GetAllMemoryPollsAsync()).ReturnsAsync(result);

        //}


        [Test]
        public void CPUViewModel_ShouldOnlyHave_OneSetOfStatsPerCore() {
            var cpuDto1 = new CPUDto() {
                cpu_count_physical = 4,
                cpu_count_virtual = new double[] { 1.2,1.2,1.2},
                cpu_ctx_switches = 934,
                cpu_id = 1,
                cpu_percent = 3.4,
                cpu_percentage_per_core = new double[] { 3.3, 3.1, 99.0, 92.4 },
                interrupts = 6,
                soft_interrupts = 66,
                syscalls = 453,
                poll_id = 444
            };
            _factory.Setup(f => f.GetLatestCPUPollAsync()).ReturnsAsync(cpuDto1);

            var vm = new CPUViewModel(_factory.Object, _logger.Object);

            vm.UpdateUiData();
            vm.UpdateUiData();
            vm.UpdateUiData();
            vm.UpdateUiData();


            Assert.That(vm.StatsContainers.Count, Is.EqualTo(cpuDto1.cpu_percentage_per_core.Length));

        }



        [Test]
        public void CPUViewModel_MetricsContainers_ShouldLimitValues() {
            var cpuDto1 = new CPUDto() {
                cpu_count_physical = 4,
                cpu_count_virtual = new double[] { 1.2, 1.2, 1.2 },
                cpu_ctx_switches = 934,
                cpu_id = 1,
                cpu_percent = 3.4,
                cpu_percentage_per_core = new double[] { 3.3, 3.1, 99.0, 92.4 },
                interrupts = 6,
                soft_interrupts = 66,
                syscalls = 453,
                poll_id = 444
            };
            _factory.Setup(f => f.GetLatestCPUPollAsync()).ReturnsAsync(cpuDto1);

            var vm = new CPUViewModel(_factory.Object, _logger.Object);

            for(var i = 0;i< 600;i++) {
                vm.UpdateUiData();
            }

            Assert.That(vm.StatsContainers.Any(c => c.PollList.Count != MMConstants.StatsContainerMaxBuffer) == false);
        }


        [Test]
        public void CPUViewModel_MetricsContainers_ShouldCorrectlyRunCalcs() {
            var cpuDto1 = new CPUDto() {
                cpu_count_physical = 1,
                cpu_count_virtual = new double[] { 1.2, 1.2, 1.2 },
                cpu_ctx_switches = 934,
                cpu_id = 1,
                cpu_percent = 3.4,
                cpu_percentage_per_core = new double[] { 96.0 },
                interrupts = 6,
                soft_interrupts = 66,
                syscalls = 453,
                poll_id = 444
            };
            _factory.Setup(f => f.GetLatestCPUPollAsync()).ReturnsAsync(cpuDto1);
            var vm = new CPUViewModel(_factory.Object, _logger.Object);

            //299 random numbers from 1-100 (the first value will come from initialization
            double[] usagePcts = new double[] { 5.0, 61.0, 18.0, 99.0, 35.0, 20.0, 74.0, 68.0, 93.0, 88.0, 25.0, 87.0, 13.0, 22.0, 88.0, 9.0, 72.0, 4.0, 11.0, 48.0, 5.0,
                93.0, 2.0, 25.0, 15.0, 14.0, 96.0, 9.0, 38.0, 29.0, 62.0, 86.0, 38.0, 37.0, 89.0, 13.0, 84.0, 86.0, 71.0, 8.0, 86.0, 40.0, 7.0, 72.0, 43.0, 83.0, 83.0, 79.0,
                68.0, 47.0, 44.0, 27.0, 89.0, 79.0, 12.0, 69.0, 62.0, 93.0, 31.0, 44.0, 22.0, 89.0, 21.0, 23.0, 5.0, 94.0, 81.0, 5.0, 79.0, 72.0, 30.0, 0.0, 63.0, 14.0, 45.0,
                61.0, 38.0, 38.0, 53.0, 70.0, 70.0, 94.0, 5.0, 6.0, 89.0, 77.0, 78.0, 24.0, 87.0, 25.0, 62.0, 77.0, 70.0, 77.0, 44.0, 99.0, 26.0, 75.0, 23.0, 32.0, 38.0, 88.0,
                72.0, 99.0, 84.0, 6.0, 3.0, 11.0, 17.0, 18.0, 83.0, 16.0, 35.0, 39.0, 77.0, 23.0, 53.0, 20.0, 42.0, 69.0, 96.0, 55.0, 61.0, 26.0, 54.0, 53.0, 77.0, 47.0, 75.0,
                44.0, 66.0, 19.0, 14.0, 98.0, 14.0, 35.0, 41.0, 21.0, 47.0, 67.0, 44.0, 38.0, 53.0, 69.0, 53.0, 76.0, 98.0, 58.0, 6.0, 16.0, 15.0, 17.0, 21.0, 97.0, 51.0, 69.0,
                14.0, 63.0, 12.0, 84.0, 10.0, 10.0, 95.0, 49.0, 58.0, 60.0, 42.0, 63.0, 69.0, 19.0, 46.0, 92.0, 29.0, 36.0, 63.0, 34.0, 59.0, 89.0, 79.0, 99.0, 35.0, 52.0, 63.0, 68.0,
                15.0, 75.0, 37.0, 97.0, 25.0, 39.0, 32.0, 97.0, 99.0, 35.0, 73.0, 60.0, 48.0, 35.0, 31.0, 55.0, 84.0, 85.0, 28.0, 77.0, 91.0, 29.0, 59.0, 24.0, 64.0, 76.0, 78.0, 86.0,
                33.0, 27.0, 26.0, 51.0, 83.0, 42.0, 48.0, 17.0, 12.0, 58.0, 36.0, 23.0, 60.0, 52.0, 19.0, 6.0, 22.0, 13.0, 43.0, 8.0, 95.0, 23.0, 82.0, 66.0, 61.0, 32.0, 87.0, 42.0,
                12.0, 19.0, 91.0, 29.0, 67.0, 100.0, 9.0, 19.0, 80.0, 29.0, 20.0, 38.0, 75.0, 49.0, 67.0, 19.0, 11.0, 74.0, 59.0, 24.0, 98.0, 64.0, 36.0, 54.0, 60.0, 84.0, 15.0, 76.0,
                83.0, 81.0, 9.0, 44.0, 4.0, 70.0, 28.0, 11.0, 51.0, 75.0, 33.0, 35.0, 21.0, 71.0, 73.0, 61.0, 19.0, 33.0, 29.0, 71.0, 12.0, 78.0, 40.0, 7.0, 14.0, 65.0, 59.0, 83.0, 67.0,
                96.0, 97.0 };
            vm.StatsContainers.Add(new CpuStatsContainer());
            vm.UpdateUiData();
            for (int i = 0;i< usagePcts.Length; i++) {
                var tempDto = new CPUDto() {
                    cpu_count_physical = 1,
                    cpu_count_virtual = new double[] { 1.2, 1.2, 1.2 },
                    cpu_ctx_switches = 934,
                    cpu_id = 1,
                    cpu_percentage_per_core = new double[] { usagePcts[i] }, 
                    cpu_percent = 3.4,
                    interrupts = 6,
                    soft_interrupts = 66,
                    syscalls = 453,
                    poll_id = 444
                };

                vm.UpdateDataSets(tempDto);
            }
            var calc = vm.StatsContainers[0];
           
            Assert.That(calc.FirstQ == 23.75, "25th percentile value is off");
            Assert.That(calc.SecondQ == 49, "50th percentile value is off");
            Assert.That(calc.ThirdQ == 75,"75th percentile value is off");
            Assert.That(calc.Avg == 50.04, "Average is off");
            Assert.That(calc.Min == 0, "Min is off");
            Assert.That(calc.Max == 100, "Max is off");
            Assert.That(calc.Current == 97.0, "Current value is  off");

            _factory.Verify(f => f.GetLatestCPUPollAsync(), Times.Once(),"factory method ran more than once");

        }



    }
}
