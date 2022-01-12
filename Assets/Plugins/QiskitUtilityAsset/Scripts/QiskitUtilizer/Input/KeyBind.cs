using UnityEngine;
using System;

namespace QiskitPlugin.Input
{
    [Serializable]
    public class KeyBind
    {
        [SerializeField] public KeyCode key;
        [SerializeField] public InputID id;
    }
}