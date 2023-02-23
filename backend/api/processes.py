#!/usr/bin/python

import sqlite3
from sqlite3 import Error

def get_processes():
    process_list = []
    try:
        db_file = r"./db/MMM-SQLite.db"
        conn = sqlite3.connect(db_file)
        cur = conn.cursor()
        res = cur.execute("SELECT * FROM processes")
        processes = res.fetchall()
        for i in processes:
            process = {
                "poll_id": i[0],
                "process_id": i[1],
                "process_name": i[2],
                "process_status": i[3],
                "cpu_percent": i[4],
                "num_thread": i[5],
                "memory_usage": i[6]
            }
            process_list.append(process)
    except Error as e:
        print(e)
    finally:
        if conn:
            conn.close()
    return process_list

def get_processes_by_id(process_id):
    process_list = []
    try:
        db_file = r"./db/MMM-SQLite.db"
        conn = sqlite3.connect(db_file)
        cur = conn.cursor()
        res = cur.execute("SELECT * FROM processes WHERE process_id = ?", (process_id,))
        processes = res.fetchall()
        for i in processes:
            process = {
                    "poll_id": i[0],
                    "process_id": i[1],
                    "process_name": i[2],
                    "process_status": i[3],
                    "cpu_percent": i[4],
                    "num_thread": i[5],
                    "memory_usage": i[6]
                }
            process_list.append(process)
    except Error as e:
        print(e)
    finally:
        if conn:
            conn.close()
    return process_list