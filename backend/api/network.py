#!/usr/bin/python

import sqlite3
from sqlite3 import Error
from flask import abort

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

def get_latest_network():
    network_list = []
    try:
        db_file = r"./db/MMM-SQLite.db"
        conn = sqlite3.connect(db_file)
        cur = conn.cursor()
        res = cur.execute("SELECT * FROM network WHERE poll_id = (SELECT MAX(poll_id) FROM network) ORDER BY network_id ASC")
        networks = res.fetchall()
        for i in networks:
            latest_network = {
                "network_id": i[0],
                "poll_id": i[1],
                "network_interface": i[2],
                "network_status": i[3],
                "network_speed": i[4]
            }
            network_list.append(latest_network)
    except Error as e:
        print(e)
    finally:
        if conn:
            conn.close()
    return network_list

def get_network_by_network_id(network_id):
    network = {}
    db_file = r"./db/MMM-SQLite.db"
    with sqlite3.connect(db_file) as conn:
        cur = conn.cursor()
        res = cur.execute("SELECT * FROM network WHERE network_id = ?", (network_id,))
        network = res.fetchone()
        if network:
            network = {
                    "network_id": network[0],
                    "poll_id": network[1],
                    "network_interface": network[2],
                    "network_status": network[3],
                    "network_speed": network[4]
                }
        else:
            abort(404, description=f"Network ID {network_id} not found")
    return network

def get_network_by_poll_id(poll_id):
    network = {}
    db_file = r"./db/MMM-SQLite.db"
    with sqlite3.connect(db_file) as conn:
        cur = conn.cursor()
        res = cur.execute("SELECT * FROM network WHERE poll_id = ?", (poll_id,))
        network = res.fetchone()
        if network:
            network = {
                    "network_id": network[0],
                    "poll_id": network[1],
                    "network_interface": network[2],
                    "network_status": network[3],
                    "network_speed": network[4]
                }
        else:
            abort(404, description=f"Poll ID {poll_id} not found")
    return network