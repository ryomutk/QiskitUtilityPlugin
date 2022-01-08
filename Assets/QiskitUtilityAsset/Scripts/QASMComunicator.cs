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

    void Test()
    {

        /*
        @"include ""qelib1.inc""; 
        qreg q[10]; 
        creg c[10]; 
        h q[0];
        h q[1]; 
        h q[2]; 
        h q[3]; 
        h q[4]; 
        h q[5];
        h q[6]; 
        h q[7]; 
        h q[8]; 
        h q[9]; 
        measure q[0] -> c[0]; 
        measure q[1] -> c[1]; 
        measure q[2] -> c[2]; 
        measure q[3] -> c[3]; 
        measure q[4] -> c[4];
        measure q[5] -> c[5]; 
        measure q[6] -> c[6]; 
        measure q[7] -> c[7]; 
        measure q[8] -> c[8]; 
        measure q[9] -> c[9];";
        */
        var circuit =
@"
    qr = QuantumRegister(10)
    cr = ClassicalRegister(10)
    qc = QuantumCircuit(qr,cr)

    for i in range(10):
        qc.h(i)

    qc.measure(qr,cr)

    return qc";

        var task = new SmallTask<QuantumResult>();
        StartCoroutine(RunCircuit(circuit, task));
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