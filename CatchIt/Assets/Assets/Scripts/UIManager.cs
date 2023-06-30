using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject winPanel;
    [SerializeField] private TextMeshProUGUI gameOverText;
    [SerializeField] private TextMeshProUGUI restartText;
    [SerializeField] private GameObject leaderboardPanel;
    [SerializeField] private TextMeshProUGUI[] leaderboardEntries;
    [SerializeField] private GameObject levelManager;

    public GameObject nextLevelPanel;
    public Spawner spawner;
    public bool isNextLevelPanelCalled; 

    public bool isGameOver = false;

    public BasketController basket;
    public PlayfabManager playfabManager;
    public GameObject makeWebcamWork;

    // Start is called before the first frame update
    private void Start()
    {   //Time.timeScale = 0;
        //Disables panels if active
        isNextLevelPanelCalled = false;
        gameOverPanel.SetActive(false);
        leaderboardPanel.SetActive(false);
        nextLevelPanel.SetActive(false);
        Time.timeScale = 1.0f;
        //levelManager.SetActive(false);
        winPanel.SetActive(false);
    }


    public void startUp(){
        levelManager.SetActive(true);
        makeWebcamWork.SetActive(false);
    }
    

    // Update is called once per frame
    void Update()
    {
        //Trigger game over manually and check with bool so it isn't called multiple times
        if (Input.GetKeyDown(KeyCode.G) && !isGameOver)
        {
            isGameOver = true;

            StartCoroutine(StartGameOverSequence());
        }

        //change to basket.score
        if (basket.score >= basket.scoreNeeded && !isNextLevelPanelCalled){
            
            if (Globals.CurrentLevel == Globals.TotalLevels){
                StartWinSequence();
            }
            StartNextLevelSequence();
        }


       
        //If game is over
        if (isGameOver)
        {
            //If R is hit, restart the current scene
            if (Input.GetKeyDown(KeyCode.R))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }

            //If Q is hit, quit the game
            if (Input.GetKeyDown(KeyCode.Q))
            {
                print("Application Quit");
                Application.Quit();
            }
        }


    }

    // Controls game over canvas and there's a brief delay between main Game Over text and option to restart/quit text
    public IEnumerator StartGameOverSequence()
    {
        Time.timeScale = 0;
        playfabManager.SendLeaderboard(basket.score);
        playfabManager.SaveScore(basket.score);

        gameOverPanel.SetActive(true);

        yield return new WaitForSecondsRealtime(4.0f);

        restartText.gameObject.SetActive(true);
    }

    public void StartNextLevelSequence()
    {   
        //Debug.Log("level is");
        //Debug.Log(Globals.currentLevel);

        isNextLevelPanelCalled = true;
        GameObject[] currentWords = GameObject.FindGameObjectsWithTag("word");
        Debug.Log($"number of spawned words to destroy = {currentWords.Length}");
        spawner.StopSpawningWords();
        foreach (GameObject word in currentWords)
        {
            Destroy(word);
        }

        spawner.gameObject.SetActive(false);
        //Time.timeScale = 0;
        //playfabManager.SendLeaderboard(basket.score);
        //playfabManager.SaveScore(basket.score);
        if (Globals.CurrentLevel < Globals.TotalLevels)
        {
            Globals.CurrentLevel++;
            nextLevelPanel.SetActive(true);
        }
        else
        {
            Debug.Log("Congrats u won :o");
            Debug.Log($"CurrentLevel = {Globals.CurrentLevel}");
            Debug.Log($"TotalLevels = {Globals.TotalLevels}");
            nextLevelPanel.SetActive(false);
            winPanel.SetActive(true);
        }
        
        // if (Globals.currentLevel < 3){
        //     Globals.currentLevel++;
        //     nextLevelPanel.SetActive(true);
        // }

        // if (Globals.currentLevel > 3){
        //     //Debug.Log("Level issue?");
        //     //Debug.Log("Level is three!");
        //     nextLevelPanel.SetActive(false);
        //     winPanel.SetActive(true);
        // }


        


        //restartText.gameObject.SetActive(true);
    }

    public void StartWinSequence()
    {
        nextLevelPanel.SetActive(false);
        winPanel.SetActive(true);
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
        Globals.CurrentLevel = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void OnGetLeaderboardClick()
    {
        playfabManager.GetLeaderboard();
        gameOverText.gameObject.SetActive(false);
        leaderboardPanel.SetActive(true);
        Debug.Log("made it here");

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