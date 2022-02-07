using UnityEngine;

public class Ball : MonoBehaviour
{
    Rigidbody2D body;
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        body.sharedMaterial.friction = 0;
    }

    void OnCollisionEnter(Collision collisionInfo)
    {
        EventManager.instance.Notice(EventName.BallEvent, new BallEventArg(BallAction.bounce));
    }

    public void Restart()
    {
        body.velocity = Vector2.zero;
        var rand = Random.Range(-20, 20);
        body.sharedMaterial.friction = 0;
        body.AddForce((Quaternion.Euler(0, 0, rand) * Vector2.left) * QPongConfig.instance.ballShotPower);
    }

    Vector2 lastVelo;
    public void Pause()
    {
        lastVelo = body.velocity;
        body.velocity = Vector2.zero;
    }
    public void Resume()
    {
        body.velocity = lastVelo;
    }
}