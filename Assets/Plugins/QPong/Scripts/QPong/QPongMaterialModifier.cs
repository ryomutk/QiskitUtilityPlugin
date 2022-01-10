using UnityEngine;
[RequireComponent(typeof(Collider2D))]
public class QPongMaterialModifier:MonoBehaviour
{
    new Collider2D collider;
    void Awake()
    {
        collider = GetComponent<Collider2D>();
    }

    void OnCollisionEnter2D(Collision2D collisionInfo)
    {
        if(collisionInfo.gameObject.tag == "Ball")
        {
            var friction = Random.Range(-QPongConfig.instance.frictionRandomRange,QPongConfig.instance.frictionRandomRange);
            collisionInfo.collider.sharedMaterial.friction = friction;
        }
    }
}