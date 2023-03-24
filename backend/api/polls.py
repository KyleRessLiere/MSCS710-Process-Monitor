#!/usr/bin/python

import json
import sqlite3
from sqlite3 import Error

def get_polls():
    poll_list = []
    try:
        db_file = r"./db/MMM-SQLite.db"
        conn = sqlite3.connect(db_file)
        cur = conn.cursor()
        res = cur.execute("SELECT * FROM polls")
        polls = res.fetchall()
        for i in polls:
            poll = {
                "poll_id": i[0],
                "poll_rate": i[1],
                "operating_system": i[2],
                "time": i[3]
            }
            poll_list.append(poll)
    except Error as e:
        print(e)
    finally:
        if conn:
            conn.close()
    return poll_list

def get_poll_by_id(poll_id):
    poll = {}
    try:
        db_file = r"./db/MMM-SQLite.db"
        conn = sqlite3.connect(db_file)
        cur = conn.cursor()
        res = cur.execute("SELECT * FROM polls WHERE poll_id = ?", (poll_id,))
        poll = res.fetchone()
        poll = {
                "poll_id": poll[0],
                "poll_rate": poll[1],
                "operating_system": poll[2],
                "time": poll[3]
            }
    except Error as e:
        print(e)
    finally:
        if conn:
            conn.close()
    return poll

def get_latest_poll():
    latest_poll = {}
    try:
        db_file = r"./db/MMM-SQLite.db"
        conn = sqlite3.connect(db_file)
        cur = conn.cursor()
        res = cur.execute("SELECT * FROM polls ORDER BY poll_id DESC LIMIT 1")
        latest_poll = res.fetchone()
        latest_poll = {
                "poll_id": latest_poll[0],
                "poll_rate": latest_poll[1],
                "operating_system": latest_poll[2],
                "time": latest_poll[3]
            }
    except Error as e:
        print(e)
    finally:
        if conn:
            conn.close()
    return latest_poll