#!/usr/bin/python

import sqlite3
from sqlite3 import Error

def get_network():
    network = []
    try:
        db_file = r"./db/MMM-SQLite.db"
        conn = sqlite3.connect(db_file)
        cur = conn.cursor()
        res = cur.execute("SELECT * FROM network")
        network = res.fetchall()
    except Error as e:
        print(e)
    finally:
        if conn:
            conn.close()
    return network

def get_network_by_id(network_id):
    network = {}
    try:
        db_file = r"./db/MMM-SQLite.db"
        conn = sqlite3.connect(db_file)
        cur = conn.cursor()
        res = cur.execute("SELECT * FROM network WHERE network_id = ?", (network_id,))
        network = res.fetchone()
    except Error as e:
        print(e)
    finally:
        if conn:
            conn.close()
    return network