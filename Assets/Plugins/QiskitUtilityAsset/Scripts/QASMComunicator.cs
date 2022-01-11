using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

public class QASMComunicator : Singleton<QASMComunicator>
{
    [SerializeField] CircuitConfig circuitConfig;
    [SerializeField] GateSetting gateSetting;

    public SmallTask<CircuitMeasurementResult> RunCircuitAsync(string method)
    {
        var task = new SmallTask<CircuitMeasurementResult>();
        StartCoroutine(RunCircuit(method, task));
        return task;
    }

    public SmallTask<string> GetCircuitSummary(string circuit)
    {
        var task = new SmallTask<string>();
        StartCoroutine(GetCircuitSummary(circuit, task));

        return task;
    }

    public SmallTask<string> GetStateVector(string circuit)
    {
        var task = new SmallTask<string>();
        StartCoroutine(GetStatevector(circuit,task));

        return task;
    }

    IEnumerator GetStatevector(string circuitString, SmallTask<string> task)
    {
        var formData = new List<IMultipartFormSection>();
        formData.Add(new MultipartFormDataSection("circuit", circuitString));

        UnityWebRequest www = UnityWebRequest.Post("http://127.0.0.1:8001/api/simulate/statevector", formData);

        yield return www.SendWebRequest();
        yield return new WaitUntil(() => www.isDone);

        var result = www.downloadHandler.text;
        if (task != null)
        {
            task.result = result;
        }

#if DEBUG
        Debug.Log(result);
#endif
    }

    IEnumerator GetCircuitSummary(string circuitString, SmallTask<string> task = null)
    {
        var formData = new List<IMultipartFormSection>();
        formData.Add(new MultipartFormDataSection("circuit", circuitString));

        UnityWebRequest www = UnityWebRequest.Post("http://127.0.0.1:8001/api/summary/circuit", formData);

        yield return www.SendWebRequest();
        yield return new WaitUntil(() => www.isDone);

        var result = www.downloadHandler.text;
        if (task != null)
        {
            task.result = result;
        }

#if DEBUG
        Debug.Log(result);
#endif
    }

    IEnumerator RunCircuit(string method, SmallTask<CircuitMeasurementResult> task = null)
    {
        var formdata = new List<IMultipartFormSection>();
        formdata.Add(new MultipartFormDataSection("circuit", method));
        formdata.Add(new MultipartFormDataSection("shots", CircuitConfig.instance.shots.ToString()));

        UnityWebRequest www = UnityWebRequest.Post("http://127.0.0.1:8001/api/run/circuit", formdata);

        yield return www.SendWebRequest();

        yield return new WaitUntil(() => www.isDone);

        var result = www.downloadHandler.text;

        if (task != null)
        {
            var dat = new CircuitMeasurementResult(result);
            task.result = dat;
#if DEBUG
            var log = dat.GetOverview();
            UnityEngine.Debug.Log(log);
#endif

        }
        yield return null;
    }

}