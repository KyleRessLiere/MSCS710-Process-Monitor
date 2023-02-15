#!/usr/bin/python

import sqlite3
from sqlite3 import Error
import json


def get_processes():
    processes = []
    try:
        db_file = r"./db/MMM-SQLite.db"
        conn = sqlite3.connect(db_file)
        cur = conn.cursor()

        res = cur.execute("SELECT * FROM processes")
        processes = res.fetchall()
        conn.close()

    except:
        processes = []

    return processes