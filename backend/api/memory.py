#!/usr/bin/python

import sqlite3
from sqlite3 import Error

def get_memory():
    memory_list = []
    try:
        db_file = r"./db/MMM-SQLite.db"
        conn = sqlite3.connect(db_file)
        cur = conn.cursor()
        res = cur.execute("SELECT * FROM memory")
        memory = res.fetchall()
        for i in memory:
            mem_item = {
                "memory_id": i[0],
                "poll_id": i[1],
                "total_memory": i[2],
                "available_memory": i[3],
                "used_memory": i[4],
                "percentage_used": i[5]
            }
            memory_list.append(mem_item)
    except Error as e:
        print(e)
    finally:
        if conn:
            conn.close()
    return memory_list

def get_memory_by_memory_id(memory_id):
    memory = {}
    try:
        db_file = r"./db/MMM-SQLite.db"
        conn = sqlite3.connect(db_file)
        cur = conn.cursor()
        res = cur.execute("SELECT * FROM memory WHERE memory_id = ?", (memory_id,))
        memory = res.fetchone()
        memory = {
                "memory_id": memory[0],
                "poll_id": memory[1],
                "total_memory": memory[2],
                "available_memory": memory[3],
                "used_memory": memory[4],
                "percentage_used": memory[5]
            }
    except Error as e:
        print(e)
    finally:
        if conn:
            conn.close()
    return memory

def get_memory_by_poll_id(poll_id):
    memory = {}
    try:
        db_file = r"./db/MMM-SQLite.db"
        conn = sqlite3.connect(db_file)
        cur = conn.cursor()
        res = cur.execute("SELECT * FROM memory WHERE poll_id = ?", (poll_id,))
        memory = res.fetchone()
        memory = {
                "memory_id": memory[0],
                "poll_id": memory[1],
                "total_memory": memory[2],
                "available_memory": memory[3],
                "used_memory": memory[4],
                "percentage_used": memory[5]
            }
    except Error as e:
        print(e)
    finally:
        if conn:
            conn.close()
    return memory

def get_latest_memory():
    latest_memory = {}
    try:
        db_file = r"./db/MMM-SQLite.db"
        conn = sqlite3.connect(db_file)
        cur = conn.cursor()
        res = cur.execute("SELECT * FROM memory ORDER BY poll_id DESC LIMIT 1")
        latest_memory = res.fetchone()
        latest_memory = {
                "memory_id": latest_memory[0],
                "poll_id": latest_memory[1],
                "total_memory": latest_memory[2],
                "available_memory": latest_memory[3],
                "used_memory": latest_memory[4],
                "percentage_used": latest_memory[5]
            }
    except Error as e:
        print(e)
    finally:
        if conn:
            conn.close()
    return latest_memory