using UnityEngine;
using QiskitPlugin.UI;

public class QPInputUtility : QiskitGUI
{
    [SerializeField] GateIndicator indicator;

    protected override void AfterUpdate()
    {
        indicator.UpdateState();
    }
}