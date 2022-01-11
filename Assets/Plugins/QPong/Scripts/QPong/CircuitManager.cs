using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class CircuitManager
{
    Dictionary<int, Dictionary<int, Gates>> circuitTable = new Dictionary<int, Dictionary<int, Gates>>();
    int register;
    SmallTask<Dictionary<string, float>>[] taskDoubleBuffer = new SmallTask<Dictionary<string, float>>[] { null, null };
    public bool updatedToHead
    {
        get
        {
            SolveBuffer();
            return taskDoubleBuffer[1] != null;
        }
    }
    public Dictionary<string, float> stateSummary
    {
        get
        {
            SolveBuffer();
            return taskDoubleBuffer[0].result;
        }
    }

    void SolveBuffer()
    {
        if (taskDoubleBuffer[1] != null && taskDoubleBuffer[1].ready)
        {
            taskDoubleBuffer[0] = taskDoubleBuffer[1];
            taskDoubleBuffer[1] = null;
        }
    }

    public CircuitManager(int register)
    {
        this.register = register;
        for (int i = 0; i < register; i++)
        {
            circuitTable[i] = new Dictionary<int, Gates>();
        }
        taskDoubleBuffer[0] = new SmallTask<Dictionary<string, float>>();
        taskDoubleBuffer[0].result = new Dictionary<string, float>() { { "000", 1f } };
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
        var qc = BuildCircuit();
        var task = qc.GetStateProbAsync();

        taskDoubleBuffer[1] = task;
    }

    public QuantumCircuit BuildCircuit()
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
                    qc.AppendGate(gate.Value, num);
                }
            }
        }

        return qc;
    }

    public SmallTask<CircuitMeasurementResult> BuildAndRunAsync()
    {
        var qc = BuildCircuit();
        return qc.RunAsync();
    }

}