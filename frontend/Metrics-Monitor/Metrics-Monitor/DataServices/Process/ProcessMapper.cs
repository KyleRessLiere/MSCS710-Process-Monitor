using MetricsMonitorClient.DataServices.Process.Dtos;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetricsMonitorClient.DataServices.Process {
    public static class ProcessMapper {



        public static ProcessPoll ToModel(this ProcessDto source) {
            ProcessPoll target = new ProcessPoll();
            target.CpuPercent = source.cpu_percent;
            target.MemoryUsage = source.memory_usage;
            target.ThreadNumber = source.num_thread;
            target.PollId = source.poll_id;
            target.Id = source.process_id;
            target.ProcessName = source.process_name;
            target.ProcessStatus = ProcessStatNameToId(source._process_status);

            return target;

        }


        public static int ProcessStatNameToId(string status) {
            if(status == null) return MMConstants.ProcessStatusId_Unknown;
            
           return MMConstants.ProcessStatus_NameToId_Map.TryGetValue(status, out var procStatus) ? procStatus : MMConstants.ProcessStatusId_Unknown;
        }

        public static string ProcessStatIdToName(int id) => MMConstants.ProcessStatus_IdToName_Map.TryGetValue(id, out var procStatus) ? procStatus : MMConstants.ProcessStatus_Unknown;



    }
}
