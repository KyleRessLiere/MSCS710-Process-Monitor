#!/usr/bin/python

import sqlite3
from sqlite3 import Error

def get_disks():
    disks = []
    try:
        db_file = r"./db/MMM-SQLite.db"
        conn = sqlite3.connect(db_file)
        cur = conn.cursor()
        res = cur.execute("SELECT * FROM disks")
        disks = res.fetchall()
    except Error as e:
        print(e)
    finally:
        if conn:
            conn.close()
    return disks

def get_disk_by_id(disk_id):
    disk = {}
    try:
        db_file = r"./db/MMM-SQLite.db"
        conn = sqlite3.connect(db_file)
        cur = conn.cursor()
        res = cur.execute("SELECT * FROM memory WHERE memory_id = ?", (disk_id,))
        disk = res.fetchone()
    except Error as e:
        print(e)
    finally:
        if conn:
            conn.close()
    return disk