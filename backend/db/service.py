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
    :param poll:
    :return: poll id
    """
    sql = ''' INSERT INTO polls(poll_rate,operating_system,operating_system_version,time)
              VALUES(?,?,?,?) '''
    cur = conn.cursor()
    cur.execute(sql, poll)
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

