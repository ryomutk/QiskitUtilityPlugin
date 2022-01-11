using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QiskitTestor : MonoBehaviour
{
    QuantumCircuit circuit;

    void Start()
    {
        circuit = new QuantumCircuit(3);
        circuit.AppendGate(Gates.H,0);
        circuit.AppendGate(Gates.H,1);
        circuit.AppendGate(Gates.CX,0,1);
    }


    public void RunCircuit()
    {
        var task = circuit.GetStateVectorAsync();
        StartCoroutine(CircuitTask(task));
    }

    IEnumerator CircuitTask(ITask task)
    {
        yield return new WaitUntil(() => task.ready);
    }

}
