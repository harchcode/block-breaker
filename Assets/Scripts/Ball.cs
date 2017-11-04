using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public float baseSpeed = 8f;

    private LevelManager levelManager;
    private float speed;
    private Rigidbody2D rb;
    private CircleCollider2D cl;
    private AudioSource sound;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        cl = GetComponent<CircleCollider2D>();
        sound = GetComponent<AudioSource>();

        levelManager = GameObject.FindObjectOfType<LevelManager>();
    }

    public void Reset()
    {
        speed = (levelManager.getLevel() / 8f + 1f) * baseSpeed;
        rb.velocity = new Vector2(0, -5);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        levelManager.Fall();
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        float yDir = rb.velocity.y / speed;

        if (yDir > -0.1f && yDir < 0.1f)
        {
            var newXDir = rb.velocity.x < 0 ? -0.99498743710662f : 0.99498743710662f;
            var newYDir = yDir < 0 ? -0.1f : 0.1f;

            rb.velocity = new Vector2(
                newXDir * speed, newYDir * speed
                );
        }

        if (collision.collider.tag == "Brick")
        {
            sound.Play();
        }
    }

    public void BeginMove()
    {
        rb.simulated = true;
    }

    public void StopMove()
    {
        rb.simulated = false;
    }

    public bool IsMoving()
    {
        return rb.simulated;
    }

    public float GetRadius()
    {
        return cl.radius;
    }

    public void Launch(Vector2 direction)
    {
        rb.velocity = direction * speed;
    }
}