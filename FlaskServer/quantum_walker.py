#!/usr/bin/env python3
from re import sub
import numpy as np
from qiskit import QuantumRegister, ClassicalRegister
from qiskit import QuantumCircuit, Aer, execute

def cnx(qc, *qubits):
    if len(qubits) >= 3:
        last = qubits[-1]
        # A matrix: (made up of a  and Y rotation, lemma4.3)
        qc.crz(np.pi/2, qubits[-2], qubits[-1])
        qc.cu3(np.pi/2, 0, 0, qubits[-2],qubits[-1])
        
        # Control not gate
        cnx(qc,*qubits[:-2],qubits[-1])
        
        # B matrix (pposite angle)
        qc.cu3(-np.pi/2, 0, 0, qubits[-2], qubits[-1])
        
        # Control
        cnx(qc,*qubits[:-2],qubits[-1])
        
        # C matrix (final rotation)
        qc.crz(-np.pi/2,qubits[-2],qubits[-1])
    elif len(qubits)==3:
        qc.ccx(*qubits)
    elif len(qubits)==2:
        qc.cx(*qubits)
        
def increment_gate(qwc, q, subnode):

    for i in range(len(q),-1,-1):
        a = (len(q)-1-i)
        b = (len(q)-1)
        print(*q[:-i-1:-1])
        cnx(qwc,subnode[0],*q[:-i-1:-1])

  #cnx(qwc, subnode[0], q[2], q[1], q[0])
  #cnx(qwc, subnode[0], q[2], q[1])
  #cnx(qwc, subnode[0], q[2])
    qwc.barrier()
    return qwc

def decrement_gate(qwc, q, subnode):
    
  qwc.x(subnode[0])
  
  #num = target
  for targ in range(0,len(q)):
      for ctr in range(targ+1,len(q)):
          qwc.x(q[ctr])

      cnx(qwc,subnode[0],*q[targ+1:len(q)],q[targ])

      for ctr in range(targ+1,len(q)):
          qwc.x(q[ctr])

  qwc.x(subnode[0])
  return qwc
  
def ibmsim(circ,shots):
  ibmqBE = Aer.get_backend('qasm_simulator')
  return execute(circ,ibmqBE, shots=shots).result().get_counts(circ)

num = 3
qnodes = QuantumRegister(num,'qc')
qsubnodes = QuantumRegister(1,'qanc')
csubnodes = ClassicalRegister(1,'canc')
cnodes = ClassicalRegister(num,'cr')
qwc = QuantumCircuit(qnodes, qsubnodes, cnodes, csubnodes)


def resetQWC(n):
    global num
    global qnodes
    global qsubnodes
    global csubnodes
    global cnodes
    global qwc

    num = n
    qnodes = QuantumRegister(n,'qc')
    qsubnodes = QuantumRegister(1,'qanc')
    csubnodes = ClassicalRegister(1,'canc')
    cnodes = ClassicalRegister(n,'cr')
    qwc = QuantumCircuit(qnodes, qsubnodes, cnodes, csubnodes)

    #for i in range(0,int(n/2)):
    #    qwc.x(qnodes[i])

    print("RESET!")


def getQWC(times,repeat = False,n = num):

    #force reset. if n has been changed
    if(n != num or not repeat):
        resetQWC(n)

    
    for i in range(times):
        qwc.h(qsubnodes[0])
        increment_gate(qwc, qnodes, qsubnodes)
        decrement_gate(qwc,qnodes,qsubnodes)
        qwc.measure(qnodes, cnodes)

    return qwc