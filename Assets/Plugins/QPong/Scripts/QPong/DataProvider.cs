using UnityEngine;

public class DataProvider:Singleton<DataProvider>
{
    [SerializeField] ScriptableObject[] requireScriptables;
    [SerializeField] int _registerNum;
    protected virtual int registerNum{get{return _registerNum;}} 
    CircuitManager _circuitManager;
    public CircuitManager circuitManager{get{return _circuitManager;}}

    protected override void Awake()
    {
        base.Awake();
        _circuitManager = new CircuitManager(registerNum);
    }
}