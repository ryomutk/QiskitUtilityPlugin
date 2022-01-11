using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class QiskitTestor : MonoBehaviour
{
    [SerializeField] TMP_Text resultField;
    [SerializeField] TMP_Text stateVector;
    [SerializeField] TMP_Text probTables;

    void Start()
    {
        StartCoroutine(ScreenUpdateRoutine());
    }


    IEnumerator ScreenUpdateRoutine()
    {
        var circuitManager = DataProvider.instance.circuitManager;
        List<ITask> tasks = new List<ITask>();
        while (true)
        {
            yield return new WaitUntil(()=>!circuitManager.updatedToHead);
            yield return new WaitUntil(()=>circuitManager.updatedToHead);

            var qc = circuitManager.BuildCircuit();
            var probTask = qc.GetStateProbAsync();
            var vecTask = qc.GetStateVectorAsync();

            yield return new WaitUntil(()=>probTask.ready);
            yield return new WaitUntil(()=>vecTask.ready);

            stateVector.text = vecTask.result;
            probTables.text = string.Join(",",probTask.result);
            Debug.Log("updated");
        }

    }
    public void RunCircuit()
    {
        var task = DataProvider.instance.circuitManager.BuildCircuit().RunAsync();

        StartCoroutine(CircuitTask(task));
    }

    IEnumerator CircuitTask(ITask<CircuitMeasurementResult> task)
    {
        yield return new WaitUntil(() => task.ready);
        resultField.text = task.result.GetOverview();
    }

}
