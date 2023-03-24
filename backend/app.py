from flask import Flask, request, jsonify
from flask_cors import CORS
from apscheduler.schedulers.background import BackgroundScheduler
from flask_apscheduler import APScheduler
from monitor.monitor import poll_system
from db.service import main_poll
from db.db_init import *
from sqlite3 import Error
from api import polls, processes, memory, disks, network, cpu


"""
run on percentage of minute intervals
gets system info and loggs it
TODO:add awaiting till db is created to prevent error
"""
def sensor(polling_rate):
    try:
        poll = poll_system()
        main_poll(poll,polling_rate, "live")
        #print(json.dumps(poll, indent=4, sort_keys=False))
    except Exception as e:
        print(e)
        print("monitor fail")
        pass


"""
Start monitoring at specified rate
"""
polling_rate = 0.07
sched = BackgroundScheduler(daemon=True)
sched.add_job(sensor,args=[polling_rate],trigger ='interval',minutes=polling_rate)
sched.start()


app = Flask(__name__)
CORS(app, resources={r"/*": {"origins": "*"}})


@app.route('/api/metrics', methods=['GET'])
def api_get_latest_metrics():
    metrics = {}
    metrics['poll'] = polls.get_latest_poll()
    metrics['process'] = processes.get_latest_process()
    metrics['network'] = network.get_latest_network()
    metrics['disk'] = disks.get_latest_disk()
    metrics['memory'] = memory.get_latest_memory()
    metrics['cpu'] = cpu.get_latest_cpu()
    return metrics

@app.route('/api/polls', methods=['GET'])
def api_get_polls():
    return polls.get_polls()

@app.route('/api/polls/<poll_id>', methods=['GET'])
def api_get_poll_by_id(poll_id):
    return polls.get_poll_by_id(poll_id)

@app.route('/api/processes', methods=['GET'])
def api_get_processes():
    return processes.get_processes()

@app.route('/api/processes/<process_id>', methods=['GET'])
def api_get_processes_by_id(process_id):
    return processes.get_processes_by_id(process_id)

@app.route('/api/processes/latest/<process_id>', methods=['GET'])
def api_get_latest_process_by_id(process_id):
    return processes.get_latest_process_by_id(process_id)

@app.route('/api/memory', methods=['GET'])
def api_get_memory():
    return memory.get_memory()

@app.route('/api/memory/<memory_id>', methods=['GET'])
def api_get_memory_by_id(memory_id):
    return memory.get_memory_by_id(memory_id)

@app.route('/api/disks', methods=['GET'])
def api_get_disks():
    return disks.get_disks()

@app.route('/api/disks/<disk_id>', methods=['GET'])
def api_get_disk_by_id(disk_id):
    return disks.get_disk_by_id(disk_id)

@app.route('/api/network', methods=['GET'])
def api_get_network():
    return network.get_network()

@app.route('/api/network/<network_id>', methods=['GET'])
def api_get_network_by_id(network_id):
    return network.get_network_by_id(network_id)

@app.route('/api/cpu', methods=['GET'])
def api_get_cpu():
    return cpu.get_cpu()

@app.route('/api/cpu/<cpu_id>', methods=['GET'])
def api_get_cpu_by_id(cpu_id):
    return cpu.get_cpu_by_id(cpu_id)


if __name__ == "__main__":
    create_db()
    app.run()
