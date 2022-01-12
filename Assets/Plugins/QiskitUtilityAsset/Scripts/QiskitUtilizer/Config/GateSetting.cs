using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
namespace QiskitPlugin.Config
{
    [CreateAssetMenu(menuName = "config/GateSetting")]
    public class GateSetting : ScriptableObject
    {
        public static Gates InputToGate(Input.InputID id)
        {
            return allGates.First(x=>x.ToString()==id.ToString());
        }
        static Gates[] allGates;
        void OnValidate()
        {
            allGates = Enum.GetValues(typeof(Gates)) as Gates[];
        }
        public SerializableDictionary<Gates, string> methodDifine { get { return _methodDefine; } }
        [SerializeField] SerializableDictionary<Gates, string> _methodDefine;
    }

}