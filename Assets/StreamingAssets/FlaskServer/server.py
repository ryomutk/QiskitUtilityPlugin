#!/usr/bin/env python3
from logging import NullHandler, debug
from flask import request
from flask import jsonify
from flask import Flask
from qisApi import get_prob_table
from qisApi import get_state_vector
from qisApi import build_circuit
from qisApi import run_circuit
from qisApi import backend_configuration
import re

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

@app.route('/api/simulate/probability',methods=['POST'])
def simulate_probability():
    circuit_string = request.form.get("circuit")
    prob_table = get_prob_table(circuit_string)

    output = dict()
    for dat in prob_table:
        label = str(dat[0])
        label = re.sub(r"\D","",label)
        output[label] = round(dat[1],5)

    return str(output)


if __name__ == '__main__':
    app.run(host='0.0.0.0', port=8001)
