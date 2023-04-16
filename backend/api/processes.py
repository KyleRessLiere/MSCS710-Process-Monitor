#!/usr/bin/python

import sqlite3
from sqlite3 import Error
from flask import abort

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

def get_latest_processes():
    process_list = []
    try:
        db_file = r"./db/MMM-SQLite.db"
        conn = sqlite3.connect(db_file)
        cur = conn.cursor()
        res = cur.execute("SELECT * FROM processes WHERE poll_id = (SELECT MAX(poll_id) FROM processes) ORDER BY process_id ASC")
        processes = res.fetchall()
        for i in processes:
            latest_process = {
                "poll_id": i[0],
                "process_id": i[1],
                "process_name": i[2],
                "process_status": i[3],
                "cpu_percent": i[4],
                "num_thread": i[5],
                "memory_usage": i[6]
            }
            process_list.append(latest_process)
    except Error as e:
        print(e)
    finally:
        if conn:
            conn.close()
    return process_list

def get_latest_process_by_process_id(process_id):
    db_file = r"./db/MMM-SQLite.db"
    with sqlite3.connect(db_file) as conn:
        cur = conn.cursor()
        res = cur.execute("SELECT * FROM processes WHERE process_id = ? ORDER BY poll_id DESC LIMIT 1", (process_id,))
        latest_process = res.fetchone()
        if latest_process:
            latest_process = {
                        "poll_id": latest_process[0],
                        "process_id": latest_process[1],
                        "process_name": latest_process[2],
                        "process_status": latest_process[3],
                        "cpu_percent": latest_process[4],
                        "num_thread": latest_process[5],
                        "memory_usage": latest_process[6]
                    }
        else:
            abort(404, description=f"Process ID {process_id} not found")
    return latest_process

def get_processes_by_process_id(process_id):
    process_list = []
    db_file = r"./db/MMM-SQLite.db"
    with sqlite3.connect(db_file) as conn:
        cur = conn.cursor()
        res = cur.execute("SELECT * FROM processes WHERE process_id = ?", (process_id,))
        processes = res.fetchall()
        if processes:
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
        else:
            abort(404, description=f"Process ID {process_id} not found")
    return process_list

def get_processes_by_poll_id(poll_id):
    process_list = []
    db_file = r"./db/MMM-SQLite.db"
    with sqlite3.connect(db_file) as conn:
        cur = conn.cursor()
        res = cur.execute("SELECT * FROM processes WHERE poll_id = ? ORDER BY process_id ASC", (poll_id,))
        processes = res.fetchall()
        if processes:
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
        else:
            abort(404, description=f"Process ID {poll_id} not found")
    return process_list
