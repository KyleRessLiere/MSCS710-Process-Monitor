#!/usr/bin/python

import sqlite3
from sqlite3 import Error

def get_memory():
    memory = []
    try:
        db_file = r"./db/MMM-SQLite.db"
        conn = sqlite3.connect(db_file)
        cur = conn.cursor()
        res = cur.execute("SELECT * FROM memory")
        memory = res.fetchall()
    except Error as e:
        print(e)
    finally:
        if conn:
            conn.close()
    return memory

def get_memory_by_id(memory_id):
    memory = {}
    try:
        db_file = r"./db/MMM-SQLite.db"
        conn = sqlite3.connect(db_file)
        cur = conn.cursor()
        res = cur.execute("SELECT * FROM memory WHERE memory_id = ?", (memory_id,))
        memory = res.fetchone()
    except Error as e:
        print(e)
    finally:
        if conn:
            conn.close()
    return memory