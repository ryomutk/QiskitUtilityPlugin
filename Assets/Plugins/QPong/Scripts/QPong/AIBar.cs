using UnityEngine;

public class AIBar:MonoBehaviour
{
    [SerializeField] Ball target;
    Vector2 moveVector;

    void Start()
    {
        moveVector = new Vector2(0,QPongConfig.instance.enemySpeed);
    }

    void Update()
    {
        var delta = transform.position.y - target.transform.position.y;
        if(delta>0)
        {
            transform.position -= (Vector3)moveVector;
        }
        else
        {
            transform.position += (Vector3)moveVector;
        }
    }
}