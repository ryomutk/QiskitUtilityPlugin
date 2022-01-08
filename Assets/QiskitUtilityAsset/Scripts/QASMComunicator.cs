using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

public class QASMComunicator : Singleton<QASMComunicator>
{
    [SerializeField] float amplifier = 1.2f;

    public SmallTask<float[]> GetRandomArrayTask(int shots)
    {
        var task = new SmallTask<float[]>();
        StartCoroutine(GetResult(shots, task));

        return task;
    }

    public SmallTask<QuantumResult> RunCircuitAsync(string method)
    {
        var task = new SmallTask<QuantumResult>();
        StartCoroutine(RunCircuit(method,task));
        return task;
    }

    IEnumerator RunCircuit(string method, SmallTask<QuantumResult> task = null)
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
            var dat = new QuantumResult(result);
            task.result = dat;
        }
        yield return null;
    }

    IEnumerator GetResult(int num, SmallTask<float[]> task)
    {
        var formdata = new List<IMultipartFormSection>();
        formdata.Add(new MultipartFormDataSection("num", num.ToString()));

        UnityWebRequest www = UnityWebRequest.Post("http://127.0.0.1:8001/api/run/randomizer", formdata);

        yield return www.SendWebRequest();

        yield return new WaitUntil(() => www.isDone);

        var result = www.downloadHandler.text;
        var data = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, int>>>(result);

        Debug.Log(result);

        task.result = ParseData(data);
    }

    float[] ParseData(Dictionary<string, Dictionary<string, int>> rawResult)
    {
        var result = new float[rawResult.Count];
        var count = 0;
        foreach (var resultPair in rawResult)
        {
            var percentage = resultPair.Value["0"] * 2 / 1024f - 1;
            percentage *= amplifier;
            result[count] = percentage;
            count++;
        }


        return result;
    }

}