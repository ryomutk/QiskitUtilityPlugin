using UnityEngine;

public class LifeArea:MonoBehaviour
{
    [SerializeField] TMPro.TMP_Text playerLife;
    [SerializeField] TMPro.TMP_Text enemyLife;

    void Update()
    {
        playerLife.text = QPongManager.instance.lifetable[Actor.player].ToString();
        enemyLife.text = QPongManager.instance.lifetable[Actor.enemy].ToString();
    }
}