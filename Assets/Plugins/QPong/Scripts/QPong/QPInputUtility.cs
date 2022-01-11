using UnityEngine;

public class QPInputUtility : QiskitGUI
{
    [SerializeField] GateIndicator indicator;

    protected override void AfterUpdate()
    {
        indicator.UpdateState();
    }
}