using System.Text;
using System.Collections.Generic;
using System.Linq;
using QiskitPlugin.Internal;
using QiskitPlugin.Config;

namespace QiskitPlugin
{
    public class QuantumCircuit
    {
        StringBuilder circuitStr = new StringBuilder();
        int registerBit;

        public QuantumCircuit(int registerNum)
        {
            circuitStr.AppendLine();
            circuitStr.AppendLine("    qr = QuantumRegister(" + registerNum + ")");
            circuitStr.AppendLine("    cr = ClassicalRegister(" + registerNum + ")");
            circuitStr.AppendLine("    qc = QuantumCircuit(qr,cr)");
            registerBit = registerNum;
        }

        public bool AppendGate(Gates type, params object[] arguments)
        {
            //必ず一つインデントが入る
            var indent = 1;
            for (int i = 0; i < indent; i++)
            {
                circuitStr.Append("    ");
            }

            if (QASMComunicator.instance.gateSetting.methodDifine.TryGetItem(type, out string methodStr))
            {
                circuitStr.AppendFormat(methodStr, arguments.Select(x => x.ToString()).ToArray());
                circuitStr.AppendLine();
                return true;
            }
            else
            {
                throw new System.Exception("Multiple arg method of gate type " + type + " is not defined!");
            }
        }

        /// <summary>
        /// Get dictionry of probability of which states will likely be measured when
        /// this circuit is executed.
        /// </summary>
        /// <returns></returns>
        public SmallTask<Dictionary<string, float>> GetStateProbAsync()
        {
            var str = circuitStr.ToString();
            str += "\n    return qc";

            return QASMComunicator.instance.GetProbability(str);
        }


        /// <summary>
        /// Get state vector of this Quantum Circuit in Bra-ket notation
        /// </summary>
        /// <returns></returns>
        public SmallTask<string> GetStateVectorAsync()
        {
            var str = circuitStr.ToString();
            str += "\n    return qc";

            return QASMComunicator.instance.GetStateVector(str);
        }

        /// <summary>
        /// Get summary string of this Quamtum Circuit
        /// </summary>
        /// <returns></returns>
        public SmallTask<string> GetCircuitSummaryAsync()
        {
            var str = circuitStr.ToString();
            str += "\n    qc.measure(qr,cr)";
            str += "\n    return qc";
            return QASMComunicator.instance.GetCircuitSummary(str);
        }

        bool CheckTargetBit(int targetQbit)
        {
            return targetQbit >= 0 && registerBit > targetQbit;
        }
        public SmallTask<CircuitMeasurementResult> RunAsync()
        {
            var str = circuitStr.ToString();
            str += "\n    qc.measure(qr,cr)";
            str += "\n    return qc";
            return QASMComunicator.instance.RunCircuitAsync(str);
        }

    }
}