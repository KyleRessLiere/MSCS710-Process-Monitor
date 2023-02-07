#!/usr/bin/python

def create_connection(db_file):
    """ create a database connection to a SQLite database """
    conn = None
    try:
        conn = sqlite3.connect(db_file)
        print(sqlite3.version)
    except Error as e:
        print(e)
    finally:
        if conn:
            conn.close()


def create_polls_table(conn):
    try:
        sql_create_polls_table = """ CREATE TABLE IF NOT EXISTS polls (
                                        id integer PRIMARY KEY,
                                        poll_rate integer,
                                        operating_system text,
                                        time text
                                    ); """
        c = conn.cursor()
        c.execute(sql_create_polls_table)
    except Error as e:
        print(e)

def create_processes_table(conn):
    try:
        sql_create_processes_table = """CREATE TABLE IF NOT EXISTS processes (
                                        process_id integer PRIMARY KEY,
                                        poll_id integer NOT NULL,
                                        thread_count integer,
                                        memory integer,
                                        FOREIGN KEY (poll_id) REFERENCES polls (id)
                                    );"""
        c = conn.cursor()
        c.execute(sql_create_processes_table)
    except Error as e:
        print(e)

def create_cpu_table(conn):
    try:
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
        c = conn.cursor()
        c.execute(sql_create_cpu_table)
    except Error as e:
        print(e)

def create_memory_table(conn):
    try:
        sql_create_memory_table = """CREATE TABLE IF NOT EXISTS memory (
                                    memory_id integer PRIMARY KEY,
                                    poll_id integer NOT NULL,
                                    total_memory integer,
                                    available_memory integer,
                                    used_memory integer,
                                    free_memory integer,
                                    active_memory integer,
                                    inactive_memory integer,
                                    wired_memory integer,
                                    FOREIGN KEY (poll_id) REFERENCES polls (id)
                                );"""
        c = conn.cursor()
        c.execute(sql_create_memory_table)
    except Error as e:
        print(e)

def create_disks_table(conn):
    try:
        sql_create_disks_table = """CREATE TABLE IF NOT EXISTS disks (
                                    disk_id integer PRIMARY KEY,
                                    poll_id integer NOT NULL,
                                    disk_partitions integer,
                                    disk_usage integer,
                                    disk_io_counters integer,
                                    FOREIGN KEY (poll_id) REFERENCES polls (id)
                                );"""
        c = conn.cursor()
        c.execute(sql_create_disks_table)
    except Error as e:
        print(e)

def create_network_table(conn):
    try:
        sql_create_network_table = """CREATE TABLE IF NOT EXISTS network (
                                    network_id integer PRIMARY KEY,
                                    poll_id integer NOT NULL,
                                    net_io_counters integer,
                                    net_connections integer,
                                    net_if_addrs integer,
                                    net_if_stats integer,
                                    FOREIGN KEY (poll_id) REFERENCES polls (id)
                                );"""
        c = conn.cursor()
        c.execute(sql_create_network_table)
    except Error as e:
        print(e)


if __name__ == '__main__':
    conn = create_connection(r"./sqlite-db/MMM-SQLite.db")
    create_polls_table(conn)
    create_processes_table(conn)
    create_cpu_table(conn)
    create_memory_table(conn)
    create_disks_table(conn)
    create_network_table(conn)
    print("DB tables have been created/updated")