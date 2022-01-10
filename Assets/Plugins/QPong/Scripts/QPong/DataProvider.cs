using UnityEngine;

public class DataProvider:Singleton<DataProvider>
{
    [SerializeField]QPongConfig config;
    CircuitManager _circuitManager;
    public CircuitManager circuit{get{return _circuitManager;}}

    void Start()
    {
        _circuitManager = new CircuitManager(QPongConfig.instance.register);
    }



}