#!/usr/bin/env python3
from logging import NullHandler
from flask import request
from flask import jsonify
from flask import Flask
from api import run_circuit
from api import run_qasm
from api import backend_configuration
import json

app = Flask(__name__)


@app.route('/')
def welcome():
    return "Hi Qiskiter!"

@app.route('/api/backend/configuration')
def backend_config():
    config = backend_configuration('qasm_simulator')
    return jsonify({"result": config})

@app.route('/api/run/circuit',methods=['POST'])
def runStringCircuit():
    circuit_string = request.form.get("circuit")
    shots = int(request.form.get("shots"))
    output = run_circuit(circuit_string,shots=shots)


    return jsonify(output)

@app.route('/api/run/randomizer', methods=['POST'])
def runRandomizer():
    num = int(request.form.get("num"))
    output = run_qasm(num)

    return jsonify(output)

if __name__ == '__main__':
    app.run(host='0.0.0.0', port=8001)
