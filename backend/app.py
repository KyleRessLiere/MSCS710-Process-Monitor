from flask import Flask, request, jsonify
from flask_cors import CORS
from apscheduler.schedulers.background import BackgroundScheduler
from flask_apscheduler import APScheduler
from monitor.monitor import poll_system
from db.service import main_poll
from db.db_init import *
from sqlite3 import Error
from api import polls, processes, memory, disks, network


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

@app.route('/api/polls/<poll_id>', methods=['GET'])
def api_get_poll_by_id(poll_id):
    poll = []
    poll.append(polls.get_poll_by_id(poll_id))
    return poll

@app.route('/api/processes', methods=['GET'])
def api_get_processes():
    return processes.get_processes()

@app.route('/api/processes/<process_id>', methods=['GET'])
def api_get_process_by_id(process_id):
    process_item = []
    process_item.append(processes.get_process_by_id(process_id))
    return process_item

@app.route('/api/memory', methods=['GET'])
def api_get_memory():
    return memory.get_memory()

@app.route('/api/memory/<memory_id>', methods=['GET'])
def api_get_memory_by_id(memory_id):
    memory_item = []
    memory_item.append(memory.get_memory_by_id(memory_id))
    return memory_item

@app.route('/api/disks', methods=['GET'])
def api_get_disks():
    return disks.get_disks()

@app.route('/api/network', methods=['GET'])
def api_get_network():
    return network.get_network()

if __name__ == "__main__":
    sensor()
    create_db()
    app.run()