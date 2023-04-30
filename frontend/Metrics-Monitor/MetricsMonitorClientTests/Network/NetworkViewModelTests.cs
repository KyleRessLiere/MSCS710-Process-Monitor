using DynamicData;
using log4net;
using MetricsMonitorClient;
using MetricsMonitorClient.DataServices.Network;
using MetricsMonitorClient.Models.Network;
using MetricsMonitorClient.ViewModels;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetricsMonitorClientTests.Network
{
    public class NetworkViewModelTests {
        private readonly Mock<INetworkFactory> _factory;
        private readonly Mock<ILog> _logger;

        public NetworkViewModelTests() {
            _factory = new Mock<INetworkFactory>();
            _logger = new Mock<ILog>();
        }

        [TearDown]
        public void TearDown() {
            _factory.Reset();
            _logger.Reset();
        }



        [Test]
        public void NetworkViewModel_ShouldCorrectlyOrganizeStats_ByInterface() {
            var netPolls = new List<NetworkPoll> {
                new NetworkPoll { Id = 1, Interface = "a", Speed = 100, Status = MMConstants.NetworkStatus_Up_Id, PollId = 1 },
                new NetworkPoll { Id = 2, Interface = "b", Speed = 10, Status = MMConstants.NetworkStatus_Up_Id, PollId = 1 },
                new NetworkPoll { Id = 3, Interface = "c", Speed = 10, Status = MMConstants.NetworkStatus_Up_Id, PollId = 1 },
                new NetworkPoll { Id = 4, Interface = "d", Speed = 10, Status = MMConstants.NetworkStatus_Up_Id, PollId = 1 },
                new NetworkPoll { Id = 5, Interface = "e", Speed = 10, Status = MMConstants.NetworkStatus_Up_Id, PollId = 1 }
            };

            _factory.Setup(f => f.GetLatestNetworkPollAsync()).ReturnsAsync(netPolls);

            var vm = new NetworkViewModel(_factory.Object, _logger.Object);

            vm.TickClock();

            var netPolls1 = new List<NetworkPoll> {
                new NetworkPoll { Id = 6, Interface = "c", Speed = 10, Status = MMConstants.NetworkStatus_Up_Id, PollId = 2 },
                new NetworkPoll { Id = 7, Interface = "e", Speed = 10, Status = MMConstants.NetworkStatus_Up_Id, PollId = 2 },
                new NetworkPoll { Id = 8, Interface = "b", Speed = 10, Status = MMConstants.NetworkStatus_Up_Id, PollId = 2 },
                new NetworkPoll { Id = 9, Interface = "d", Speed = 10, Status = MMConstants.NetworkStatus_Up_Id, PollId = 2 },
                new NetworkPoll { Id = 10, Interface = "a", Speed = 100, Status = MMConstants.NetworkStatus_Up_Id, PollId = 2 }
            };

            vm.UpdateDataSets(netPolls1);

            Assert.That(vm.StatsContainers.Count, Is.EqualTo(5), "Incorrect number of stats containers");
            var aContainer = vm.StatsContainers.Where(sc => sc.Name == "a").FirstOrDefault();

            Assert.That(aContainer.Avg, Is.EqualTo(100), "VM was not able to correctly organize network data by interface.");
            
        }


        [Test]
        public void NetworkViewModel_ShouldInsertZero_WhenInterfaceMissing() {
            var netPolls = new List<NetworkPoll> {
                new NetworkPoll { Id = 1, Interface = "a", Speed = 100, Status = MMConstants.NetworkStatus_Up_Id, PollId = 1 },
                new NetworkPoll { Id = 2, Interface = "b", Speed = 10, Status = MMConstants.NetworkStatus_Up_Id, PollId = 1 },
                new NetworkPoll { Id = 3, Interface = "c", Speed = 10, Status = MMConstants.NetworkStatus_Up_Id, PollId = 1 },
                new NetworkPoll { Id = 4, Interface = "d", Speed = 10, Status = MMConstants.NetworkStatus_Up_Id, PollId = 1 },
                new NetworkPoll { Id = 5, Interface = "e", Speed = 10, Status = MMConstants.NetworkStatus_Up_Id, PollId = 1 }
            };

            _factory.Setup(f => f.GetLatestNetworkPollAsync()).ReturnsAsync(netPolls);

            var vm = new NetworkViewModel(_factory.Object, _logger.Object);

            vm.TickClock();

            var netPolls1 = new List<NetworkPoll> {
                new NetworkPoll { Id = 6, Interface = "c", Speed = 10, Status = MMConstants.NetworkStatus_Up_Id, PollId = 2 },
                new NetworkPoll { Id = 7, Interface = "e", Speed = 10, Status = MMConstants.NetworkStatus_Up_Id, PollId = 2 },
                new NetworkPoll { Id = 9, Interface = "d", Speed = 10, Status = MMConstants.NetworkStatus_Up_Id, PollId = 2 },
                new NetworkPoll { Id = 10, Interface = "a", Speed = 100, Status = MMConstants.NetworkStatus_Up_Id, PollId = 2 }
            };

            vm.UpdateDataSets(netPolls1);

            Assert.That(vm.StatsContainers.Count, Is.EqualTo(5));

            var bContainer = vm.StatsContainers.Where(sc => sc.Name == "b").FirstOrDefault();

            Assert.That(bContainer.Status == MMConstants.NetworkStatus_Unknown, "didnt set the status to unkown");
            Assert.That(bContainer.Current == 0, "didnt set the current to 0");
        }
    }
}
