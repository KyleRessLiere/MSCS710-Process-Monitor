import requests
import json
import datetime


## METRICS API ##

def test_get_metrics():
    response = requests.get('http://127.0.0.1:5000/api/metrics')
    assert response.status_code == 200
    data = response.json()
    assert 'poll' in data
    assert 'processes' in data
    assert 'network' in data
    assert 'disk' in data
    assert 'memory' in data
    assert 'cpu' in data

def test_get_latest_metrics():
    response = requests.get('http://127.0.0.1:5000/api/metrics/latest')
    assert response.status_code == 200
    data = response.json()
    assert 'poll' in data
    assert 'processes' in data
    assert 'network' in data
    assert 'disk' in data
    assert 'memory' in data
    assert 'cpu' in data

def test_get_metrics_by_poll_id():
    response = requests.get('http://127.0.0.1:5000/api/metrics/1')
    assert response.status_code == 200
    data = response.json()
    assert 'poll' in data
    assert data['poll']['poll_id'] == 1
    assert 'processes' in data
    assert 'network' in data
    assert 'disk' in data
    assert 'memory' in data
    assert 'cpu' in data



def test_get_metrics_by_time_interval():
    # use 'get_poll_by_id' endpoint to return and parse example start and end times
    metrics1 = requests.get('http://127.0.0.1:5000/api/metrics/1')
    metrics3 = requests.get('http://127.0.0.1:5000/api/metrics/3')
    metrics1_time = json.loads(metrics1.text)['poll']['time']
    metrics3_time = json.loads(metrics3.text)['poll']['time']

    # use example start and end times to test endpoint
    metrics_between = requests.get(f'http://127.0.0.1:5000/api/metrics/{metrics1_time}/{metrics3_time}')
    metrics_between_list = json.loads(metrics_between.text)
    assert len(metrics_between_list) == 3             # CHECK: array of 3 objects
    assert metrics_between_list[0]['poll']['poll_id'] == 1    # CHECK: first object has Poll ID == 1
    assert metrics_between_list[-1]['poll']['poll_id'] == 3   # CHECK: last object has Poll ID == 3


## POLLS API ##

def test_get_polls():
    response = requests.get('http://127.0.0.1:5000/api/polls')
    assert response.status_code == 200
    assert isinstance(response.json(), list)
    for poll in response.json():
        assert 'operating_system' in poll
        assert 'poll_id' in poll
        assert 'poll_rate' in poll
        assert 'time' in poll

def test_get_latest_poll():
    url = "http://127.0.0.1:5000/api/polls/latest"
    response = requests.get(url)
    assert response.status_code == 200
    data = response.json()
    assert "operating_system" in data
    assert "poll_id" in data
    assert "poll_rate" in data
    assert "time" in data

def test_get_poll_by_poll_id():
    url = "http://127.0.0.1:5000/api/polls/1"
    response = requests.get(url)
    assert response.status_code == 200
    data = response.json()
    assert "operating_system" in data
    assert data["poll_id"] == 1
    assert "poll_rate" in data
    assert "time" in data

def test_get_polls_by_time_interval():
    # use 'get_poll_by_id' endpoint to return and parse example start and end times
    poll1 = requests.get('http://127.0.0.1:5000/api/polls/1')
    poll3 = requests.get('http://127.0.0.1:5000/api/polls/3')
    poll1_time = json.loads(poll1.text)['time']
    poll3_time = json.loads(poll3.text)['time']

    # use example start and end times to test endpoint
    polls_between = requests.get(f'http://127.0.0.1:5000/api/polls/{poll1_time}/{poll3_time}')
    polls_between_list = json.loads(polls_between.text)
    assert len(polls_between_list) == 3             # CHECK: array of 3 objects
    assert polls_between_list[0]['poll_id'] == 1    # CHECK: first object has Poll ID == 1
    assert polls_between_list[-1]['poll_id'] == 3   # CHECK: last object has Poll ID == 3


## PROCESSES API ##

def test_get_processes():
    response = requests.get('http://127.0.0.1:5000/api/processes')
    assert response.status_code == 200
    assert isinstance(response.json(), list)
    for process in response.json():
        assert 'cpu_percent' in process
        assert 'memory_usage' in process
        assert 'num_thread' in process
        assert 'poll_id' in process
        assert 'process_id' in process
        assert 'process_name' in process
        assert 'process_status' in process

def test_get_latest_processes():
    url = "http://127.0.0.1:5000/api/processes/latest"
    response = requests.get(url)
    assert response.status_code == 200
    assert isinstance(response.json(), list)
    for process in response.json():
        assert 'cpu_percent' in process
        assert 'memory_usage' in process
        assert 'num_thread' in process
        assert 'poll_id' in process
        assert 'process_id' in process
        assert 'process_name' in process
        assert 'process_status' in process

def test_get_latest_process_by_process_id():
    response = requests.get('http://127.0.0.1:5000/api/processes/poll/1')
    first_process = response.json()[0]
    process_id = first_process['process_id']
    url = "http://127.0.0.1:5000/api/processes/latest/" + str(process_id)
    response = requests.get(url)
    assert response.status_code == 200
    process = response.json()
    assert 'cpu_percent' in process
    assert 'memory_usage' in process
    assert 'num_thread' in process
    assert 'poll_id' in process
    assert process["process_id"] == process_id
    assert 'process_name' in process
    assert 'process_status' in process

def test_get_processes_by_process_id():
    response = requests.get('http://127.0.0.1:5000/api/processes/poll/1')
    first_process = response.json()[0]
    process_id = first_process['process_id']
    url = "http://127.0.0.1:5000/api/processes/" + str(process_id)
    response = requests.get(url)
    assert response.status_code == 200
    assert isinstance(response.json(), list)
    for process in response.json():
        assert 'cpu_percent' in process
        assert 'memory_usage' in process
        assert 'num_thread' in process
        assert 'poll_id' in process
        assert process["process_id"] == process_id
        assert 'process_name' in process
        assert 'process_status' in process

def test_get_processes_by_poll_id():
    url = "http://127.0.0.1:5000/api/processes/poll/1"
    response = requests.get(url)
    assert response.status_code == 200
    assert isinstance(response.json(), list)
    for process in response.json():
        assert 'cpu_percent' in process
        assert 'memory_usage' in process
        assert 'num_thread' in process
        assert process["poll_id"] == 1
        assert 'process_id' in process
        assert 'process_name' in process
        assert 'process_status' in process


## MEMORY API ##

def test_get_memory():
    response = requests.get('http://127.0.0.1:5000/api/memory')
    assert response.status_code == 200
    assert isinstance(response.json(), list)
    for memory in response.json():
        assert 'memory_id' in memory
        assert 'poll_id' in memory
        assert 'total_memory' in memory
        assert 'available_memory' in memory
        assert 'used_memory' in memory
        assert 'percentage_used' in memory

def test_get_latest_memory():
    url = "http://127.0.0.1:5000/api/memory/latest"
    response = requests.get(url)
    assert response.status_code == 200
    data = response.json()
    assert 'memory_id' in data
    assert 'poll_id' in data
    assert 'total_memory' in data
    assert 'available_memory' in data
    assert 'used_memory' in data
    assert 'percentage_used' in data

def test_get_memory_by_memory_id():
    url = "http://127.0.0.1:5000/api/memory/1"
    response = requests.get(url)
    assert response.status_code == 200
    data = response.json()
    assert data["memory_id"] == 1
    assert data["poll_id"] == 1
    assert 'total_memory' in data
    assert 'available_memory' in data
    assert 'used_memory' in data
    assert 'percentage_used' in data

def test_get_memory_by_poll_id():
    url = "http://127.0.0.1:5000/api/memory/poll/1"
    response = requests.get(url)
    assert response.status_code == 200
    data = response.json()
    assert data["memory_id"] == 1
    assert data["poll_id"] == 1
    assert 'total_memory' in data
    assert 'available_memory' in data
    assert 'used_memory' in data
    assert 'percentage_used' in data


## DISKS API ##

def test_get_disks():
    response = requests.get('http://127.0.0.1:5000/api/disks')
    assert response.status_code == 200
    assert isinstance(response.json(), list)
    for disk in response.json():
        assert 'disk_free' in disk
        assert 'disk_id' in disk
        assert 'disk_percentage' in disk
        assert 'disk_total' in disk
        assert 'disk_used' in disk
        assert 'poll_id' in disk

def test_get_latest_disk():
    url = "http://127.0.0.1:5000/api/disks/latest"
    response = requests.get(url)
    assert response.status_code == 200
    data = response.json()
    assert 'disk_free' in data
    assert 'disk_id' in data
    assert 'disk_percentage' in data
    assert 'disk_total' in data
    assert 'disk_used' in data
    assert 'poll_id' in data

def test_get_disk_by_disk_id():
    url = "http://127.0.0.1:5000/api/disks/1"
    response = requests.get(url)
    assert response.status_code == 200
    data = response.json()
    assert 'disk_free' in data
    assert data["disk_id"] == 1
    assert 'disk_percentage' in data
    assert 'disk_total' in data
    assert 'disk_used' in data
    assert data["poll_id"] == 1

def test_get_disk_by_poll_id():
    url = "http://127.0.0.1:5000/api/disks/poll/1"
    response = requests.get(url)
    assert response.status_code == 200
    data = response.json()
    assert 'disk_free' in data
    assert data["disk_id"] == 1
    assert 'disk_percentage' in data
    assert 'disk_total' in data
    assert 'disk_used' in data
    assert data["poll_id"] == 1


## NETWORK API ##

def test_get_network():
    response = requests.get('http://127.0.0.1:5000/api/network')
    assert response.status_code == 200
    assert isinstance(response.json(), list)
    for network in response.json():
        assert 'network_id' in network
        assert 'poll_id' in network
        assert 'network_interface' in network
        assert 'network_status' in network
        assert 'network_speed' in network

def test_get_latest_network():
    url = "http://127.0.0.1:5000/api/network/latest"
    response = requests.get(url)
    assert response.status_code == 200
    data = response.json()
    for network in response.json():
        assert 'network_id' in network
        assert 'poll_id' in network
        assert 'network_interface' in network
        assert 'network_status' in network
        assert 'network_speed' in network

def test_get_network_by_network_id():
    url = "http://127.0.0.1:5000/api/network/1"
    response = requests.get(url)
    assert response.status_code == 200
    data = response.json()
    assert data["network_id"] == 1
    assert data["poll_id"] == 1
    assert 'network_interface' in data
    assert 'network_status' in data
    assert 'network_speed' in data

def test_get_network_by_poll_id():
    url = "http://127.0.0.1:5000/api/network/poll/1"
    response = requests.get(url)
    assert response.status_code == 200
    data = response.json()
    assert data["network_id"] == 1
    assert data["poll_id"] == 1
    assert 'network_interface' in data
    assert 'network_status' in data
    assert 'network_speed' in data


## CPU API ##

def test_get_cpu():
    response = requests.get('http://127.0.0.1:5000/api/cpu')
    assert response.status_code == 200
    assert isinstance(response.json(), list)
    for cpu in response.json():
        assert 'cpu_count_physical' in cpu
        assert 'cpu_count_virtual' in cpu
        assert 'cpu_ctx_switches' in cpu
        assert 'cpu_id' in cpu
        assert 'cpu_percent' in cpu
        assert 'cpu_percentage_per_core' in cpu
        assert 'interrupts' in cpu
        assert 'poll_id' in cpu
        assert 'soft_interrupts' in cpu
        assert 'syscalls' in cpu

def test_get_latest_cpu():
    url = "http://127.0.0.1:5000/api/cpu/latest"
    response = requests.get(url)
    assert response.status_code == 200
    data = response.json()
    assert 'cpu_count_physical' in data
    assert 'cpu_count_virtual' in data
    assert 'cpu_ctx_switches' in data
    assert 'cpu_id' in data
    assert 'cpu_percent' in data
    assert 'cpu_percentage_per_core' in data
    assert 'interrupts' in data
    assert 'poll_id' in data
    assert 'soft_interrupts' in data
    assert 'syscalls' in data

def test_get_disk_by_disk_id():
    url = "http://127.0.0.1:5000/api/cpu/1"
    response = requests.get(url)
    assert response.status_code == 200
    data = response.json()
    assert 'cpu_count_physical' in data
    assert 'cpu_count_virtual' in data
    assert 'cpu_ctx_switches' in data
    assert data["cpu_id"] == 1
    assert 'cpu_percent' in data
    assert 'cpu_percentage_per_core' in data
    assert 'interrupts' in data
    assert data["poll_id"] == 1
    assert 'soft_interrupts' in data
    assert 'syscalls' in data

def test_get_disk_by_poll_id():
    url = "http://127.0.0.1:5000/api/cpu/poll/1"
    response = requests.get(url)
    assert response.status_code == 200
    data = response.json()
    assert 'cpu_count_physical' in data
    assert 'cpu_count_virtual' in data
    assert 'cpu_ctx_switches' in data
    assert data["cpu_id"] == 1
    assert 'cpu_percent' in data
    assert 'cpu_percentage_per_core' in data
    assert 'interrupts' in data
    assert data["poll_id"] == 1
    assert 'soft_interrupts' in data
    assert 'syscalls' in data