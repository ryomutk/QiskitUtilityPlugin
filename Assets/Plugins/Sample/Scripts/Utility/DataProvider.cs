using UnityEngine;
namespace QiskitPlugin.Utility
{
    public class DataProvider : Singleton<DataProvider>
    {
        [SerializeField] ScriptableObject[] requireScriptables;
        [SerializeField] int _registerNum;
        protected virtual int registerNum { get { return _registerNum; } }
        CircuitBuilder _circuitBuilder;
        public CircuitBuilder circuitBuilder { get { return _circuitBuilder; } }

        protected override void Awake()
        {
            base.Awake();
            _circuitBuilder = new CircuitBuilder(registerNum);
        }
    }
}