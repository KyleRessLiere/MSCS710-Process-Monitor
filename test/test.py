import psutil
import datetime
import json
import time


def poll():
    ### *** CPU FUNCTIONS ***

    # Number of logical CPUs in the system
    print("CPU info")
    print(" _____________________________________________________________________________________________________________________________________________\n")

    print ("psutil.cpu_count() = {0}".format(psutil.cpu_count()))
    cpuStats = psutil.cpu_stats()
    print("\n _____________________________________________________________________________________________________________________________________________")



    ### *** DISK FUNCTIONS ***
    print("\n _____________________________________________________________________________________________________________________________________________")
    print("_____________________________________________________________________________________________________________________________________________ \n")

    print("Disk info")
    print("\n _____________________________________________________________________________________________________________________________________________")

    # List of named tuples containing all mounted disk partitions
    dparts = psutil.disk_partitions()
    print("psutil.disk_partitions() = {0}".format(dparts))

    # Disk usage statistics
    du = psutil.disk_usage('/')
    print("psutil.disk_usage('/') = {0}".format(du))


    ### *** MEMORY FUNCTIONS ***
    print("\n _____________________________________________________________________________________________________________________________________________")

    print("Memory info")
    print(" _____________________________________________________________________________________________________________________________________________")

    # System memory usage statistics
    mem = psutil.virtual_memory()
    print("psutil.virtual_memory() = {0}".format(mem))

    THRESHOLD = 100 * 1024 * 1024  # 100MB
    if mem.available <= THRESHOLD:
        print("warning, available memory below threshold")


    ### *** PROCESS FUNCTIONS ***
    print("\n _____________________________________________________________________________________________________________________________________________")

    print("Process info")
    print(" _____________________________________________________________________________________________________________________________________________")
    

    ### *** SYSTEM INFORMATION FUNCTIONS ***
    print("\n _____________________________________________________________________________________________________________________________________________")
    print("System info")

    print("\n _____________________________________________________________________________________________________________________________________________")

    # System boot time expressed in seconds since the epoch
    boot_time = psutil.boot_time()
    print("psutil.boot_time() = {0}".format(boot_time))

    # System boot time converted to human readable format
    print(datetime.datetime.fromtimestamp(psutil.boot_time()).strftime("%Y-%m-%d %H:%M:%S"))

    # Users currently connected on the system
    users = psutil.users()
    print("psutil.users() = {0}".format(users))


    ##
    poll = {
        'process_info' : processList,


    }
    print("POLLLING")

    return poll
      

poll();
