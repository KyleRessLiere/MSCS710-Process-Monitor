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
    sql = ''' INSERT INTO polls(poll_rate,operating_system,operating_system_version,time)
              VALUES(?,?,?,?) '''
    cur = conn.cursor()
    cur.execute(sql, poll)
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


def main_poll(poll,polling_rate):
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
    poll_data = (polling_rate, os,os_version, timestamp)
    poll_id = insert_poll(conn, poll_data)

    """
    INSERT disk
    """
    disk = poll["disk"]
    
    disk_data = (poll_id,disk["disk_total"],disk["disk_used"],disk["disk_free"],disk["disk_percent"])
    #insert_disk(conn, disk_data)

    
    """
    INSERT process
    """
    process_list = poll["process"]

    for p in process_list:
        process_data = (poll_id,p["pid"],p["process_name"],p["status"],p["cpu_percent"],p["num_thread"],p["memory_mb"])
        insert_process(conn, process_data)
        #print("Poll ID: {}, and Process ID: {} have been logged in SQLite DB".format(poll_id, p["pid"]))

    """

    """   

poll = {
  "process": [
    {
      "pid": 98895,
      "process_name": "trustd",
      "status": "running",
      "cpu_percent": "0.03",
      "num_thread": 2,
      "memory_mb": "13.771"
    },
    {
      "pid": 98895,
      "process_name": "docker",
      "status": "running",
      "cpu_percent": "0.00",
      "num_thread": 14,
      "memory_mb": "8.012"
    }
  ],
  "disk": {
    "disk_total": 1000240963584,
    "disk_used": 11682930688,
    "disk_free": 363153088512,
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
      "network": "utun5",
      "status": "Up",
      "speed": 0
    },
    {
      "network": "utun6",
      "status": "Up",
      "speed": 0
    }
  ],
  "memory": {
    "total_memory": "17.180",
    "used_memory": "10.114",
    "available": "6.198",
    "percentage": 63.9
  },
  "battery": {
    "battery_percent": 78,
    "estimated_battery_seconds_remaining": 14760
  },
  "cpuStats": {
    "cpuSumPercentage": 16.0,
    "cpuPercentageByCore": [51.0, 12.0, 46.0, 9.0, 44.0, 10.0, 35.0, 10.1]
  }
}
#main_poll(poll,1)
