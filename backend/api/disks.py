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

def get_disk_by_disk_id(disk_id):
    disk = {}
    try:
        db_file = r"./db/MMM-SQLite.db"
        conn = sqlite3.connect(db_file)
        cur = conn.cursor()
        res = cur.execute("SELECT * FROM memory WHERE memory_id = ?", (disk_id,))
        disk = res.fetchone()
        disk = {
                "disk_id": disk[0],
                "poll_id": disk[1],
                "disk_total": disk[2],
                "disk_used": disk[3],
                "disk_free": disk[4],
                "disk_percentage": disk[5]
            }
    except Error as e:
        print(e)
    finally:
        if conn:
            conn.close()
    return disk

def get_disk_by_poll_id(poll_id):
    disk = {}
    try:
        db_file = r"./db/MMM-SQLite.db"
        conn = sqlite3.connect(db_file)
        cur = conn.cursor()
        res = cur.execute("SELECT * FROM memory WHERE poll_id = ?", (poll_id,))
        disk = res.fetchone()
        disk = {
                "disk_id": disk[0],
                "poll_id": disk[1],
                "disk_total": disk[2],
                "disk_used": disk[3],
                "disk_free": disk[4],
                "disk_percentage": disk[5]
            }
    except Error as e:
        print(e)
    finally:
        if conn:
            conn.close()
    return disk

def get_latest_disk():
    latest_disk = {}
    try:
        db_file = r"./db/MMM-SQLite.db"
        conn = sqlite3.connect(db_file)
        cur = conn.cursor()
        res = cur.execute("SELECT * FROM disks ORDER BY poll_id DESC LIMIT 1")
        latest_disk = res.fetchone()
        latest_disk = {
                "disk_id": latest_disk[0],
                "poll_id": latest_disk[1],
                "disk_total": latest_disk[2],
                "disk_used": latest_disk[3],
                "disk_free": latest_disk[4],
                "disk_percentage": latest_disk[5]
            }
    except Error as e:
        print(e)
    finally:
        if conn:
            conn.close()
    return latest_disk