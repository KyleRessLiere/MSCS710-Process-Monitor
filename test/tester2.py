# Import the required libraries
import psutil
import time
from subprocess import call
from prettytable import PrettyTable

# Run an infinite loop to constantly monitor the system


# Clear the screen using a bash command
call('clear')

print("==============================Process Monitor\
======================================")

# Fetch the battery information
battery = psutil.sensors_battery().percent
print("----Battery Available: %d " % (battery,) + "%")

# We have used PrettyTable to print the data on console.
# t = PrettyTable(<list of headings>)
# t.add_row(<list of cells in row>)

# Fetch the Network information
print("----Networks----")
table = PrettyTable(['Network', 'Status', 'Speed'])
for key in psutil.net_if_stats().keys():
    name = key
    up = "Up" if psutil.net_if_stats()[key].isup else "Down"
    speed = psutil.net_if_stats()[key].speed
    table.add_row([name, up, speed])
print(table)

# Fetch the memory information
print("----Memory----")
memory_table = PrettyTable(["Total(GB)", "Used(GB)",
                            "Available(GB)", "Percentage"])
vm = psutil.virtual_memory()
memory_table.add_row([
    f'{vm.total / 1e9:.3f}',
    f'{vm.used / 1e9:.3f}',
    f'{vm.available / 1e9:.3f}',
    vm.percent
])
print(memory_table)





# Create a 1 second delay
time.sleep(1)
