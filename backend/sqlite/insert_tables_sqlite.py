import sqlite3
from sqlite3 import Error


def create_connection(db_file):
    """ create a database connection to the SQLite database
        specified by db_file
    :param db_file: database file
    :return: Connection object or None
    """
    conn = None
    try:
        conn = sqlite3.connect(db_file)
    except Error as e:
        print(e)

    return conn


def insert_poll(conn, poll):
    """
    Log a new poll to polls table
    :param conn:
    :param poll:
    :return: project id
    """
    sql = ''' INSERT INTO polls(poll_rate,operating_system,time)
              VALUES(?,?,?) '''
    cur = conn.cursor()
    cur.execute(sql, poll)
    conn.commit()
    return cur.lastrowid


def insert_process(conn, process):
    """
    Create a new process
    :param conn:
    :param task:
    :return:
    """

    sql = ''' INSERT INTO processes(poll_id,thread_count,memory)
              VALUES(?,?,?) '''
    cur = conn.cursor()
    cur.execute(sql, process)
    conn.commit()
    return cur.lastrowid


def main():
    database = r"./db/testDB.db"

    # create a database connection
    conn = create_connection(database)
    with conn:
        # log a poll
        poll_data = (2, 'Linux', '02-06-2023')
        poll_id = insert_poll(conn, poll_data)

        # log a process
        process_data = (poll_id, 6, 8)
        process_id = insert_process(conn, process_data)

        print("Poll ID: {}, and Process ID: {} have been logged in SQLite DB".format(poll_id,process_id))
if __name__ == '__main__':
    main()