from flask import Flask, jsonify, request
from flask_cors import CORS
from apscheduler.schedulers.background import BackgroundScheduler
from flask_apscheduler import APScheduler
from monitor.monitor import poll_system
from db.service import main_poll
from db.db_init import *
from sqlite3 import Error
from api import polls, processes, memory, disks, network, cpu
import time
from datetime import datetime, timedelta
from apscheduler.triggers.date import DateTrigger

"""
run on percentage of minute intervals
gets system info and logs it
TODO:add awaiting till db is created to prevent error
"""
def sensor(polling_rate, poll_type):
    try:
        poll = poll_system()
        main_poll(poll, polling_rate, poll_type)
        # print(json.dumps(poll, indent=4, sort_keys=False))
    except Exception as e:
        print(e)
        print("monitor fail")
        pass






"""
Start monitoring at specified rate
"""
polling_rate = 0.07
sched = BackgroundScheduler(daemon=True)

live_poll_job = sched.add_job(sensor,args=[polling_rate,"live"],trigger ='interval',minutes=polling_rate)
sched.start()


app = Flask(__name__)
CORS(app, resources={r"/*": {"origins": "*"}})

@app.route('/api/polling_rate', methods=['POST'])
def api_set_polling_rate():
    global polling_rate
    global live_poll_job
    data = request.get_json()
    new_rate = data.get('polling_rate')
    polling_rate = new_rate

    # Stop the current job
    if live_poll_job:
        live_poll_job.remove()

    # Start a new job with the updated polling rate
    live_poll_job = sched.add_job(sensor, args=[polling_rate, "live"], trigger='interval', minutes=polling_rate)

    return jsonify({'message': f'Polling rate changed to {new_rate}'}), 200


@app.route('/api/metrics', methods=['GET'])
def api_get_metrics():
    metrics = {}
    metrics['poll'] = polls.get_polls()
    metrics['processes'] = processes.get_processes()
    metrics['network'] = network.get_networks()
    metrics['disk'] = disks.get_disks()
    metrics['memory'] = memory.get_memory()
    metrics['cpu'] = cpu.get_cpu()
    return metrics

@app.route('/api/metrics/latest', methods=['GET'])
def api_get_latest_metrics():
    metrics = {}
    metrics['poll'] = polls.get_latest_poll()
    metrics['processes'] = processes.get_latest_processes()
    metrics['network'] = network.get_latest_networks()
    metrics['disk'] = disks.get_latest_disk()
    metrics['memory'] = memory.get_latest_memory()
    metrics['cpu'] = cpu.get_latest_cpu()
    return metrics

@app.route('/api/metrics/<poll_id>', methods=['GET'])
def api_get_metrics_by_poll_id(poll_id):
    metrics = {}
    metrics['poll'] = polls.get_poll_by_poll_id(poll_id)
    metrics['processes'] = processes.get_processes_by_poll_id(poll_id)
    metrics['network'] = network.get_networks_by_poll_id(poll_id)
    metrics['disk'] = disks.get_disk_by_poll_id(poll_id)
    metrics['memory'] = memory.get_memory_by_poll_id(poll_id)
    metrics['cpu'] = cpu.get_cpu_by_poll_id(poll_id)
    return metrics

@app.route('/api/metrics/<start_time>/<end_time>', methods=['GET'])
def api_get_metrics_by_time_interval(start_time, end_time):
    metrics_list = []
    polls_ids = polls.get_metrics_by_time_interval(start_time, end_time)
    for poll_id in polls_ids:
        metrics = {}
        metrics['poll'] = polls.get_poll_by_poll_id(poll_id)
        metrics['process'] = processes.get_processes_by_poll_id(poll_id)
        metrics['network'] = network.get_networks_by_poll_id(poll_id)
        metrics['disk'] = disks.get_disk_by_poll_id(poll_id)
        metrics['memory'] = memory.get_memory_by_poll_id(poll_id)
        metrics['cpu'] = cpu.get_cpu_by_poll_id(poll_id)
        metrics_list.append(metrics)
    return metrics_list

@app.route('/api/polls', methods=['GET'])
def api_get_polls():
    return polls.get_polls()

@app.route('/api/polls/latest', methods=['GET'])
def api_get_latest_poll():
    return polls.get_latest_poll()

@app.route('/api/polls/<poll_id>', methods=['GET'])
def api_get_poll_by_poll_id(poll_id):
    return polls.get_poll_by_poll_id(poll_id)

@app.route('/api/polls/<start_time>/<end_time>', methods=['GET'])
def api_get_polls_by_time_interval(start_time, end_time):
    return polls.get_polls_by_time_interval(start_time, end_time)

@app.route('/api/processes', methods=['GET'])
def api_get_processes():
    return processes.get_processes()

@app.route('/api/processes/latest', methods=['GET'])
def api_get_latest_processes():
    return processes.get_latest_processes()

@app.route('/api/processes/latest/<process_id>', methods=['GET'])
def api_get_latest_process_by_process_id(process_id):
    return processes.get_latest_process_by_process_id(process_id)

@app.route('/api/processes/<process_id>', methods=['GET'])
def api_get_processes_by_process_id(process_id):
    return processes.get_processes_by_process_id(process_id)

@app.route('/api/processes/poll/<poll_id>', methods=['GET'])
def api_get_processes_by_poll_id(poll_id):
    return processes.get_processes_by_poll_id(poll_id)

@app.route('/api/memory', methods=['GET'])
def api_get_memory():
    return memory.get_memory()

@app.route('/api/memory/latest', methods=['GET'])
def api_get_latest_memory():
    return memory.get_latest_memory()

@app.route('/api/memory/<memory_id>', methods=['GET'])
def api_get_memory_by_memory_id(memory_id):
    return memory.get_memory_by_memory_id(memory_id)

@app.route('/api/memory/poll/<poll_id>', methods=['GET'])
def api_get_memory_by_poll_id(poll_id):
    return memory.get_memory_by_poll_id(poll_id)

@app.route('/api/disks', methods=['GET'])
def api_get_disks():
    return disks.get_disks()

@app.route('/api/disks/latest', methods=['GET'])
def api_get_latest_disk():
    return disks.get_latest_disk()

@app.route('/api/disks/<disk_id>', methods=['GET'])
def api_get_disk_by_disk_id(disk_id):
    return disks.get_disk_by_disk_id(disk_id)

@app.route('/api/disks/poll/<poll_id>', methods=['GET'])
def api_get_disk_by_poll_id(poll_id):
    return disks.get_disk_by_poll_id(poll_id)

@app.route('/api/networks', methods=['GET'])
def api_get_network():
    return network.get_networks()

@app.route('/api/networks/latest', methods=['GET'])
def api_get_latest_network():
    return network.get_latest_networks()

@app.route('/api/networks/<network_id>', methods=['GET'])
def api_get_network_by_disk_id(network_id):
    return network.get_network_by_network_id(network_id)

@app.route('/api/networks/poll/<poll_id>', methods=['GET'])
def api_get_network_by_network_id(poll_id):
    return network.get_networks_by_poll_id(poll_id)

@app.route('/api/cpu', methods=['GET'])
def api_get_cpu():
    return cpu.get_cpu()

@app.route('/api/cpu/latest', methods=['GET'])
def api_get_latest_cpu():
    return cpu.get_latest_cpu()

@app.route('/api/cpu/<cpu_id>', methods=['GET'])
def api_get_cpu_by_cpu_id(cpu_id):
    return cpu.get_cpu_by_cpu_id(cpu_id)

@app.route('/api/cpu/poll/<poll_id>', methods=['GET'])
def api_get_cpu_by_poll_id(poll_id):
    return cpu.get_cpu_by_poll_id(poll_id)


if __name__ == "__main__":
    create_db()
    app.run()
