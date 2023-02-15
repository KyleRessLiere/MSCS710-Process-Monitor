from flask import Flask, request, jsonify
from flask_cors import CORS
from apscheduler.schedulers.background import BackgroundScheduler
from flask_apscheduler import APScheduler
from monitor.monitor import poll_system
from db.service import main_poll
from db.db_init import *
from sqlite3 import Error
from api import polls, processes


"""
run on percentage of minute intervals
gets system info and loggs it
"""
def sensor():
    try:
        poll = poll_system()
        process_list = poll["process"]
        main_poll(process_list)
        #print(json.dumps(poll, indent=4, sort_keys=False))

    except Exception:
        print("monitor fail")
        pass

sched = BackgroundScheduler(daemon=True)
sched.add_job(sensor,'interval',minutes=.07)
sched.start()


app = Flask(__name__)
CORS(app, resources={r"/*": {"origins": "*"}})

@app.route('/api/polls', methods=['GET'])
def api_get_polls():
    return polls.get_polls()

@app.route('/api/processes', methods=['GET'])
def api_get_proccess():
    return processes.get_processes()



if __name__ == "__main__":
    sensor()
    create_db()
    app.run()