using System.Data;
using UnityEngine;

namespace QiskitPlugin.Config
{
    [CreateAssetMenu(menuName = "config/Circuit")]
    public class CircuitConfig : ScriptableObject
    {
        [SerializeField] int _shots = 1024;
        [SerializeField] int _register = 3;
        [SerializeField] string _backend = "qasm_simulator";
        public int shots { get { return _shots; } }
        public string backend{get{return _backend;}}
    }
}