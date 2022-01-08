#!/usr/bin/env python3
import numpy as np
from qiskit import QuantumRegister, ClassicalRegister
from qiskit import QuantumCircuit, Aer, execute


def RandomGenerator():
    qr = QuantumRegister(1)
    cr = ClassicalRegister(1)
    qc = QuantumCircuit(qr,cr)

    qc.h(0)
    qc.measure(qr,cr)

    return qc