using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;

public class UIManager : MonoBehaviour
{
    [Header("UI Panel Elements")]
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private TextMeshProUGUI gameOverText;
    [SerializeField] private GameObject winPanel;
    [SerializeField] private GameObject nextLevelPanel;

    [Header("Gameplay UI Elements")]
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI livesText;

    [Header("Animations")]
    public Animator loseLifeAnimation; // -1 animation
    public Animator increaseScoreAnimation; // +100 animation

    [Header("Managers")]
    [SerializeField] private GameObject levelManager;
    [SerializeField] private Spawner spawner;
    [SerializeField] private GameMechanics gameMechanics;
    //private bool isNextLevelPanelCalled; 

    public bool isGameOver = false;

    public BasketController basket;
    public PlayfabManager playfabManager;
    //public GameObject makeWebcamWork;

    // Start is called before the first frame update
    private void Start()
    {   
        gameOverPanel.SetActive(false);
        nextLevelPanel.SetActive(false);
        winPanel.SetActive(false);

        Time.timeScale = 1.0f;

        // Correct text of "increasing score" animation
        TextMeshProUGUI textMesh = increaseScoreAnimation.GetComponent<TextMeshProUGUI>();
        textMesh.text = "+" + GameMechanics.ScoreIncrementValue.ToString();
    }

    // Update is called once per frame
    void Update()
    {       
        
    }

    public void UpdateScoreUIText(int newScore, int scoreNeeded)
    {
        scoreText.text = $"Score: {newScore} / {scoreNeeded}";
    }

    public void UpdateLivesUIText(int newLives)
    {
        livesText.text = $"Lives: {newLives}";
    }

    // Controls game over panel
    public void StartGameOverSequence()
    {
        StartCoroutine(GameOverSequence());
    }

    private IEnumerator GameOverSequence()
    {
        playfabManager.SendLeaderboard(gameMechanics.Score);
        playfabManager.SaveScore(gameMechanics.Score);

        gameOverPanel.SetActive(true);

        yield return new WaitForSecondsRealtime(0.05f); // you can get rid of this if u so desire
    }

    public void StartEndOfLevelSequence()
    {   
        //isNextLevelPanelCalled = true;

        // Destroy current words
        GameObject[] currentWords = GameObject.FindGameObjectsWithTag("word");
        spawner.StopSpawningWords();
        foreach (GameObject word in currentWords)
        {
            Destroy(word);
        }
        spawner.gameObject.SetActive(false);

        if (LevelOperator.CurrentLevel < LevelOperator.TotalLevels)
        {
            LevelOperator.CurrentLevel++;
            nextLevelPanel.SetActive(true);
        }
        else
        {
            winPanel.SetActive(true);
        }
    }

    public void OnMainMenuClick()
    {
        SceneManager.LoadScene(0);
    }

    public void OnNextLevelClick()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void OnRestartGameClick()
    {
        LevelOperator.CurrentLevel = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void OnGetLeaderboardClick()
    {
        playfabManager.GetLeaderboard();
        gameOverText.gameObject.SetActive(false);
        //leaderboardPanel.SetActive(true);
        //Debug.Log("made it here");

        //for (int i = 0; i < Globals.leaderboardEntries.Count; i++)
        //{
        //    Debug.Log($"Value: ${Globals.leaderboardEntries[i].StatValue}");
        //    leaderboardEntries[i].text = Globals.leaderboardEntries[i].StatValue.ToString();
        //}
        //if (Globals.leaderboardEntries.Count < leaderboardEntries.Length)
        //{
        //    for (int i = Globals.leaderboardEntries.Count; i < leaderboardEntries.Length; i++)
        //    {
        //        leaderboardEntries[i].text = "0";
        //    }
        //}
    }
}