#!/usr/bin/env python
import pika
import psutil
import time
from subprocess import call
from prettytable import PrettyTable


def getRunningProcesses():
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
    top10 = str(top10)
    return top10

