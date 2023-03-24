#!/usr/bin/python

import ast
import sqlite3
from sqlite3 import Error

def get_cpu():
    cpu_list = []
    try:
        db_file = r"./db/MMM-SQLite.db"
        conn = sqlite3.connect(db_file)
        cur = conn.cursor()
        res = cur.execute("SELECT * FROM cpu")
        cpu = res.fetchall()
        for i in cpu:
            cpu_item = {
                "cpu_id": i[0],
                "poll_id": i[1],
                "cpu_percent": i[2],
                "cpu_percentage_per_core": i[3],
                "cpu_count_virtual": i[4],
                "cpu_count_physical": i[5],
                "cpu_ctx_switches": i[6],
                "interrupts": i[7],
                "soft_interrupts": i[8],
                "syscalls": i[9]
            }
            cpu_item['cpu_percentage_per_core'] = ast.literal_eval(cpu_item['cpu_percentage_per_core'])
            cpu_item['cpu_count_virtual'] = ast.literal_eval(cpu_item['cpu_count_virtual'])
            cpu_list.append(cpu_item)
    except Error as e:
        print(e)
    finally:
        if conn:
            conn.close()
    return cpu_list

def get_cpu_by_cpu_id(cpu_id):
    cpu = {}
    try:
        db_file = r"./db/MMM-SQLite.db"
        conn = sqlite3.connect(db_file)
        cur = conn.cursor()
        res = cur.execute("SELECT * FROM cpu WHERE cpu_id = ?", (cpu_id,))
        cpu = res.fetchone()
        cpu_item = {
                "cpu_id": cpu[0],
                "poll_id": cpu[1],
                "cpu_percent": cpu[2],
                "cpu_percentage_per_core": cpu[3],
                "cpu_count_virtual": cpu[4],
                "cpu_count_physical": cpu[5],
                "cpu_ctx_switches": cpu[6],
                "interrupts": cpu[7],
                "soft_interrupts": cpu[8],
                "syscalls": cpu[9]
            }
        cpu_item['cpu_percentage_per_core'] = ast.literal_eval(cpu_item['cpu_percentage_per_core'])
        cpu_item['cpu_count_virtual'] = ast.literal_eval(cpu_item['cpu_count_virtual'])
    except Error as e:
        print(e)
    finally:
        if conn:
            conn.close()
    return cpu_item

def get_cpu_by_poll_id(poll_id):
    cpu = {}
    try:
        db_file = r"./db/MMM-SQLite.db"
        conn = sqlite3.connect(db_file)
        cur = conn.cursor()
        res = cur.execute("SELECT * FROM cpu WHERE poll_id = ?", (poll_id,))
        cpu = res.fetchone()
        cpu_item = {
                "cpu_id": cpu[0],
                "poll_id": cpu[1],
                "cpu_percent": cpu[2],
                "cpu_percentage_per_core": cpu[3],
                "cpu_count_virtual": cpu[4],
                "cpu_count_physical": cpu[5],
                "cpu_ctx_switches": cpu[6],
                "interrupts": cpu[7],
                "soft_interrupts": cpu[8],
                "syscalls": cpu[9]
            }
        cpu_item['cpu_percentage_per_core'] = ast.literal_eval(cpu_item['cpu_percentage_per_core'])
        cpu_item['cpu_count_virtual'] = ast.literal_eval(cpu_item['cpu_count_virtual'])
    except Error as e:
        print(e)
    finally:
        if conn:
            conn.close()
    return cpu_item

def get_latest_cpu():
    cpu_item = {}
    try:
        db_file = r"./db/MMM-SQLite.db"
        conn = sqlite3.connect(db_file)
        cur = conn.cursor()
        res = cur.execute("SELECT * FROM cpu ORDER BY poll_id DESC LIMIT 1")
        latest_cpu = res.fetchone()
        cpu_item = {
                "cpu_id": latest_cpu[0],
                "poll_id": latest_cpu[1],
                "cpu_percent": latest_cpu[2],
                "cpu_percentage_per_core": latest_cpu[3],
                "cpu_count_virtual": latest_cpu[4],
                "cpu_count_physical": latest_cpu[5],
                "cpu_ctx_switches": latest_cpu[6],
                "interrupts": latest_cpu[7],
                "soft_interrupts": latest_cpu[8],
                "syscalls": latest_cpu[9]
            }
        cpu_item['cpu_percentage_per_core'] = ast.literal_eval(cpu_item['cpu_percentage_per_core'])
        cpu_item['cpu_count_virtual'] = ast.literal_eval(cpu_item['cpu_count_virtual'])
    except Error as e:
        print(e)
    finally:
        if conn:
            conn.close()
    return cpu_item