#!/usr/bin/env python3

from logging import debug
from qiskit import QuantumRegister, ClassicalRegister
from qiskit import QuantumCircuit, Aer, execute, IBMQ
from qiskit import circuit
from qiskit.circuit import quantumcircuit, quantumregister
from qiskit.providers import aer, backend
from QuantumRandomizer import RandomGenerator


def run_qasm(num, backend_to_run="qasm_simulator", api_token=None, shots=1024, memory=False):
    if api_token:
        IBMQ.enable_account(api_token, 'https://api.quantum-computing.ibm.com/api/Hubs/ibm-q/Groups/open/Projects/main')
    
    backend = Aer.get_backend(backend_to_run)

    result = dict()
    for i in range(num):
        qc = SampleCircuit()
        job_sim = execute(qc, backend, shots=shots, memory=memory)
        sim_result = job_sim.result()
        result[i] = sim_result.get_counts(qc)

    return result


def run_circuit(circuitString:str,backend_to_run="qasm_simulator", api_token=None, shots=1024, memory=False):

    if api_token:
        IBMQ.enable_account(api_token, 'https://api.quantum-computing.ibm.com/api/Hubs/ibm-q/Groups/open/Projects/main')
    
    circuitString = "def get_qc():" + circuitString

    print(circuitString)



    exec(circuitString,globals())
    qc = globals()['get_qc']()
    print(qc)

    backend = Aer.get_backend(backend_to_run)
    job_sim = execute(qc,backend,shots=shots,memory=memory)
    sim_result = job_sim.result()
    result = sim_result.get_counts(qc)


    return result
 

#SampleCircuit of bell pair. Just for testing.
#this is where you implement your circuit
def SampleCircuit():
    qc = RandomGenerator()

    return qc

def backend_configuration(backend_to_run="qasm_simulator"):
    backend = Aer.get_backend(backend_to_run)
    return backend.configuration().as_dict()