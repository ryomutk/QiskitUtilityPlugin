using UnityEngine;

[CreateAssetMenu(menuName = "config/GateSetting")]
public class GateSetting:SingleScriptableObject<GateSetting>
{
    public SerializableDictionary<Gates,string> methodDifine{get{return _methodDefine;}}
    [SerializeField]SerializableDictionary<Gates,string> _methodDefine;

}