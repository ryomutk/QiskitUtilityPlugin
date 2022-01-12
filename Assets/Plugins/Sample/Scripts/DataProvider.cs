using UnityEngine;
namespace QiskitPlugin.Utility
{
    public class DataProvider : Singleton<DataProvider>
    {
        [SerializeField] ScriptableObject[] requireScriptables;
        [SerializeField] int _registerNum;
        protected virtual int registerNum { get { return _registerNum; } }
        CircuitBuilder _circuitManager;
        public CircuitBuilder circuitManager { get { return _circuitManager; } }

        protected override void Awake()
        {
            base.Awake();
            _circuitManager = new CircuitBuilder(registerNum);
        }
    }
}