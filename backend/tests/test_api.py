import requests
import json
import datetime


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

#def test_get_poll_by_time_interval():
    # use 'get_poll_by_id' endpoint to return and parse example start and end times
    #poll1 = requests.get('http://127.0.0.1:5000/api/polls/1')
    #poll3 = requests.get('http://127.0.0.1:5000/api/polls/3')
    #poll1_time = json.loads(poll1.text)['time']
    #poll3_time = json.loads(poll3.text)['time']
    #print("poll times: " + poll1_time + poll3_time)

    # use example start and end times to test endpoint
    #polls_between = requests.get(f'http://127.0.0.1:5000/api/polls/{poll1_time}/{poll3_time}')
    #polls_between_list = json.loads(polls_between.text)
    #assert polls_between_list[0]['poll_id'] == 1
    #assert polls_between_list[-1]['poll_id'] == 3

#def test_get_metrics_by_time_interval():


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