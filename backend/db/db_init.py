import sqlite3
from sqlite3 import Error


def create_connection(db_file):
    """ create a database connection to the SQLite database """
    conn = None
    try:
        conn = sqlite3.connect(db_file)
        return conn
    except Error as e:
        print(e)

    return conn


def create_table(conn, create_table_sql):
    """ create a table from the create_table_sql statement """
    try:
        c = conn.cursor()
        c.execute(create_table_sql)
    except Error as e:
        print(e)


def create_db():
    database = r"./db/MMM-SQLite.db"

    sql_create_polls_table = """ CREATE TABLE IF NOT EXISTS polls (
                                        id integer PRIMARY KEY,
                                        poll_rate integer,
                                        operating_system text,
                                        time text
                                    ); """

    sql_create_processes_table = """CREATE TABLE IF NOT EXISTS processes (
                                    poll_id integer NOT NULL PRIMARY KEY
                                    process_id integer ,
                                    process_name text,
                                    process_status text,
                                    cpu_percent real,
                                    num_thread integer,
                                    memory_usage real
                                    FOREIGN KEY (poll_id) REFERENCES polls (id)
                                );"""
   
    sql_create_cpu_table = """CREATE TABLE IF NOT EXISTS cpu (
                                    cpu_id integer PRIMARY KEY,
                                    poll_id integer NOT NULL,
                                    cpu_times integer,
                                    cpu_percent integer,
                                    cpu_times_percent integer,
                                    cpu_count integer,
                                    cpu_stats integer,
                                    cpu_freq integer,
                                    FOREIGN KEY (poll_id) REFERENCES polls (id)
                                );"""

    sql_create_memory_table = """CREATE TABLE IF NOT EXISTS memory (
                                    memory_id integer PRIMARY KEY,
                                    poll_id integer NOT NULL,
                                    total_memory real,
                                    available_memory real,
                                    used_memory real,
                                    percentage_used real,
                                    FOREIGN KEY (poll_id) REFERENCES polls (id)
                                );"""

    sql_create_disks_table = """CREATE TABLE IF NOT EXISTS disks (
                                    disk_id integer PRIMARY KEY,
                                    poll_id integer NOT NULL,
                                    disk_total integer,
                                    disk_used integer,
                                    disk_free integer,
                                    dis_percentage real
                                    FOREIGN KEY (poll_id) REFERENCES polls (id)
                                );"""

    sql_create_network_table = """CREATE TABLE IF NOT EXISTS network (
                                    network_id integer PRIMARY KEY,
                                    poll_id integer NOT NULL,
                                    network_interface text,
                                    network_status text,
                                    network_speed real,
                                    FOREIGN KEY (poll_id) REFERENCES polls (id)
                                );"""


    # create a database connection
    conn = create_connection(database)

    # create tables
    if conn is not None:
        create_table(conn, sql_create_polls_table)
        create_table(conn, sql_create_processes_table)
        create_table(conn, sql_create_cpu_table)
        create_table(conn, sql_create_memory_table)
        create_table(conn, sql_create_disks_table)
        create_table(conn, sql_create_network_table)
        print("-> MMM DB has been verified!")
        print("-> SQLite version: " + sqlite3.version)
    else:
        print("[DB Error]: cannot create the database connection.")


if __name__ == '__main__':
    
    create_db()