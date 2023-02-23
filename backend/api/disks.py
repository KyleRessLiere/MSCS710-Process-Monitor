#!/usr/bin/python

import sqlite3
from sqlite3 import Error

def get_disks():
    disk_list = []
    try:
        db_file = r"./db/MMM-SQLite.db"
        conn = sqlite3.connect(db_file)
        cur = conn.cursor()
        res = cur.execute("SELECT * FROM disks")
        disks = res.fetchall()
        for i in disks:
            disk = {
                "disk_id": i[0],
                "poll_id": i[1],
                "disk_total": i[2],
                "disk_used": i[3],
                "disk_free": i[4],
                "disk_percentage": i[5]
            }
            disk_list.append(disk)
    except Error as e:
        print(e)
    finally:
        if conn:
            conn.close()
    return disk_list

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