using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "QPongConfig")]
public class QPongConfig : SingleScriptableObject<QPongConfig>
{
    [SerializeField]int _register = 3;
    [SerializeField]float _enemySpeed = 0.02f;
    
    //Friction coefficient of every bouncy item will be rondomized on this range on every ball hit
    //壁以外の反射物体の摩擦係数がボールがヒットするたびに+-この値でランダマイズされます
    [SerializeField]float _frictionRandomRange = 0.2f;
    //プレイヤーのバーにボールが当たった後にメジャーメントの結果を表示しておく時間。
    [SerializeField]float _measurmentShowTime = 1.5f;
    public float measurmentShowTime{get{return _measurmentShowTime;}}
    public float frictionRandomRange{get{return _frictionRandomRange;}}
    public float enemySpeed{get{return _enemySpeed;}}
}
