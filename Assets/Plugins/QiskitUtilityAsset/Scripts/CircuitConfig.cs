using UnityEngine;

[CreateAssetMenu(menuName = "config/Circuit")]
public class CircuitConfig:SingleScriptableObject<CircuitConfig>
{
    [SerializeField] int _shots = 1024;
    public int shots{get{return _shots;}}
}