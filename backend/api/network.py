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

def get_network_by_network_id(network_id):
    network = {}
    try:
        db_file = r"./db/MMM-SQLite.db"
        conn = sqlite3.connect(db_file)
        cur = conn.cursor()
        res = cur.execute("SELECT * FROM network WHERE network_id = ?", (network_id,))
        network = res.fetchone()
        network = {
                "network_id": network[0],
                "poll_id": network[1],
                "network_interface": network[2],
                "network_status": network[3],
                "network_speed": network[4]
            }
    except Error as e:
        print(e)
    finally:
        if conn:
            conn.close()
    return network

def get_network_by_poll_id(poll_id):
    network = {}
    try:
        db_file = r"./db/MMM-SQLite.db"
        conn = sqlite3.connect(db_file)
        cur = conn.cursor()
        res = cur.execute("SELECT * FROM network WHERE poll_id = ?", (poll_id,))
        network = res.fetchone()
        network = {
                "network_id": network[0],
                "poll_id": network[1],
                "network_interface": network[2],
                "network_status": network[3],
                "network_speed": network[4]
            }
    except Error as e:
        print(e)
    finally:
        if conn:
            conn.close()
    return network

def get_latest_network():
    latest_network = {}
    try:
        db_file = r"./db/MMM-SQLite.db"
        conn = sqlite3.connect(db_file)
        cur = conn.cursor()
        res = cur.execute("SELECT * FROM network ORDER BY poll_id DESC LIMIT 1")
        latest_network = res.fetchone()
        latest_network = {
                "network_id": latest_network[0],
                "poll_id": latest_network[1],
                "network_interface": latest_network[2],
                "network_status": latest_network[3],
                "network_speed": latest_network[4]
            }
    except Error as e:
        print(e)
    finally:
        if conn:
            conn.close()
    return latest_network