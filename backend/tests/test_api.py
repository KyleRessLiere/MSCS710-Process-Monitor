import requests

def test_get_polls():
    response = requests.get('http://127.0.0.1:5000/api/polls')
    assert response.status_code == 200
    assert isinstance(response.json(), list)
    for poll in response.json():
        assert 'operating_system' in poll
        assert 'poll_id' in poll
        assert 'poll_rate' in poll
        assert 'time' in poll

def test_latest_poll():
    url = "http://127.0.0.1:5000/api/polls/latest"
    response = requests.get(url)
    assert response.status_code == 200
    data = response.json()
    assert "operating_system" in data
    assert "poll_id" in data
    assert "poll_rate" in data
    assert "time" in data

def test_poll_id_endpoint():
    url = "http://127.0.0.1:5000/api/polls/1"
    response = requests.get(url)
    assert response.status_code == 200
    data = response.json()
    assert "operating_system" in data
    assert data["poll_id"] == 1
    assert "poll_rate" in data
    assert "time" in data