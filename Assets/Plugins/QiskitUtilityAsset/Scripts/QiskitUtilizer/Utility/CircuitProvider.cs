using UnityEngine;
using System.Collections.Generic;
namespace QiskitPlugin.Utility
{
    public class CircuitProvider : Singleton<CircuitProvider>
    {
        [SerializeField] ScriptableObject[] requireScriptables;
        List<CircuitBuilder> _circuits = new List<CircuitBuilder>();
        public List<CircuitBuilder> circuits { get { return _circuits; } }

        protected override void Awake()
        {
            base.Awake();
        }

        /// <summary>
        /// CreatesNewCircuitBuilder
        /// </summary>
        /// <param name="registerBit"></param>
        /// <param name="circuit">outputs new CircuitBuilder</param>
        /// <returns>index of created circuit</returns>
        public int CreateCircuit(int registerBit,out CircuitBuilder circuit)
        {
            circuit = new CircuitBuilder(registerBit);
            _circuits.Add(circuit);

            return _circuits.Count-1;
        }
    }
}