import sqlite3
from sqlite3 import Error
import os
import platform
import datetime
import time


def create_connection(db_file):
    """ create a database connection to the SQLite database """
    conn = None
    try:
        conn = sqlite3.connect(db_file)
        return conn
    except Error as e:
        print(e)

    return conn


def insert_poll(conn, poll):
    """
    Log a new poll to polls table
    :param conn:
    :param poll:
    :return: poll id
    """
    sql = ''' INSERT INTO polls(poll_rate, operating_system, operating_system_version, poll_type, time)
              VALUES(?,?,?,?,?) '''
    cur = conn.cursor()
    cur.execute(sql, poll)
    conn.commit()
    return cur.lastrowid


def insert_network(conn, network):
    """
    insert network details for poll
    :param conn:
    :param task:
    :return:network_id
    """

    sql = ''' INSERT INTO network(poll_id, network_interface, network_status, network_speed)
              VALUES(?,?,?,?) '''
    cur = conn.cursor()
    cur.execute(sql, network)
    conn.commit()
    return cur.lastrowid


def insert_disk(conn, disk):
    """
    Log a disk info to disk table
    :param conn:
    :param disk:
    :return: disk id
    """
    sql = ''' INSERT INTO disks(poll_id, disk_total, disk_used, disk_free, disk_percentage)
              VALUES(?,?,?,?,?) '''
    cur = conn.cursor()
    cur.execute(sql, disk)
    conn.commit()
    return cur.lastrowid

def insert_process(conn, process):
    """
    Create a new process
    :param conn:
    :param task:
    :return:
    """

    sql = ''' INSERT INTO processes(poll_id,process_id,process_name,process_status,cpu_percent,num_thread,memory_usage)
              VALUES(?,?,?,?,?,?,?) '''
    cur = conn.cursor()
    cur.execute(sql, process)
    conn.commit()
    return cur.lastrowid


def insert_memory(conn, memory):
    """
    Log a disk info to disk table
    :param conn:
    :param memory:
    :return: memory id
    """
    sql = ''' INSERT INTO memory(poll_id, total_memory, available_memory, used_memory, percentage_used)
              VALUES(?,?,?,?,?) '''
    cur = conn.cursor()
    cur.execute(sql, memory)
    conn.commit()
    return cur.lastrowid

"""
TODO:get more cpu stats then implement
"""
def insert_cpu(conn, cpu):
    """
    Log a disk info to disk table
    :param conn:
    :param disk:
    :return: disk id
    """
    sql = ''' INSERT INTO cpu(poll_id, cpu_percent, cpu_percentage_by_core, cpu_load_average, cpu_count_virtual, cpu_count_physical, cpu_ctx_switches, interrupts, soft_interrupts, syscalls)
              VALUES(?,?,?,?,?,?,?,?,?,?) '''
    cur = conn.cursor()
    cur.execute(sql, cpu)
    conn.commit()
    return cur.lastrowid


def main_poll(poll,polling_rate, poll_type):
    """
    Stores polled information into database
    :param poll:
    :param polling rate
    """
    #database path
    database = r"./db/MMM-SQLite.db"

    # create a database connection
    conn = create_connection(database)
    # with conn:
        # log a poll


    """
    INSERT poll_data
    """
    ts = time.time()
    timestamp = datetime.datetime.fromtimestamp(ts).strftime('%Y-%m-%d %H:%M:%S')
    os = ("Mac OS X" if platform.system() == "Darwin" else platform.system())
    os_version = platform.release()
    poll_data = (polling_rate, os,os_version,poll_type, timestamp)
    poll_id = insert_poll(conn, poll_data)

    """
    INSERT disk
    """
    disk = poll["disk"]
    disk_data = (poll_id,disk["disk_total"],disk["disk_used"],disk["disk_free"],disk["disk_percent"])
    insert_disk(conn, disk_data)

    """
    INSERT Memory
    """
    memory = poll["memory"]
    memory_data = (poll_id,memory["total_memory"], memory["used_memory"], memory["available_memory"], memory["percentage_memory"])
    insert_memory(conn, memory_data)

    """
    INSERT network
    """
    network_list = poll["network"]
    for n in network_list:
        network_data = (poll_id,n["network"], n["status"], n["speed"])
        insert_network(conn, network_data)

    """
    INSERT CPU
    TODO:get more cpu info
    """
    cpu = poll["cpu"]
    cpu_data = ( poll_id,cpu["cpu_sum_percentage"], str(cpu["cpu_percentage_by_core"]),   str(cpu["cpu_load_average"]),  cpu["cpu_count_virtual"],  cpu["cpu_count_physical"],  cpu["cpu_ctx_switches"],cpu["interrupts"],   cpu["soft_interrupts"], cpu["syscalls"] )
    insert_cpu(conn, cpu_data)

    """
    INSERT process
    """
    process_list = poll["process"]
    for p in process_list:
        process_data = (poll_id,p["pid"],p["process_name"],p["status"],p["cpu_percent"],p["num_thread"],p["memory_mb"])
        insert_process(conn, process_data)
        #print("Poll ID: {}, and Process ID: {} have been logged in SQLite DB".format(poll_id, p["pid"]))


test_poll = {
  "process": [
    {
      "pid": 98894,
      "process_name": "trustd",
      "status": "running",
      "cpu_percent": "0.00",
      "num_thread": 2,
      "memory_mb": "15.491"
    }
  ],
  "disk": {
    "disk_total": 1000240963584,
    "disk_used": 11682930688,
    "disk_free": 364035297280,
    "disk_percent": 3.1
  },
  "network": [
    {
      "network": "lo0",
      "status": "Up",
      "speed": 0
    },
    {
      "network": "gif0",
      "status": "Down",
      "speed": 0
    },
    {
      "network": "stf0",
      "status": "Down",
      "speed": 0
    }
  ],
  "memory": {
    "total_memory": "17.180",
    "used_memory": "9.984",
    "available_memory": "6.075",
    "percentage_memory": 64.6
  },
  "battery": {
    "battery_percent": 39,
    "estimated_battery_seconds_remaining": 8280
  },
  "cpu": {
    "cpu_sum_percentage": 6.5,
    "cpu_percentage_by_core": [22.8, 0.0, 12.2, 1.0, 12.9, 0.0, 6.0, 0.0],
    "cpu_load_average": [80.28564453125, 81.5185546875, 153.01513671875],
    "cpu_count_virtual": 4,
    "cpu_count_physical": 4,
    "cpu_ctx_switches": 6007,
    "interrupts": 951412,
    "soft_interrupts": 1465898952,
    "syscalls": 1480147
  }
}

#main_poll(test_poll,1)
