#!/usr/bin/python

import sqlite3
from sqlite3 import Error

def get_polls():
    polls = []
    try:
        db_file = r"./db/MMM-SQLite.db"
        conn = sqlite3.connect(db_file)
        cur = conn.cursor()
        res = cur.execute("SELECT * FROM polls")
        polls = res.fetchall()
    except Error as e:
        print(e)
    finally:
        if conn:
            conn.close()
    return polls

def get_poll_by_id(poll_id):
    poll = {}
    try:
        db_file = r"./db/MMM-SQLite.db"
        conn = sqlite3.connect(db_file)
        cur = conn.cursor()
        res = cur.execute("SELECT * FROM polls WHERE poll_id = ?", (poll_id,))
        poll = res.fetchone()
    except Error as e:
        print(e)
    finally:
        if conn:
            conn.close()
    return poll