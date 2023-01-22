
from flask import Flask
import pika
import monitor

#!/usr/bin/python3
""" Demonstrating Flask, using APScheduler. """

from apscheduler.schedulers.background import BackgroundScheduler
from flask import Flask

def sensor():
    print(monitor.getRunningProcesses())
    print("POLLING ")

sched = BackgroundScheduler(daemon=True)
sched.add_job(sensor,'interval',minutes=.5)
sched.start()
app = Flask(__name__)


@app.route('/')
def index():
    return 'testing'


@app.route('/add-job/<cmd>')
def add(cmd):
    connection = pika.BlockingConnection(pika.ConnectionParameters(host='rabbitmq'))
    channel = connection.channel()
    channel.queue_declare(queue='task_queue', durable=True)
    channel.basic_publish(
        exchange='',
        routing_key='task_queue',
        body=cmd,
        properties=pika.BasicProperties(
            delivery_mode=2,  # make message persistent
        ))
    connection.close()
    return " [x] Sent: %s" % cmd


if __name__ == '__main__':
    app.run(debug=True, host='0.0.0.0', port=4200)
