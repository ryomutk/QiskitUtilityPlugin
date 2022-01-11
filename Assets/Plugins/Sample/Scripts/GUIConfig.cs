using UnityEngine;
using System.Collections.Generic;
[CreateAssetMenu(menuName = "config/GUIConfig")]
public class GUIConfig : SingleScriptableObject<GUIConfig>
{
    public Vector2 gap { get { return _gap; } }
    public int orderNum { get { return _orderNum; } }
    public List<KeyBind> keyConfig;
    [SerializeField] Vector2 _gap;
    [SerializeField] int _orderNum = 9;

}