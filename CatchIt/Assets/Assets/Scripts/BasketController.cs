using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class BasketController : MonoBehaviour
{
    // Managers
    [SerializeField] private PlayfabManager playfabManager;
    [SerializeField] private UIManager uiManager;
    [SerializeField] private Spawner spawner;

    private Rigidbody2D myBody;
    private BoxCollider2D topCollider;

    // Parameters
    public float xBoundLeft, xBoundRight;
    public float maxSpeed = 5;
    public float acceleration = 10;
    public float deceleration = -5;

    // UI - Must externally set text
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI livesText;
    public Joystick joystick;
    public int score = 0;
    public int lives = 5;
    public int score_increment_value = 100;

    // Animations
    public GameObject loseLife; // -1 animation
    public GameObject increaseScore; // +100 animation

    // Trigger enter stuff
    private bool enter = false;

    private bool isLightning = false;
    private bool isStopwatch = false;
    private bool isBurger = false;
    private bool isMultiplier = false;

    // Start is called before the first frame update
    void Start()
    {
        myBody = GetComponent<Rigidbody2D>();
        topCollider = GetComponentInChildren<BoxCollider2D>();
        scoreText.text = $"Score: {score}";
        livesText.text = $"Lives: {lives}";

        score_increment_value = Globals.incrementValue;
        TextMeshProUGUI textMesh = increaseScore.GetComponent<TextMeshProUGUI>();
        textMesh.text = "+" + score_increment_value.ToString();

        loseLife.SetActive(false);
        increaseScore.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {

    }

    // FixedUpdate is called 32 times every second (or whatever predetermined framerate)
   void FixedUpdate()
   {
       float h = 0f;

       // Handle keyboard input
       if (Input.GetKey(KeyCode.LeftArrow))
       {
           h = -1f;
       }
       else if (Input.GetKey(KeyCode.RightArrow))
       {
           h = 1f;
       }

       // Handle touch input
       if (Input.touchCount > 0)
       {
           //Touch touch = Input.GetTouch(0);
           //float touchX = touch.position.x;
           //float screenMiddleX = Screen.width / 2f;

           //if (touchX < screenMiddleX)
           //{
           //    h = -1f;
           //}
           //else if (touchX > screenMiddleX)
           //{
           //    h = 1f;
           //}

           h = joystick.Horizontal;
       }

       if (h > 0)
       {
           if (transform.position.x < xBoundRight)
           {
               myBody.AddForce(Vector2.right * acceleration);
           }
           else
           {
               myBody.AddForce(Vector2.zero);
           }
       }
       else if (h < 0)
       {
           if (transform.position.x > xBoundLeft)
           {
               myBody.AddForce(Vector2.left * acceleration);
           }
           else
           {
               myBody.AddForce(Vector2.zero);
           }
       }
       else
       {
           myBody.AddForce(myBody.velocity * deceleration);
       }

       transform.position = new Vector2(Mathf.Clamp(transform.position.x, xBoundLeft, xBoundRight), transform.position.y);
       myBody.velocity = new Vector2(Mathf.Clamp(myBody.velocity.x, -maxSpeed, maxSpeed), myBody.velocity.y);
   }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!enter)
        {
            enter = true;
            Debug.Log($"collision tag: {collision.tag}");
            GameObject gameObject = collision.gameObject;
            string tag = collision.tag;
            Destroy(collision.gameObject);
            if (tag == "lightning_bolt")
            {
                StartCoroutine(LightningBoltPowerup(15, 100, 10));
                //Destroy(collision.gameObject);
            }
            else if (tag == "stopwatch")
            {
                StartCoroutine(StopwatchPowerup(0.05f, 10));
                //Destroy(collision.gameObject);
            }
            else if (tag == "burger")
            {
                StartCoroutine(BurgerPowerup(1.5f, 10));
                //Destroy(collision.gameObject);
            }
            else if (tag == "multiplication")
            {
                StartCoroutine(MultiplicationPowerup(2, 10));
                //Destroy(collision.gameObject);
            }

            if (tag == "word")
            {
                TextMeshPro textMeshPro = gameObject.GetComponent<TextMeshPro>();
                Debug.Log($"collision word: {textMeshPro.text}");
                if (textMeshPro != null)
                {
                    string textWord = textMeshPro.text;
                    if (spawner != null)
                    {
                        if (string.Equals(textWord, spawner.correctWord))
                        {
                            score = score + score_increment_value;
                            scoreText.text = $"Score: {score} / 1000";
                            increaseScore.SetActive(true);
                            increaseScore.GetComponent<Animator>().Play("increase_score_text", 0, 0.0f);
                            spawner.ChangeCorrectWord();
                            GameObject[] currentWords = GameObject.FindGameObjectsWithTag("word");
                            foreach (GameObject word in currentWords)
                            {
                                TextMeshPro wordTextMeshPro = word.GetComponent<TextMeshPro>();
                                wordTextMeshPro.color = Color.gray;
                                CircleCollider2D circleCollider = word.GetComponent<CircleCollider2D>();
                                circleCollider.enabled = false;
                            }
                            //Debug.Log($"Score: {score}");
                        }
                        else
                        {
                            lives--;
                            livesText.text = $"Lives: {lives}";
                            loseLife.SetActive(true);
                            loseLife.GetComponent<Animator>().Play("lose_life_text", 0, 0.0f);
                            //loseLife.SetActive(false);
                            //Debug.Log($"Lives: {lives}");

                            if (lives == 0)
                            {
                                uiManager.isGameOver = true;
                                StartCoroutine(uiManager.GameOverSequence());
                            }
                        }
                    }
                }

                Destroy(collision.gameObject);
                //Debug.Log("object collected and destroyed");
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        enter = false;
    }

    IEnumerator LightningBoltPowerup(float newMaxSpeed, float newAcceleration, float duration)
    {
        if (isLightning)
        {
            yield break;
        }
        isLightning = true;

        float oldMaxSpeed = maxSpeed;
        float oldAcceleration = acceleration;
        maxSpeed = newMaxSpeed;
        acceleration = newAcceleration;

        yield return new WaitForSeconds(duration);

        maxSpeed = oldMaxSpeed;
        acceleration = oldAcceleration;

        isLightning = false;
    }

    IEnumerator StopwatchPowerup(float newFallingSpeed, float duration)
    {
        if (isStopwatch)
        {
            yield break;
        }
        isStopwatch = true;

        float oldFallingSpeed = spawner.fallingSpeed;
        spawner.fallingSpeed = newFallingSpeed;
        GameObject[] currentWords = GameObject.FindGameObjectsWithTag("word");
        foreach (GameObject word in currentWords)
        {
            Rigidbody2D rigidBody = word.GetComponent<Rigidbody2D>();
            rigidBody.gravityScale = newFallingSpeed;
        }

        yield return new WaitForSeconds(duration);

        spawner.fallingSpeed = oldFallingSpeed;
        currentWords = GameObject.FindGameObjectsWithTag("word");
        foreach (GameObject word in currentWords)
        {
            Rigidbody2D rigidBody = word.GetComponent<Rigidbody2D>();
            rigidBody.gravityScale = oldFallingSpeed;
        }

        isStopwatch = false;
    }

    IEnumerator BurgerPowerup(float scaleFactor, float duration)
    {
        if (isBurger)
        {
            yield break;
        }
        isBurger = true;

        Vector3 oldTransform = transform.localScale;
        transform.localScale = transform.localScale * scaleFactor;

        yield return new WaitForSeconds(duration);

        transform.localScale = oldTransform;

        isBurger = false;
    }

    IEnumerator MultiplicationPowerup(int multiplier, float duration)
    {
        if (isMultiplier)
        {
            yield break;
        }
        isMultiplier = true;

        int oldValue = score_increment_value;
        TextMeshProUGUI textMesh = increaseScore.GetComponent<TextMeshProUGUI>();
        string oldText = textMesh.text;
        score_increment_value = score_increment_value * multiplier;
        textMesh.text = "+" + score_increment_value.ToString();

        yield return new WaitForSeconds(duration);

        score_increment_value = oldValue;
        textMesh.text = oldText;

        isMultiplier = false;
    }
}
