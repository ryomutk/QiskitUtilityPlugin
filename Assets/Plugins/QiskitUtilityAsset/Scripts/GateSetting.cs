using UnityEngine;

[CreateAssetMenu(menuName = "config/GateSetting")]
public class GateSetting:SingleScriptableObject<GateSetting>
{
    public SerializableDictionary<Gates,string> singleArgMethodDefine{get{return _singleArgMethodDifine;}}
    public SerializableDictionary<Gates,string> doubleArgMethodDefine{get{return _doubleArgMethodDefine;}}
    [SerializeField]SerializableDictionary<Gates,string> _singleArgMethodDifine;
    [SerializeField]SerializableDictionary<Gates,string> _doubleArgMethodDefine;

}