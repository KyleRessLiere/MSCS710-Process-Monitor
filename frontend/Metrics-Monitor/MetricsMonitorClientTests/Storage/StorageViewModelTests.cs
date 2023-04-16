using JetBrains.Annotations;
using log4net;
using MetricsMonitorClient;
using MetricsMonitorClient.DataServices.Storage;
using MetricsMonitorClient.DataServices.Storage.Dtos;
using MetricsMonitorClient.ViewModels;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetricsMonitorClientTests.Storage {
    public  class StorageViewModelTests {
        private readonly Mock<IStorageFactory> _factory;
        private readonly Mock<ILog> _logger;
        
        public StorageViewModelTests() {
            _factory = new Mock<IStorageFactory>();
            _logger = new Mock<ILog>();
        }

        [SetUp]
        public void SetUp() {
            _factory.Reset();
            _logger.Reset();
        }

        [Test]
        public void StorageViewModel_FormatsAmountsInGb() {

            var dto = new StorageDto { disk_free = 4254000000, disk_percentage = 22, disk_total = 10000000000, disk_used = 5746000000};
            _factory.Setup(f => f.GetLatestStoragePollAsync()).ReturnsAsync(dto);

            var vm = new StorageViewModel(_factory.Object, _logger.Object);

            vm.Refresh();

            Assert.That(vm.DiskTotal == 10, "Disk total wrong");
            Assert.That(vm.DiskUsed.Value == 5.746, "disk used value wrong");
            Assert.That(vm.DiskFree.Value == 4.254, "disk free value wrong");
        }




    }
}
