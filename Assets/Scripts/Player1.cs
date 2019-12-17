using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player1 : MonoBehaviour, IDamageable
{
    public static float speed = 6;
    public static float rateOfFire = 1f; // How fast player can throw balls
    public int lives = 3;
    private float timeSinceLastShot;
    private float bgShrinkTimer;
    private float time;

    public GameObject bg;
    private Bounds bgBounds;
    private Bounds playerBounds;
    private SpriteRenderer sprite;

    private GameObject ball;
    public Image heart3;
    public Image heart2;

    void Start()
    {
        bgBounds = bg.GetComponent<BoxCollider2D>().bounds;
        playerBounds = GetComponent<BoxCollider2D>().bounds;
        sprite = GetComponent<SpriteRenderer>();
        ball = Resources.Load<GameObject>("Prefabs/Ball");
        time = 0;
        bgShrinkTimer = 20;
    }

    void FixedUpdate()
    {
        time += Time.deltaTime;
        if (time > bgShrinkTimer && bg.transform.localScale.x - 1 != 1)
        {
            StartCoroutine(ScaleToTarget(new Vector3(bg.transform.localScale.x - 1, bg.transform.localScale.y - 1, bg.transform.localScale.z), 10));
            time = 0;
        }
        move();
        throwBall();
    }

    private void move()
    {
        if (Input.GetKey(KeyCode.A)) // Move right
        {
            if (transform.position.x - (playerBounds.size.x / 2) > bgBounds.min.x)
                transform.Translate(Vector2.left * Time.deltaTime * speed);
        }
        else if (Input.GetKey(KeyCode.D)) // Move left
        {
            if (transform.position.x + (playerBounds.size.x / 2) < 1)
                transform.Translate(Vector2.right * Time.deltaTime * speed);
        }

        if (Input.GetKey(KeyCode.S)) // Move down
        {
            if (transform.position.y - (playerBounds.size.y / 2) > bgBounds.min.y)
                transform.Translate(Vector2.down * Time.deltaTime * speed);
        }
        else if (Input.GetKey(KeyCode.W)) // Move up
        {
            if (transform.position.y + (playerBounds.size.y / 2) < bgBounds.max.y)
                transform.Translate(Vector2.up * Time.deltaTime * speed);
        }
    }

    private void throwBall()
    {
        if (Time.time > timeSinceLastShot && Input.GetKeyDown(KeyCode.Space)) // A short cooldown when player throws a ball
        {
            timeSinceLastShot = Time.time + rateOfFire;

            GameObject ballCopy = Instantiate(ball); // Instantiate a ball object and "throw" it
            ballCopy.GetComponent<Ball>().thrownByPlayer1 = true;
            ballCopy.transform.position = gameObject.transform.position;
        }
    }

    public void takeDamage()
    {
        if (lives == 3)
        {
            GetComponent<AudioSource>().Play();
            lives--;
            heart3.enabled = false;
            StartCoroutine(Flash());
        }
        else if (lives == 2)
        {
            GetComponent<AudioSource>().Play();
            lives--;
            heart2.enabled = false;
            StartCoroutine(Flash());
        }
        else
        {
            if (SceneManager.GetActiveScene().name == "2AIvs2Players" || SceneManager.GetActiveScene().name == "AIvsPlayer")
            {
                Manager.endGame(SceneManager.GetActiveScene().name);
                SceneManager.LoadScene("AiWin");
            }
            else
            {
            Manager.endGame(SceneManager.GetActiveScene().name);
            SceneManager.LoadScene("Player2Win");
            }
        }
    }

    IEnumerator Flash()
    {
        sprite.color = Color.red;
        yield return new WaitForSeconds(.1f);
        sprite.color = Color.white;
    }

    private IEnumerator ScaleToTarget(Vector3 targetScale, float duration)
    {
        Vector3 startScale = bg.transform.localScale;
        float timer = 0.0f;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            float t = timer / duration;
            //smoother step algorithm
            t = t * t * t * (t * (6f * t - 15f) + 10f);
            bg.transform.localScale = Vector3.Lerp(startScale, targetScale, t);
            bgBounds = bg.GetComponent<BoxCollider2D>().bounds;
            yield return null;
        }

        yield return null;
    }
}