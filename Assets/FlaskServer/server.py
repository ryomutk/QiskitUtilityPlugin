#!/usr/bin/env python3
from logging import NullHandler
from flask import request
from flask import jsonify
from flask import Flask
from api import get_state_vector
from api import build_circuit
from api import run_circuit
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

@app.route('/api/summary/circuit',methods=['POST'])
def getcircuitSummary():

    circuit_string = request.form.get("circuit")
    qc = build_circuit(circuit_string)
    return str(qc)

@app.route('/api/simulate/statevector',methods=['POST'])
def simulate_statevector():
    circuit_string = request.form.get("circuit")
    qc = get_state_vector(circuit_string)

    return str(qc)
