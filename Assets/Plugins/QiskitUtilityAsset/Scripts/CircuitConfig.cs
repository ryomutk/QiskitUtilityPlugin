using UnityEngine;

[CreateAssetMenu(menuName = "config/Circuit")]
public class CircuitConfig:SingleScriptableObject<CircuitConfig>
{
    [SerializeField] int _shots = 1024;
    [SerializeField] int _register = 3;
    public int shots{get{return _shots;}}
}