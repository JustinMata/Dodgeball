using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class AI : MonoBehaviour, IDamageable
{
    public static float speed = 6;
    public static float rateOfFire = 1f; // How fast player can throw balls
    public int lives = 3;
    private float timeSinceLastShot;

    private float latestDirectionChangeTime;
    public float directionChangeTime = .5f;
    private Vector3 targetPosition;

    private Bounds bgBounds;
    private Bounds playerBounds;
    private SpriteRenderer sprite;

    private GameObject ball;
    public Image heart3;
    public Image heart2;

    public GameObject pauseCanvas;
    public Transform player1;
    public Transform player2;

    void Start()
    {
        bgBounds = GameObject.Find("Background").GetComponent<BoxCollider2D>().bounds;
        playerBounds = GetComponent<BoxCollider2D>().bounds;
        sprite = GetComponent<SpriteRenderer>();
        ball = Resources.Load<GameObject>("Prefabs/Ball");

        latestDirectionChangeTime = 0f;
        targetPosition = new Vector3(Random.Range(1, bgBounds.max.x - (playerBounds.size.x / 2)), Random.Range(bgBounds.min.y + (playerBounds.size.x / 2), bgBounds.max.y - (playerBounds.size.x / 2)) );
}

    void Update()
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
        if (Time.time - latestDirectionChangeTime > directionChangeTime)
        {
            latestDirectionChangeTime = Time.time;
            targetPosition = new Vector3(Random.Range(1, bgBounds.max.x), Random.Range(bgBounds.min.y, bgBounds.max.y));
        }
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
    }

    private void throwBall()
    {
        if (Time.time > timeSinceLastShot && player1.position.y > transform.position.y - (playerBounds.size.y) && player1.position.y < transform.position.y + (playerBounds.size.y)) // A short cooldown when player throws a ball
        {
            timeSinceLastShot = Time.time + rateOfFire;

            GameObject ballCopy = Instantiate(ball); // Instantiate a ball object and "throw" it
            Ball ballScript = ballCopy.GetComponent<Ball>();
            ballScript.velocity = -ballScript.velocity;
            ballScript.thrownByPlayer1 = false;
            ballCopy.transform.position = gameObject.transform.position;
        }
        if (Time.time > timeSinceLastShot && player2.position.y > transform.position.y - (playerBounds.size.y) && player2.position.y < transform.position.y + (playerBounds.size.y)) // A short cooldown when player throws a ball
        {
            timeSinceLastShot = Time.time + rateOfFire;

            GameObject ballCopy = Instantiate(ball); // Instantiate a ball object and "throw" it
            Ball ballScript = ballCopy.GetComponent<Ball>();
            ballScript.velocity = -ballScript.velocity;
            ballScript.thrownByPlayer1 = false;
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
            SceneManager.LoadScene("Player1Win");
        }
    }

    IEnumerator Flash()
    {
        sprite.color = Color.red;
        yield return new WaitForSeconds(.1f);
        sprite.color = Color.white;
    }
}