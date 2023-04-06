import requests
import json
import datetime


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

def test_get_poll_by_time_interval():
    # use 'get_poll_by_id' endpoint to return and parse example start and end times
    poll1 = requests.get('http://127.0.0.1:5000/api/polls/1')
    poll3 = requests.get('http://127.0.0.1:5000/api/polls/3')
    poll1_time = json.loads(poll1.text)['time']
    poll3_time = json.loads(poll3.text)['time']
    print("poll times: " + poll1_time + poll3_time)

    # use example start and end times to test endpoint
    #polls_between = requests.get(f'http://127.0.0.1:5000/api/polls/{poll1_time}/{poll3_time}')
    #polls_between_list = json.loads(polls_between.text)
    #assert polls_between_list[0]['poll_id'] == 1
    #assert polls_between_list[-1]['poll_id'] == 3