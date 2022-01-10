using UnityEngine;

public class Ball : MonoBehaviour
{
    Rigidbody2D body;
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        body.AddForce(-Vector2.one * 1000);
        body.sharedMaterial.friction = 0;
    }

    public void Restart()
    {
        body.velocity = Vector2.zero;
        body.AddForce(-Vector2.one * 1000);
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