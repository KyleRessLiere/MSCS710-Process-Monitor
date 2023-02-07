#!/usr/bin/python

import sqlite3
from sqlite3 import Error

def create_connection():
    """ create a database connection to a SQLite database """
    conn = None
    try:
        db_file = '../db/sqlite-db/MMM-SQLite.db'  # Adjust Different Databases HERE
        conn = sqlite3.connect(db_file)
        print(sqlite3.version)
    except Error as e:
        print(e)
    finally:
        if conn:
            conn.close()


## CRUD ##

def get_polls():
    polls = []
    try:
        conn = create_connection()
        conn.row_factory = sqlite3.Row
        cur = conn.cursor()
        cur.execute("SELECT * FROM polls")
        rows = cur.fetchall()

        for i in rows:
            poll = {}
            poll["poll_id"] = i["id"]
            poll["poll_rate"] = i["poll_rate"]
            poll["operating_system"] = i["operating_system"]
            poll["time"] = i["time"]
            polls.append(poll)

    except:
        polls = []

    return polls


def get_poll_by_id(poll_id):
    poll = {}
    try:
        conn = create_connection()
        conn.row_factory = sqlite3.Row
        cur = conn.cursor()
        cur.execute("SELECT * FROM polls WHERE id = ?", (poll_id,))
        row = cur.fetchone()

        poll["poll_id"] = row["id"]
        poll["poll_rate"] = row["poll_rate"]
        poll["operating_system"] = row["operating_system"]
        poll["time"] = row["time"]
    except:
        poll = {}

    return poll


def insert_poll(poll):
    inserted_user = {}
    try:
        sql = ''' INSERT INTO polls(poll_rate,operating_system,time)
                    VALUES(?,?,?) '''
        conn = create_connection()
        cur = conn.cursor()
        cur.execute(sql, poll)
        conn.commit()
        inserted_user = get_poll_by_id(cur.lastrowid)

    except:
        conn().rollback()

    finally:
        conn.close()

    return inserted_user


def update_poll(poll):
    updated_poll = {}
    try:
        conn = create_connection()
        cur = conn.cursor()
        cur.execute("UPDATE polls SET poll_rate = ?, operating_system = ?, time = ? WHERE user_id =?",
                     (poll["poll_rate"], poll["operating_system"], poll["time"],))
        conn.commit()
        updated_poll = get_poll_by_id(poll["poll_id"])

    except:
        conn.rollback()
        updated_poll = {}
    finally:
        conn.close()

    return updated_poll


def delete_poll(poll_id):
    msg = {}
    try:
        conn = create_connection()
        conn.execute("DELETE from polls WHERE poll_id = ?",
                      (poll_id,))
        conn.commit()
        msg["status"] = ("Poll(%s) deleted successfully" %(poll_id))
    except:
        conn.rollback()
        msg["status"] = "Cannot delete user"
    finally:
        conn.close()

    return msg