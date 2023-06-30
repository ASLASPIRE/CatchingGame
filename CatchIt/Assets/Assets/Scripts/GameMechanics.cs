using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMechanics : MonoBehaviour
{
    public int Score;
    public static int ScoreIncrementValue = 100;
    public static int LevelScoreNeeded;
    public static int Lives = 5;
    public bool IsGameOver;
    public bool isLevelScoreReached;

    private bool isNextLevelPanelCalled;

    [Header("Managers")]
    [SerializeField] private UIManager uiManager;

    // Start is called before the first frame update
    void Start()
    {
        // Process difficulty
        switch (Globals.difficulty)
		{
			case Globals.Difficulty.Easy:
                LevelScoreNeeded = 800;
				break;
			case Globals.Difficulty.Medium:
                LevelScoreNeeded = 1000;
				break;
			case Globals.Difficulty.Hard:
                LevelScoreNeeded = 1500;
				break;
		}

        isNextLevelPanelCalled = false;

        Score = 0;

        uiManager.UpdateScoreUIText(Score, LevelScoreNeeded);
        uiManager.UpdateLivesUIText(Lives);
    }

    // Update is called once per frame
    void Update()
    {
        if (Score >= LevelScoreNeeded)
        {
            Debug.Log("starting up next level panel");
            isLevelScoreReached = true;
        }
        // If lives are at 0, game over
        if (GameMechanics.Lives == 0)
        {
            Debug.Log("setting game over boolean to true");
            IsGameOver = true;
        }

        if (IsGameOver)
        {
            Time.timeScale = 0.0f;
            Debug.Log("Starting game over sequence");
            StartGameOverSequence();
        }
        if (isLevelScoreReached && !isNextLevelPanelCalled)
        {
            Time.timeScale = 0.0f;

            uiManager.StartEndOfLevelSequence();
            isNextLevelPanelCalled = true;
        }
    }

    public int AddScore()
    {
        Score += ScoreIncrementValue;
        return Score;
    }

    public int LoseLife()
    {
        Lives -= 1;
        return Lives;
    }

    public void StartGameOverSequence()
    {
        uiManager.StartGameOverSequence();
    }
}
