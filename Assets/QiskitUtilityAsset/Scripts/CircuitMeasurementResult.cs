using System.Collections.Generic;
using System;
using Newtonsoft.Json;
using System.Linq;

public class CircuitMeasurementResult
{
    public Dictionary<string, int> rawResult { get; private set; }

    /// <summary>
    /// Probabilitys of each qubits appeared in measurement
    /// </summary>
    /// <value></value>
    public Dictionary<int, float> bitProbTable { get; set; }
    public int bitLength { get; private set; }
    public string resultString { get; private set; }
    public int shots { get; private set; }
    public string[] maxStates{get;private set;}
    public int maxCount{get;private set;}

    //get Existence probability of selected qubit
    public float GetProbability(int n)
    {
        if (n >= bitLength)
        {
            return 0;
        }
        else if (bitProbTable.ContainsKey(n))
        {
            return bitProbTable[n];
        }

        return 0;
    }

    public CircuitMeasurementResult(string resultStr)
    {
        this.rawResult = JsonConvert.DeserializeObject<Dictionary<string, int>>(resultStr);
        this.resultString = resultStr;

        InitData();
    }

    void InitData()
    {
        bitProbTable = new Dictionary<int, float>();

        bitLength = rawResult.Max(x=>x.Key.Length);
        shots = rawResult.Sum(x=>x.Value);
        maxCount = rawResult.Max(x=>x.Value);
        maxStates = rawResult.Where(x=>x.Value==maxCount).ToDictionary(x=>x.Key,x=>x.Value).Keys.ToArray();

        Dictionary<int, int> countTable = new Dictionary<int, int>();
        for (int i = 0; i < bitLength; i++)
        {
            countTable[i] = 0;
        }
        foreach (var item in rawResult)
        {
            /*
            var state = int.Parse(item.Key);
            var th = state / 100;
            var sec = (state - th * 100) / 10;
            var mono = state - th * 100 - sec * 10;
            */
            var resultFlag = Convert.ToInt32(item.Key, 2);

            for (int i = 0; i < bitLength; i++)
            {
                var targetBit = (int)Math.Pow(2, i);
                if (BitFlagSolver.HasFlag(resultFlag, targetBit))
                {
                    countTable[i] += item.Value;
                }
            }
        }

        foreach (var item in countTable)
        {
            bitProbTable[item.Key] = item.Value / (float)shots;
        }
    }

    public float GetRate(int flags)
    {
        foreach (var data in rawResult)
        {
            if (flags == int.Parse(data.Key))
            {
                return data.Value / shots;
            }
        }

        return 0;
    }

}