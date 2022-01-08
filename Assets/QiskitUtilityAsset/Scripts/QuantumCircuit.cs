using System.Text;
using System.Collections.Generic;

public class QuantumCircuit
{
    StringBuilder circuitStr = new StringBuilder();
    int registerBit;

    public QuantumCircuit(int registerNum)
    {
        circuitStr.AppendLine();
        circuitStr.AppendLine("    qr = QuantumRegister("+ registerNum +")");
        circuitStr.AppendLine("    cr = ClassicalRegister("+registerNum+")");
        circuitStr.AppendLine("    qc = QuantumCircuit(qr,cr)");
        registerBit = registerNum;
    }

    public bool AppendGate(int targetQbit,Gates type,int indent = 0)
    {
        if(targetQbit<0&&registerBit<=targetQbit)
        {
            throw new System.Exception("target is illegal");
        }

        //必ず一つインデントが入る
        indent++;
        for(int i = 0;i < indent;i++)
        {
            circuitStr.Append("    ");
        }

        if(GateSetting.instance.singleArgMethodDefine.TryGetItem(type,out string methodStr))
        {
            circuitStr.AppendFormat(methodStr,targetQbit);
            circuitStr.AppendLine();
            return true;
        }
        else
        {
            #if DEBUG
            throw new  System.Exception("Single arg method of gate type "+type+" is not defined!");
            #endif
        }
      
    }
    public bool AppendGate(int firstTarget,int secondTarget,Gates type,int indent = 0)
    {
        if(!CheckTargetBit(firstTarget)||!CheckTargetBit(secondTarget))
        {
            throw new System.Exception("target is illegal");
        }

        //必ず一つインデントが入る
        indent++;
        for(int i = 0;i < indent;i++)
        {
            circuitStr.Append("    ");
        }

        if(GateSetting.instance.doubleArgMethodDefine.TryGetItem(type,out string methodStr))
        {
            circuitStr.AppendFormat(methodStr,firstTarget,secondTarget);
            circuitStr.AppendLine();
            return true;
        }
        else
        {
            #if DEBUG
            UnityEngine.Debug.LogWarning("Double arg method of gate type "+type+" is not defined!");
            #endif
            return false;
        }
      
    }

    bool CheckTargetBit(int targetQbit)
    {
        return targetQbit>=0 && registerBit>targetQbit;
    }
    public SmallTask<QuantumResult> RunAsync()
    {
        var str = circuitStr.ToString();
        str += "\n    qc.measure(qr,cr)";
        str += "\n    return qc";
        return QASMComunicator.instance.RunCircuitAsync(str);
    }
    
}