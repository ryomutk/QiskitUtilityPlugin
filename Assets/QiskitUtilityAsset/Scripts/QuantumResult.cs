using System.Collections.Generic;
using System;
using Newtonsoft.Json;

public class QuantumResult
{
    public Dictionary<string, int> rawData { get; private set; }
    //各々のQubitの存在確立
    public Dictionary<int, float> probTable { get; set; }
    public int bitLength { get; private set; }
    public string resultString{get;private set;}

    //get Existence probability of selected qubit
    public float GetProbability(int n)
    {
        if (n >= bitLength)
        {
            return 0;
        }
        else if (probTable.ContainsKey(n))
        {
            return probTable[n];
        }

        return 0;
    }

    public int totalShots { get; private set; }
    public QuantumResult(Dictionary<string, int> rawData)
    {
        this.rawData = rawData;
        InitData();
    }

    public QuantumResult(string resultStr)
    {
        this.rawData = JsonConvert.DeserializeObject<Dictionary<string, int>>(resultStr);
        this.resultString = resultStr;

        InitData();
    }

    void InitData()
    {
        probTable = new Dictionary<int, float>();
        foreach (var data in rawData)
        {
            bitLength = data.Key.Length;
            totalShots += data.Value;
        }

        Dictionary<int, int> countTable = new Dictionary<int, int>();
        for (int i = 0; i < bitLength; i++)
        {
            countTable[i] = 0;
        }
        foreach (var item in rawData)
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
            probTable[item.Key] = item.Value / (float)totalShots;
        }
    }

    public float GetRate(int flags)
    {
        foreach (var data in rawData)
        {
            if (flags == int.Parse(data.Key))
            {
                return data.Value / totalShots;
            }
        }

        return 0;
    }

}