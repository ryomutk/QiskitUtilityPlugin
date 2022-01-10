using UnityEngine;

public class DeadLine : MonoBehaviour
{
    [SerializeField] Transform respawnPos;
    void OnTriggerEnter2D(Collider2D other)
    {
        var ball = other.gameObject.GetComponent<Ball>();
        if (ball != null)
        {
            ball.transform.position = respawnPos.position;
            ball.Restart();
        }

    }
}