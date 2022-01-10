using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class CircuitManager
{
    Dictionary<int, Dictionary<int, Gates>> circuitTable = new Dictionary<int, Dictionary<int, Gates>>();
    int register;
    public Dictionary<int, float> stateSummary = new Dictionary<int, float>();

    public CircuitManager(int register)
    {
        this.register = register;
        for (int i = 0; i < register; i++)
        {
            circuitTable[i] = new Dictionary<int, Gates>();
            stateSummary[i] = 0;
        }
        
    }

    public Gates CheckGate(int qbit, int order)
    {
        if (circuitTable[qbit].TryGetValue(order, out var gate))
        {
            return gate;
        }
        return Gates.none;
    }

    public bool AppendAt(int qnum, int orderNum, Gates gate)
    {
        circuitTable[qnum][orderNum] = gate;
        UpdateSummary();
        return true;
    }

    public bool RemoveAt(int qnum, int orderNum)
    {
        circuitTable[qnum][orderNum] = Gates.none;
        UpdateSummary();
        return true;
    }

    void UpdateSummary()
    {
        foreach (var line in circuitTable)
        {
            if (line.Value.ContainsValue(Gates.H))
            {
                stateSummary[line.Key] = 0.5f;
            }
            else
            {
                var count = line.Value.Count(x => x.Value == Gates.X);
                if (count % 2 == 0)
                {
                    stateSummary[line.Key] = 0;
                }
                else
                {
                    stateSummary[line.Key] = 1;
                }
            }
        }
    }

    public SmallTask<CircuitMeasurementResult> BuildAndRunAsync()
    {
        var qc = new QuantumCircuit(register);
        foreach (var line in circuitTable)
        {
            var num = line.Key;
            line.Value.OrderBy(x => x.Key);
            foreach (var gate in line.Value)
            {
                if (gate.Value != Gates.none)
                {
                    qc.AppendGate(gate.Value,num);
                }
            }
        }

        return qc.RunAsync();
    }

}