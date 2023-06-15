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
    [Header("Managers")]
    [SerializeField] private PlayfabManager playfabManager;
    [SerializeField] private UIManager uiManager;
    [SerializeField] private Spawner spawner;

    private Rigidbody2D myBody;

    // Parameters
    [Header("Player Parameters")]
    public float MaxSpeed = 5;
    public float Acceleration = 10;
    public float Deceleration = -5;

    // UI - Must externally set text
    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI livesText;
    [SerializeField] private Joystick joystick;
    public int score { get; set; } = 0;
    public int scoreNeeded { get; set; } = 1000;
    public int lives { get; set; } = 5;
    private int score_increment_value = 100;

    // Animations
    [Header("Animations")]
    public Animator loseLifeAnimation; // -1 animation
    public Animator increaseScoreAnimation; // +100 animation

    // Check if word entered trigger
    private bool enter = false;

    // Booleans for powerups
    private bool isLightning = false;
    private bool isStopwatch = false;
    private bool isBurger = false;
    private bool isMultiplier = false;

    // Start is called before the first frame update
    private void Start()
    {
        myBody = GetComponent<Rigidbody2D>();

        // Set UI text
        UpdateScoreUIText(score);
        UpdateLivesUIText(lives);

        score_increment_value = Globals.incrementValue;

        // Correct text of "increasing score" animation
        TextMeshProUGUI textMesh = increaseScoreAnimation.GetComponent<TextMeshProUGUI>();
        textMesh.text = "+" + score_increment_value.ToString();
    }

    private void UpdateScoreUIText(int newScore)
    {
        scoreText.text = $"Score: {newScore} / {scoreNeeded}";
    }

    private void UpdateLivesUIText(int newLives)
    {
        livesText.text = $"Lives: {newLives}";
    }

    // FixedUpdate is called 32 times every second (or whatever predetermined framerate) and is useful for physics stuff
    private void FixedUpdate()
    {
        // Handle player movement left and right
        float horizontalInput = Input.GetAxis("Horizontal");

        if (Input.touchCount > 0)
        {
            horizontalInput = joystick.Horizontal;
        }

        myBody.velocity = new Vector2(horizontalInput * MaxSpeed, myBody.velocity.y);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (enter)
        {
            return;
        }
        enter = true;

        GameObject colliderGameObject = collision.gameObject;
        string tag = collision.tag;
        Destroy(collision.gameObject);

        Debug.Log($"collision tag: {tag}");

        if (tag == "lightning_bolt")
        {
            StartCoroutine(LightningBoltPowerup(15, 100, 10));
        }
        else if (tag == "stopwatch")
        {
            StartCoroutine(StopwatchPowerup(0.05f, 10));
        }
        else if (tag == "burger")
        {
            StartCoroutine(BurgerPowerup(1.5f, 10));
        }
        else if (tag == "multiplication")
        {
            StartCoroutine(MultiplicationPowerup(2, 10));
        }

        if (tag == "word")
        {
            TextMeshPro textMeshPro = colliderGameObject.GetComponent<TextMeshPro>();
            Debug.Log($"collision word: {textMeshPro.text}");
            if (textMeshPro == null)
            {
                throw new System.Exception("Falling word does not contain a TextMeshPro element.");
            }

            if (spawner == null)
            {
                throw new System.Exception("Ensure a spawner GameObject exists and is linked to this script.");
            }

            string textWord = textMeshPro.text;

            if (string.Equals(textWord, spawner.correctWord))
            {
                // Player caught the correct word, increase score
                score += score_increment_value;
                UpdateScoreUIText(score);
                increaseScoreAnimation.SetTrigger("playAnimation");

                // Choose the next word to catch
                spawner.ChangeCorrectWord();

                // Gray out the current words on the screen so players don't accidentally catch them
                GameObject[] currentWords = GameObject.FindGameObjectsWithTag("word");
                foreach (GameObject word in currentWords)
                {
                    TextMeshPro wordTextMeshPro = word.GetComponent<TextMeshPro>();
                    wordTextMeshPro.color = Color.gray;
                    CircleCollider2D circleCollider = word.GetComponent<CircleCollider2D>();
                    circleCollider.enabled = false;
                }
            }
            else
            {
                // Player caught the wrong word, decrease lives
                lives--;
                UpdateLivesUIText(lives);
                loseLifeAnimation.SetTrigger("playAnimation");

                // If lives are at 0, game over
                if (lives == 0)
                {
                    uiManager.isGameOver = true;
                    StartCoroutine(uiManager.StartGameOverSequence());
                }
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

        float oldMaxSpeed = MaxSpeed;
        float oldAcceleration = Acceleration;
        MaxSpeed = newMaxSpeed;
        Acceleration = newAcceleration;

        yield return new WaitForSeconds(duration);

        MaxSpeed = oldMaxSpeed;
        Acceleration = oldAcceleration;

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
        TextMeshProUGUI textMesh = increaseScoreAnimation.GetComponent<TextMeshProUGUI>();
        string oldText = textMesh.text;
        score_increment_value = score_increment_value * multiplier;
        textMesh.text = "+" + score_increment_value.ToString();

        yield return new WaitForSeconds(duration);

        score_increment_value = oldValue;
        textMesh.text = oldText;

        isMultiplier = false;
    }
}
