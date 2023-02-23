#!/usr/bin/python

import sqlite3
from sqlite3 import Error

def get_network():
    network_list = []
    try:
        db_file = r"./db/MMM-SQLite.db"
        conn = sqlite3.connect(db_file)
        cur = conn.cursor()
        res = cur.execute("SELECT * FROM network")
        networks = res.fetchall()
        for i in networks:
            network = {
                "network_id": i[0],
                "poll_id": i[1],
                "network_interface": i[2],
                "network_status": i[3],
                "network_speed": i[4]
            }
            network_list.append(network)
    except Error as e:
        print(e)
    finally:
        if conn:
            conn.close()
    return network_list

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