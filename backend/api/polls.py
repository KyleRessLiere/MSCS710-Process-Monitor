#!/usr/bin/python

import datetime
import sqlite3
from sqlite3 import Error
from flask import abort

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
                "time": datetime.datetime.strptime(latest_poll[3], '%Y-%m-%d %H:%M:%S').strftime('%Y-%m-%d %H:%M:%S')
            }
    except Error as e:
        print(e)
    finally:
        if conn:
            conn.close()
    return latest_poll

def get_poll_by_poll_id(poll_id):
    db_file = r"./db/MMM-SQLite.db"
    with sqlite3.connect(db_file) as conn:
        cur = conn.cursor()
        res = cur.execute("SELECT * FROM polls WHERE poll_id = ?", (poll_id,))
        poll = res.fetchone()
        if poll:
            poll = {
                "poll_id": poll[0],
                "poll_rate": poll[1],
                "operating_system": poll[2],
                "time": poll[3]
            }
        else:
            abort(404, description=f"Poll ID {poll_id} not found")
    return poll

def get_polls_by_time_interval(start_time, end_time):
    start_dt, end_dt = datetime_validation(start_time, end_time)
    poll_list = []
    try:
        db_file = r"./db/MMM-SQLite.db"
        conn = sqlite3.connect(db_file)
        cur = conn.cursor()
        res = cur.execute("SELECT * FROM polls WHERE time BETWEEN ? AND ?", (start_dt, end_dt))
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

def get_metrics_by_time_interval(start_time, end_time):
    start_dt, end_dt = datetime_validation(start_time, end_time)
    poll_ids = []
    try:
        db_file = r"./db/MMM-SQLite.db"
        conn = sqlite3.connect(db_file)
        cur = conn.cursor()
        res = cur.execute("SELECT * FROM polls WHERE time BETWEEN ? AND ?", (start_dt, end_dt))
        polls = res.fetchall()
        for i in polls:
            poll_ids.append(i[0])
    except Error as e:
        print(e)
    finally:
        if conn:
            conn.close()
    return poll_ids


def datetime_validation(start_time, end_time):
    if len(start_time) == 19:
        try:
            start_dt = datetime.datetime.strptime(start_time, '%Y-%m-%d %H:%M:%S')
            end_dt = datetime.datetime.strptime(end_time, '%Y-%m-%d %H:%M:%S')
        except ValueError as e:
            raise ValueError("Invalid timestamp format. Expected format: YYYY-mm-dd HH-MM-SS")
    elif len(start_time) == 16:
        try:
            start_dt = datetime.datetime.strptime(start_time, '%Y-%m-%d %H:%M')
            start_dt = start_dt.replace(second=0)
            end_dt = datetime.datetime.strptime(end_time, '%Y-%m-%d %H:%M')
            end_dt = end_dt.replace(second=59)
        except ValueError as e:
            raise ValueError("Invalid timestamp format. Expected format: YYYY-mm-dd HH-MM")
    elif len(start_time) == 13:
        try:
            start_dt = datetime.datetime.strptime(start_time, '%Y-%m-%d %H')
            start_dt = start_dt.replace(minute=0,second=0)
            end_dt = datetime.datetime.strptime(end_time, '%Y-%m-%d %H')
            end_dt = end_dt.replace(minute=59,second=59)
        except ValueError as e:
            raise ValueError("Invalid timestamp format. Expected format: YYYY-mm-dd HH")
    elif len(start_time) == 10:
        try:
            start_dt = datetime.datetime.strptime(start_time, '%Y-%m-%d')
            start_dt = start_dt.replace(hour=0,minute=0,second=0)
            end_dt = datetime.datetime.strptime(end_time, '%Y-%m-%d')
            end_dt = end_dt.replace(hour=23,minute=59,second=59)
        except ValueError as e:
            raise ValueError("Invalid timestamp format. Expected format: YYYY-mm-dd")
    else:
        raise ValueError("Invalid timestamp format. Expected format: YYYY-mm-dd HH-MM-SS")
    return start_dt, end_dt