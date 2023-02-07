#!/usr/bin/python

exec(open('./create_db_sqlite.py').read())
exec(open('./db_version_sqlite.py').read())
exec(open('./create_tables_sqlite.py').read())
exec(open('./insert_tables_sqlite.py').read())