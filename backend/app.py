import json
from flask import Flask, request, jsonify
from flask_cors import CORS
import sqlite3
from sqlite3 import Error
from apscheduler.schedulers.background import BackgroundScheduler
from flask_apscheduler import APScheduler
from api import polls
from db.service import main_poll
from db.db_init import *

from monitor.monitor import poll_system


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

@app.route('/api/polls', methods=['GET'])
def api_get_polls():
    return jsonify(polls.get_polls())

@app.route('/api/polls/<poll_id>', methods=['GET'])
def api_get_poll(poll_id):
    return jsonify(polls.get_poll_by_id(poll_id))

@app.route('/api/polls/add',  methods = ['POST'])
def api_add_poll():
    print("working")
    poll = request.data
    #poll = json.loads(request.data, strict=False)
    print(jsonify(poll))
    return print("not working...")
    #return jsonify(polls.insert_poll(poll))

@app.route('/api/polls/update',  methods = ['PUT'])
def api_update_poll():
    poll = request.get_json()
    return jsonify(polls.update_poll(poll))

@app.route('/api/polls/delete/<poll_id>',  methods = ['DELETE'])
def api_delete_poll(poll_id):
    return jsonify(polls.delete_poll(poll_id))



if __name__ == "__main__":
   
    create_db()
    app.run()