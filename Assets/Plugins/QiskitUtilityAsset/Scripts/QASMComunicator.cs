using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        StartCoroutine(SimulateCircuit(circuit, task));

        return task;
    }

    public SmallTask<Dictionary<string,float>> GetProbability(string circuit)
    {
        var task = new SmallTask<Dictionary<string,float>>();
        StartCoroutine(GetStateProbability(circuit,task));
        return task;
    }

    IEnumerator GetStateProbability(string circuitString, SmallTask<Dictionary<string, float>> task)
    {
        var task2 = new SmallTask<string>();
        yield return StartCoroutine(SimulateCircuit(circuitString, task2,false));
        
        task.result = JsonConvert.DeserializeObject<Dictionary<string,float>>(task2.result);
    }


    IEnumerator SimulateCircuit(string circuitString, SmallTask<string> task, bool getStateVector = true)
    {
        var formData = new List<IMultipartFormSection>();
        formData.Add(new MultipartFormDataSection("circuit", circuitString));

        UnityWebRequest www = null;
        if (getStateVector)
        {
            www = UnityWebRequest.Post("http://127.0.0.1:8001/api/simulate/statevector", formData);
        }
        else
        {
            www = UnityWebRequest.Post("http://127.0.0.1:8001/api/simulate/probability", formData);
        }

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