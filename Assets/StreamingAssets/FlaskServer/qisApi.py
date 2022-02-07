#!/usr/bin/env python3

from logging import debug
from qiskit import QuantumRegister, ClassicalRegister
from qiskit import QuantumCircuit, Aer, execute, IBMQ
from qiskit import circuit
from sympy.physics.quantum.qubit import matrix_to_qubit,measure_all
import numpy as np



def run_circuit(circuit_string: str, backend_to_run="qasm_simulator", api_token=None, shots=1024, memory=False):
    qc = build_circuit(circuit_string)
    print(qc)

    sim_result = simulate_circuit(qc, backend_to_run, api_token, shots, memory)
    result = sim_result.get_counts(qc)

    return result


def simulate_circuit(qc: QuantumCircuit, backend_to_run="qasm_simulator", api_token=None, shots=1024, memory=False):
    if api_token:
        IBMQ.enable_account(
            api_token, 'https://api.quantum-computing.ibm.com/api/Hubs/ibm-q/Groups/open/Projects/main')

    backend = Aer.get_backend(backend_to_run)
    job_sim = execute(qc, backend, shots=shots, memory=memory)
    sim_result = job_sim.result()

    return sim_result


def get_state_vector(circuit_string):
    qc = build_circuit(circuit_string)

    backend = Aer.get_backend('statevector_simulator')
    job_statevector = execute(qc,backend)
    result = job_statevector.result()
    statevector = result.get_statevector(qc)
    ket_vector = matrix_to_qubit(np.array(statevector)[:, np.newaxis])

    return ket_vector

def get_prob_table(circuit_string):
    ket_vector = get_state_vector(circuit_string)
    probs = measure_all(ket_vector)

    return probs


def build_circuit(circuit_string):
    circuit_string = "def get_qc():" + circuit_string
    exec(circuit_string, globals())
    qc = globals()['get_qc']()
    return qc


def backend_configuration(backend_to_run="qasm_simulator"):
    backend = Aer.get_backend(backend_to_run)
    return backend.configuration().as_dict()
