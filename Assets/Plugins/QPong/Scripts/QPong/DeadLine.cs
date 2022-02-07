using UnityEngine;

public class DeadLine : MonoBehaviour
{
    [SerializeField] Transform respawnPos;
    [SerializeField] Actor master;
    void OnTriggerEnter2D(Collider2D other)
    {
        var ball = other.gameObject.GetComponent<Ball>();
        if (ball != null)
        {
            EventManager.instance.Notice(EventName.BallEvent,new BallEventArg(BallAction.dead));
            EventManager.instance.Notice(EventName.DamageEvent,new DamageEventArg(1,master));
            ball.transform.position = respawnPos.position;
        }
    }
}