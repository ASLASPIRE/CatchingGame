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
    [SerializeField] private GameMechanics gameMechanics;
    [SerializeField] private PowerupTimer powerupTimer;

    private Rigidbody2D myBody;

    // Parameters
    [Header("Player Parameters")]
    public float MaxSpeed = 5;
    public float Acceleration = 10;
    public float Deceleration = -5;

    // UI - Must externally set text
    [Header("UI Elements")]
    [SerializeField] private Joystick joystick;

    // Booleans for powerups
    private bool isLightning = false;
    private bool isStopwatch = false;
    private bool isBurger = false;
    private bool isMultiplier = false;

    // Start is called before the first frame update
    private void Start()
    {
        myBody = GetComponent<Rigidbody2D>();
    }

    public int AddScore()
    {
        int newScore = gameMechanics.AddScore();
        uiManager.UpdateScoreUIText(newScore, GameMechanics.LevelScoreNeeded);
        uiManager.increaseScoreAnimation.SetTrigger("playAnimation");
        return newScore;
    }

    public int LoseLife()
    {
        int newLives = gameMechanics.LoseLife();
        uiManager.UpdateLivesUIText(newLives);
        uiManager.loseLifeAnimation.SetTrigger("playAnimation");
        return newLives;
    }

    // FixedUpdate is called 50 times every second and is useful for physics stuff
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
        GameObject colliderGameObject = collision.gameObject;
        string tag = collision.tag;
        if (tag.Equals("word_greyed"))
        {
            return;
        }

        Destroy(collision.gameObject);

        if (tag.Equals("lightning_bolt"))
        {
            StartCoroutine(LightningBoltPowerup(15, 100, 10));
        }
        else if (tag.Equals("stopwatch"))
        {
            StartCoroutine(StopwatchPowerup(0.05f, 10));
        }
        else if (tag.Equals("burger"))
        {
            StartCoroutine(BurgerPowerup(1.5f, 10));
        }
        else if (tag.Equals("multiplication"))
        {
            StartCoroutine(MultiplicationPowerup(2, 10));
        }

        if (tag.Equals("word"))
        {
            TextMeshPro textMeshPro = colliderGameObject.GetComponent<TextMeshPro>();
            if (textMeshPro == null)
            {
                throw new System.Exception("Falling word does not contain a TextMeshPro element.");
            }

            if (spawner == null)
            {
                throw new System.Exception("Ensure a spawner GameObject exists and is linked to this script.");
            }

            string textWord = textMeshPro.text;

            if (string.Equals(textWord, spawner.CorrectWord))
            {
                // Player caught the correct word, increase score
                AddScore();

                // Choose the next word to catch
                spawner.ChangeCorrectWord();

                // Gray out the current words on the screen so players don't accidentally catch them
                GameObject[] currentWords = GameObject.FindGameObjectsWithTag("word");
                foreach (GameObject word in currentWords)
                {
                    TextMeshPro wordTextMeshPro = word.GetComponent<TextMeshPro>();
                    wordTextMeshPro.color = Color.gray;
                    word.tag = "word_greyed";
                    SpriteRenderer sprite = word.GetComponentInChildren<SpriteRenderer>();
                    sprite.color = new Color32(255, 255, 255, 36);
                    //CircleCollider2D circleCollider = word.GetComponent<CircleCollider2D>();
                    //circleCollider.enabled = false;
                }
            }
            else
            {
                // Player caught the wrong word, decrease lives
                LoseLife();
            }
        }
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

        powerupTimer.RestartTimer(duration);
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

        powerupTimer.RestartTimer(duration);
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

        powerupTimer.RestartTimer(duration);
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

        int oldValue = GameMechanics.ScoreIncrementValue;
        TextMeshProUGUI textMesh = uiManager.increaseScoreAnimation.GetComponent<TextMeshProUGUI>();
        string oldText = textMesh.text;
        GameMechanics.ScoreIncrementValue = GameMechanics.ScoreIncrementValue * multiplier;
        textMesh.text = "+" + GameMechanics.ScoreIncrementValue.ToString();

        powerupTimer.RestartTimer(duration);
        yield return new WaitForSeconds(duration);

        GameMechanics.ScoreIncrementValue = oldValue;
        textMesh.text = oldText;

        isMultiplier = false;
    }
}
