using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class Player2Coop : MonoBehaviour, IDamageable
{
    public static float speed = 6;
    public static float rateOfFire = 1f; // How fast player can throw balls
    public int lives = 3;
    private float timeSinceLastShot;

    private Bounds bgBounds;
    private Bounds playerBounds;
    private SpriteRenderer sprite;

    private GameObject ball;
    public Image heart3;
    public Image heart2;

    public GameObject pauseCanvas;

    void Start()
    {
        bgBounds = GameObject.Find("Background").GetComponent<BoxCollider2D>().bounds;
        playerBounds = GetComponent<BoxCollider2D>().bounds;
        sprite = GetComponent<SpriteRenderer>();
        ball = Resources.Load<GameObject>("Prefabs/Ball");
    }

    void FixedUpdate()
    {
        bgBounds = GameObject.Find("Background").GetComponent<BoxCollider2D>().bounds;
        if (Input.GetKey(KeyCode.Escape))
        {
            Manager.Instance.GetComponent<AudioSource>().Pause();
            pauseCanvas.SetActive(true);
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(EventSystem.current.firstSelectedGameObject);
            Time.timeScale = 0;
        }
        move();
        throwBall();
    }

    private void move()
    {
        if (Input.GetKey(KeyCode.LeftArrow)) // Move right
        {
            if (transform.position.x - (playerBounds.size.x / 2) > bgBounds.min.x)
                transform.Translate(Vector2.left * Time.deltaTime * speed);
        }
        else if (Input.GetKey(KeyCode.RightArrow)) // Move left
        {
            if (transform.position.x + (playerBounds.size.x / 2) < 1)
                transform.Translate(Vector2.right * Time.deltaTime * speed);
        }

        if (Input.GetKey(KeyCode.DownArrow)) // Move down
        {
            if (transform.position.y - (playerBounds.size.y / 2) > bgBounds.min.y)
                transform.Translate(Vector2.down * Time.deltaTime * speed);
        }
        else if (Input.GetKey(KeyCode.UpArrow)) // Move up
        {
            if (transform.position.y + (playerBounds.size.y / 2) < bgBounds.max.y)
                transform.Translate(Vector2.up * Time.deltaTime * speed);
        }
    }

    private void throwBall()
    {
        if (Time.time > timeSinceLastShot && Input.GetKeyDown(KeyCode.K)) // A short cooldown when player throws a ball
        {
            timeSinceLastShot = Time.time + rateOfFire;

            GameObject ballCopy = Instantiate(ball); // Instantiate a ball object and "throw" it
            Ball ballScript = ballCopy.GetComponent<Ball>();
            ballScript.thrownByPlayer1 = true;
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
            Manager.endGame(SceneManager.GetActiveScene().name);
            SceneManager.LoadScene("AiWin");
        }
    }

    IEnumerator Flash()
    {
        sprite.color = Color.red;
        yield return new WaitForSeconds(.1f);
        sprite.color = Color.white;
    }
}