#!/usr/bin/env python
import psutil
import time
from subprocess import call
from prettytable import PrettyTable
import json

def getRunningProcesses():
    # List of current running process IDs.
    proc = []
    # get the pids from last which mostly are user processes
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
        top[p] = p.cpu_percent() / psutil.cpu_count()

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
                processList.append({"pid":pid
                                    ,"process_name":p.name(),
                                    "status":str(p.status()),
                                    "cpu_percent": f'{cpu_percent:.2f}',
                                    "num_thread":p.num_threads(),
                                    "memory_mb": f'{p.memory_info().rss / 1e6:.3f}'
                                    })
                

        except Exception as e:
            pass

    for x in processList:
        print(x)

    return processList
"""
TODO:CPU stats
"""
def getCPUStats():
    p = psutil
    cpuPercentageByCore = p.cpu_percent(interval=None, percpu=True)
    cpuStats ={
        "cpuSumPercentage":p.cpu_percent(interval=None),
        "cpuPercentageByCore" : cpuPercentageByCore,
        
    }
    return cpuStats
def getMemoryStats():
    vm = psutil.virtual_memory()
    memory = {
        "total_memory":f'{vm.total / 1e9:.3f}',
        "used_memory":f'{vm.used / 1e9:.3f}',
        "available":f'{vm.available / 1e9:.3f}',
        "percentage": vm.percent
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
TODO:add other physcial stats such as temprature and fan speed
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
    disk = psutil.disk_usage('/')
    diskStats = {
        "disk_total":disk.total,
        "disk_used":disk.used,
        "disk_free":disk.free,
        "disk_percent":disk.percent
    }
    return diskStats

"""
Gather info about system stats and returns them together as a dictionary
Stats include:Memory,processes,network,disk,
"""
def poll_system():
    poll = {
        "process":getRunningProcesses(),
        "disk":getDiskStats(),
        "network":getNetworkStats(),
        "memory":getMemoryStats(),
        "battery":getBatteryStats(),
        "cpuStats":getCPUStats(),
    }
    return poll


print(json.dumps(poll_system(), indent=4, sort_keys=False))
