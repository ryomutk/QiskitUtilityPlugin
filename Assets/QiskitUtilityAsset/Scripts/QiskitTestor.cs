using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QiskitTestor : MonoBehaviour
{
    QuantumCircuit circuit;

    void Start()
    {
        circuit = new QuantumCircuit(3);
        circuit.AppendGate(0, Gates.H);
        circuit.AppendGate(1, Gates.H);
        circuit.AppendGate(2, Gates.H);
        circuit.AppendGate(0,1,Gates.CX);
    }


    public void RunCircuit()
    {
        var task = circuit.RunAsync();
        StartCoroutine(CircuitTask(task));
    }

    IEnumerator CircuitTask(ITask<CircuitMeasurementResult> task)
    {
        yield return new WaitUntil(() => task.ready);
        Debug.Log(task.result.resultString);
        Debug.Log("probs:");
        foreach(var data in task.result.bitProbTable)
        {
            Debug.Log("Bit num:"+data.Key+" Prob:"+data.Value);
        }
    }

}
