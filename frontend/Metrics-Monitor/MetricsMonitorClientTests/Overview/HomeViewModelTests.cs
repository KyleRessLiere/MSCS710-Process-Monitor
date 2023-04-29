using MetricsMonitorClient.DataServices.Storage.Dtos;
using MetricsMonitorClient.DataServices.Storage;
using MetricsMonitorClient.ViewModels;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MetricsMonitorClient.DataServices.MonitorSystem;
using log4net;
using MetricsMonitorClient;
using MetricsMonitorClient.Models.Overview;
using Org.BouncyCastle.Asn1.BC;
using MetricsMonitorClient.DataServices.CPU.Dtos;
using MetricsMonitorClient.DataServices.Process.Dtos;

namespace MetricsMonitorClientTests.Overview
{
    public class HomeViewModelTests {
        private readonly Mock<IMonitorSystemFactory> _factory;
        private readonly Mock<ILog> _logger;

        public HomeViewModelTests() {
            _factory = new Mock<IMonitorSystemFactory>();
            _logger = new Mock<ILog>();
        }

        [SetUp]
        public void SetUp() {
            _factory.Reset();
            _logger.Reset();
        }
        #region Chart Container Tests
        [Test]
        public void ChartContainer_Update_ShouldLimitValues_ToBufferSize() {
            var chart = new ChartContainer("testGraph", "amount", "time");

            for (int i = 1; i <= (ChartContainer.BufferSize * 10); i++) {
                chart.Update((i * .01));
            }

            Assert.That(chart.Values.Count, Is.EqualTo(ChartContainer.BufferSize));
        }


        [Test]
        public void ChartContainer_Update_ShouldSetYMax_To_120Pct_Of_MaxValueInBuffer() {
            var chart = new ChartContainer("testGraph", "amount", "time");

            for (int i = (ChartContainer.BufferSize * 10); i > 0; i--) {
                
                chart.Update((i * .01));
            }

            double expectedMax = ChartContainer.BufferSize * .012;

            Assert.That(chart.YAxis[0].MaxLimit, Is.EqualTo(expectedMax));
        }
        #endregion

        [Test]
        public void HomeViewModel_GetProcessSummaryList_GroupsProessesByName() {


            var procList = new List<ProcessDto>();
            var pNameStr = "a,b,c,d,e,f,g,h,i,j,k,l,m,n,o,p,q,r,s";
            var pNames = pNameStr.Split(',');
            for(int i = 189;i > 0; i--) {
                
                int pNameIdx = (int)Math.Ceiling((double)(i / 10));

                var proc = new ProcessDto {
                    cpu_percent = i * .01,
                    memory_usage = i * .02,
                    num_thread = 2,
                    process_name = pNames[pNameIdx]
                };
                procList.Add(proc);
            }

            var subNameList = pNames.TakeLast(MMConstants.PollBufferSize).ToHashSet();

            var results = HomeViewModel.GetProcessSummaryList(procList).ToList();

            Assert.That(results.Count, Is.EqualTo(MMConstants.PollBufferSize), "returned an unexpected number of records");

            Assert.That(results.All(r => subNameList.Contains(r.ProcessName)), "there are records being returned that are not expected");

            Assert.That(results.DistinctBy(r => r.ProcessName).Count() == MMConstants.PollBufferSize, "there are duplicate records being returned");



            var expectedCpuValDict = new Dictionary<string, double>();
            var expectedRamValDict = new Dictionary<string, double>();
          
            foreach(var result in results) {
                var pName  = result.ProcessName;

                var cpuTotal = procList.Where(p => p.process_name ==  pName).Select(p => p.cpu_percent).Sum();
                var ramTotal = procList.Where(p => p.process_name ==  pName).Select(p => p.memory_usage).Sum();

                expectedCpuValDict[pName] = cpuTotal;
                expectedRamValDict[pName] = ramTotal;
            }

            var cpuValsMatch = results.All(r => r.CpuUsagePctTotal == expectedCpuValDict[r.ProcessName]);
            
            var ramValsMatch = results.All(r => r.MemoryUsagePctTotal == expectedRamValDict[r.ProcessName]);

            Assert.That(cpuValsMatch, "Not all cpu usage values were added corectly");
           
            Assert.That(ramValsMatch, "Not all memory usage values were added corectly");


        }



    }
}
