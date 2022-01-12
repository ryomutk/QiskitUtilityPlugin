using System;
using System.Security.Cryptography.X509Certificates;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using QiskitPlugin.Config;
using QiskitPlugin.Internal;

namespace QiskitPlugin.Utility
{
    public class CircuitBuilder
    {
        static List<Gates> handableControllGates = new List<Gates>(){Gates.CX,Gates.CZ};
        //<Qubit index<Order in circuit,GateType>>
        Dictionary<int, Dictionary<int, Gates>> circuitTable = new Dictionary<int, Dictionary<int, Gates>>();
        //memorize relations of multi xxx gate.(or CX,CZ gate)
        List<MultiControllData> multiControllData = new List<MultiControllData>();
        public bool IsTargetQubit(int order,int registerNum)
        {
            return multiControllData.Any(x=>x.Contains(order,registerNum)&&x.targetQbit==registerNum);
        }
        GateSetting gateSetting { get { return QASMComunicator.instance.gateSetting; } }
        class MultiControllData
        {
            public Gates type;
            public int orderNum;
            public int targetQbit;
            public int[] controllQubits;
            public bool Contains(int orderNum, int qbit)
            {
                return this.orderNum == orderNum && (targetQbit == orderNum || controllQubits.Contains(qbit));
            }
        }
        public int register{get;private set;}

        /// <summary>
        /// 収束確率の
        /// </summary>
        /// <value></value>
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

        public CircuitBuilder(int register)
        {
            this.register = register;
            for (int i = 0; i < register; i++)
            {
                circuitTable[i] = new Dictionary<int, Gates>();
            }
            taskDoubleBuffer[0] = new SmallTask<Dictionary<string, float>>();
            taskDoubleBuffer[0].result = new Dictionary<string, float>() { { "000", 1f } };
        }

        public Gates CheckGate(int order, int registerNum)
        {
            if (circuitTable[registerNum].TryGetValue(order, out var gate))
            {
                return gate;
            }
            return Gates.none;
        }

        public bool AppendAt(int orderNum, int registerNum, Gates gate)
        {
            if(IsControllGate(gate))
            {
                return false;
                throw new System.Exception("Gate "+gate+" is Controll Gate! Use Append CGate otherwise.");
            }
            circuitTable[registerNum][orderNum] = gate;
            UpdateSummary();
            return true;
        }

        public bool RemoveAt(int orderNum, int registerNum)
        {
            var target = circuitTable[registerNum][orderNum];

            //消そうとしているものがnコントロールゲートなら
            if (IsControllGate(target))
            {
                //関連するゲートすべて消す
                var data = multiControllData.First(x => x.Contains(orderNum, registerNum));
                if (data == null)
                {
                    return false;
                    throw new System.Exception("There is no multi gate on O,R:" + orderNum + "," + registerNum);
                }

                circuitTable[data.targetQbit][orderNum] = Gates.none;
                foreach (var controll in data.controllQubits)
                {
                    circuitTable[controll][orderNum] = Gates.none;
                }

                //関連性も消す
                multiControllData.Remove(data);
            }
            else
            {
                circuitTable[registerNum][orderNum] = Gates.none;
            }
            UpdateSummary();
            return true;
        }

        public bool AppendMultiCGate(Gates gate, int orderNum, int targetQbit, params int[] controllQbits)
        {
            if (multiControllData.Any(x => x.Contains(orderNum, targetQbit)))
            {
                RemoveAt(orderNum, targetQbit);
            }

            circuitTable[targetQbit][orderNum] = gate;
            foreach (var controll in controllQbits)
            {
                if (!ValidateQubitIndex(controll))
                {
                    return false;
                    throw new System.Exception("Out of index");
                }

                if (multiControllData.Any(x => x.Contains(orderNum, controll)))
                {
                    RemoveAt(orderNum, controll);
                }
                circuitTable[controll][orderNum] = gate;
            }
            multiControllData.Add(new MultiControllData() { type = gate, orderNum = orderNum, targetQbit = targetQbit, controllQubits = controllQbits });

            return true;
        }

        bool ValidateQubitIndex(int target)
        {
            return 0 <= target && target < register;
        }

        void UpdateSummary()
        {
            var qc = BuildCircuit();
            var task = qc.GetStateProbAsync();

            taskDoubleBuffer[1] = task;
        }

        public bool IsControllGate(Gates gate)
        {
            return handableControllGates.Contains(gate);
        }

        public QuantumCircuit BuildCircuit()
        {
            var qc = new QuantumCircuit(register);
            foreach (var registerLine in circuitTable)
            {
                var num = registerLine.Key;
                registerLine.Value.OrderBy(x => x.Key);
                foreach (var orderedGate in registerLine.Value)
                {
                    if (orderedGate.Value != Gates.none)
                    {
                        if (IsControllGate(orderedGate.Value))
                        {
                            var data = multiControllData.First(x => x.Contains(orderedGate.Key, registerLine.Key));
                            if (data == null) { throw new System.Exception("Couldn't find your controll"); }

                            //if ControllQubit
                            if (data.targetQbit != registerLine.Key)
                            {
                                //Skip
                                continue;
                            }
                            //if targetQbit
                            else
                            {
                                qc.AppendGate(orderedGate.Value,data.controllQubits,data.targetQbit);
                            }
                        }
                        else
                        {
                            qc.AppendGate(orderedGate.Value, num);
                        }

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
}