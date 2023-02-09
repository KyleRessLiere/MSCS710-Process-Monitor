#!/usr/bin/env python
import psutil
import time
from subprocess import call
from prettytable import PrettyTable
import json



def getRunningProcesses():
    # List of current running process IDs.
    proc = []
    # get the pids from last 200 which mostly are user processes
    for pid in psutil.pids()[-200:]:
        try:
            p = psutil.Process(pid)
            # trigger cpu_percent() the first time which leads to return of 0.0
            p.cpu_percent()
            proc.append(p)

        except Exception as e:
            pass

    # sort by cpu_percent
    top = {}
    time.sleep(0.1)
    for p in proc:
        # trigger cpu_percent() the second time for measurement
        try:
            top[p] = p.cpu_percent() / psutil.cpu_count()
        except Exception as e:
            top[p] = -1
            print(e)
            print("pid gone before reached")

    top_list = sorted(top.items(), key=lambda x: x[1])
    top10 = top_list
    top10.reverse()

    processList = []
    for p, cpu_percent in top10:

        # While fetching the processes, some of the subprocesses may exit
        # Hence we need to put this code in try-except block
        try:
            # oneshot to improve info retrieve efficiency
            with p.oneshot():
                processList.append({"pid":p.pid
                                    ,"process_name":p.name(),
                                    "status":str(p.status()),
                                    "cpu_percent": f'{cpu_percent:.2f}',
                                    "num_thread":p.num_threads(),
                                    "memory_mb": f'{p.memory_info().rss / 1e6:.3f}'
                                    })
        except Exception as e:
            pass

    return processList

"""
TODO:add cpu frequency
https://psutil.readthedocs.io/en/latest/#psutil.cpu_freq
"""
def getCPUStats():
    p = psutil
    print(psutil.cpu_stats())
 
    cpuPercentageByCore = p.cpu_percent(interval=1, percpu=True)
    cpu ={
        "cpuSumPercentage":p.cpu_percent(interval=1),
        "cpuPercentageByCore" : cpuPercentageByCore,
        "cpu_load_average": [x / psutil.cpu_count() * 100 for x in psutil.getloadavg()],
        "cpu_count_virtual":  psutil.cpu_count(),
        "cpu_count_physical": psutil.cpu_count(logical="False"),
        "cpu_ctx_switches":psutil.cpu_stats()[0],
        "interrupts":psutil.cpu_stats()[1],
        "soft_interrupts": psutil.cpu_stats()[2],
        "syscalls": psutil.cpu_stats()[3]
        
    }
    return cpu

def getMemoryStats():
    vm = psutil.virtual_memory()
    memory = {
        "total_memory":f'{vm.total / 1e9:.3f}',
        "used_memory":f'{vm.used / 1e9:.3f}',
        "available_memory":f'{vm.available / 1e9:.3f}',
        "percentage_memory": vm.percent
    }
    return memory

def getNetworkStats():
    networkList = []
    for key in psutil.net_if_stats().keys():
        networkList.append({
        "network":  key,
        "status":"Up" if psutil.net_if_stats()[key].isup else "Down",
        "speed": psutil.net_if_stats()[key].speed
    }  
        )
    return networkList


"""
TODO:add other physical stats such as temperature and fan speed
https://psutil.readthedocs.io/en/latest/#disks
"""
def getBatteryStats():

    battery = psutil.sensors_battery()
    batteryStats = {
        "battery_percent":psutil.sensors_battery().percent,
        "estimated_battery_seconds_remaining":battery.secsleft
    }
  
    return batteryStats
"""
TODO: ADD DISK PARTITIONS, maybe IO counters
https://psutil.readthedocs.io/en/latest/#disks
"""
def getDiskStats():
    """
    Gather disk stats 
    :stats include disk_total,disk_used,disk_free,disk_percent
    :return dict diskStat 

    """
    disk = psutil.disk_usage('/')
    diskStats = {
        "disk_total":disk.total,
        "disk_used":disk.used,
        "disk_free":disk.free,
        "disk_percent":disk.percent
    }
    return diskStats


def poll_system():
    """
    Gather info about system stats 
    Stats include:Memory,processes,network,disk,battery,cpu
    :return dict poll 
    """
    poll = {
        "process":getRunningProcesses(),
        "disk":getDiskStats(),
        "network":getNetworkStats(),
        "memory":getMemoryStats(),
        "battery":getBatteryStats(),
        "cpu":getCPUStats(),
    }
    return poll

#print(getBatteryStats())

print(getCPUStats())