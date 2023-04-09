using MetricsMonitorClient;
using MetricsMonitorClient.DataServices.CPU;
using MetricsMonitorClient.DataServices.CPU.Dtos;
using MetricsMonitorClient.ViewModels;
using Moq;
using NP.Concepts.Behaviors;
using ILog = log4net.ILog;
namespace MetricsMonitorClientTests.ViewModels {
    public class CPUViewModelTests {
        private readonly Mock<ICPUFactory> _factory;
        private readonly Mock<ILog> _logger;
        public CPUViewModelTests() { //initialize mocks
            _factory = new Mock<ICPUFactory>();
            _logger = new Mock<ILog>();
        }

        [TearDown]
        public void TearDown() { //clean up setups
            _factory.Reset();
            _logger.Reset();
        }

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
            double[] usagePcts = new double[] { 96.0, 5.0, 61.0, 18.0, 99.0, 35.0, 20.0, 74.0, 68.0, 93.0, 88.0, 25.0, 87.0, 13.0, 22.0, 88.0, 9.0, 72.0, 4.0, 11.0,
                48.0, 5.0, 93.0, 2.0, 25.0, 15.0, 14.0, 96.0, 9.0, 38.0, 29.0, 62.0, 86.0, 38.0, 37.0, 89.0, 13.0, 84.0, 86.0, 71.0, 8.0, 86.0, 40.0, 7.0, 72.0, 43.0,
                83.0, 83.0, 79.0, 68.0, 47.0, 44.0, 27.0, 89.0, 79.0, 12.0, 69.0, 62.0, 93.0, 31.0, 44.0, 22.0, 89.0, 21.0, 23.0, 5.0, 94.0, 81.0, 5.0, 79.0, 72.0, 30.0,
                0.0, 63.0, 14.0, 45.0, 61.0, 38.0, 38.0, 53.0, 70.0, 70.0, 94.0, 5.0, 6.0, 89.0, 77.0, 78.0, 24.0, 87.0, 25.0, 62.0, 77.0, 70.0, 77.0, 44.0, 99.0, 26.0, 75.0,
                23.0 };
            //vm.StatsContainers.Add(new CpuStatsContainer());
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

            _factory.Verify(f => f.GetLatestCPUPollAsync(), Times.Once(), "factory method ran more than once");

            Assert.That(calc.FirstQ == 22.75, "25th percentile value is off");
            Assert.That(calc.SecondQ == 50.5, "50th percentile value is off");
            Assert.That(calc.ThirdQ == 79, "75th percentile value is off");
            Assert.That(calc.Avg == 51.05, "Average is off");
            Assert.That(calc.Min == 0, "Min is off");
            Assert.That(calc.Max == 99, "Max is off");
            Assert.That(calc.Current == 23.0, "Current value is  off");

           

        }



    }
}
