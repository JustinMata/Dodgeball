using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public Vector2 velocity = new Vector2(10, 0); // Move ten units to right in 1 second
    public bool thrownByPlayer1;

    private Bounds bgBounds;
    private Bounds ballBounds;

    void Start()
    {
        bgBounds = GameObject.Find("Background").GetComponent<SpriteRenderer>().bounds;
        ballBounds = GetComponent<SpriteRenderer>().bounds;

        GetComponent<Rigidbody2D>().velocity = velocity;

        StartCoroutine(lifespan());
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "boy" && !thrownByPlayer1) // If collided w boy and not thrown by the boy (thrown by the girl)
        {
            collision.GetComponent<IDamageable>().takeDamage(); // boy takes damage
            Destroy(gameObject);
        }
        else if (collision.tag == "girl" && thrownByPlayer1) // If collided w girl and not thrown by the girl (thrown by the boy)
        {
            collision.GetComponent<IDamageable>().takeDamage(); // girl takes damage
            Destroy(gameObject);
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "ball")
        {
            GetComponent<AudioSource>().Play();
        }
    }
    
    IEnumerator lifespan()
    {
        yield return new WaitForSeconds(5);
        Destroy(gameObject);
    }
}
