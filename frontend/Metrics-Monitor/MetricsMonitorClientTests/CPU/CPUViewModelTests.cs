using LiveChartsCore;
using MetricsMonitorClient;
using MetricsMonitorClient.DataServices.CPU;
using MetricsMonitorClient.DataServices.CPU.Dtos;
using MetricsMonitorClient.DataServices.Memory;
using MetricsMonitorClient.DataServices.Memory.Dtos;
using MetricsMonitorClient.ViewModels;
using Moq;
using NP.Concepts.Behaviors;
using NP.Utilities;
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
       

    }
}
