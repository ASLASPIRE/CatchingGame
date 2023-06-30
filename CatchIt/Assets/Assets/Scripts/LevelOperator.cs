using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class LevelOperator : MonoBehaviour
{
    [Header("Managers")]
    [SerializeField] private Spawner spawner;

    [Header("External Components")]
    [SerializeField] private GameObject learnNewVocabPanel;
    [SerializeField] private TextMeshProUGUI vocabTextDisplay;
    [SerializeField] private TextMeshProUGUI levelTextDisplay;
    private VideoPlayer learningVideoPlayer;
    private Button button;

    [Header("Parameters")]
    public static List<string> CurrentLevelVocabList;
    private int vocabListIndex;
    public static int CurrentLevel = 1;
    public static int TotalLevels = 4;

    // Start is called before the first frame update
    void Start()
    {
        // Get Components
        learningVideoPlayer = learnNewVocabPanel.GetComponentInChildren<VideoPlayer>();
        button = learnNewVocabPanel.GetComponentInChildren<Button>();

        // Process difficulty
        switch (Globals.difficulty)
		{
			case Globals.Difficulty.Easy:
                Globals.fallingSpeed = 0.2f;
                Globals.spawnRate = 1.2f;
                Globals.TotalLevels = 3;
				break;
			case Globals.Difficulty.Medium:
				Globals.fallingSpeed = 0.3f;
                Globals.spawnRate = 1.0f;
                Globals.TotalLevels = 4;
				break;
			case Globals.Difficulty.Hard:
				Globals.fallingSpeed = 0.4f;
                Globals.spawnRate = 0.8f;
                Globals.TotalLevels = 5;
				break;
		}

        // Generate list of vocab words
        VideoManager.GenerateVocabListFromSelectedVocabSet();

        // Display level
        levelTextDisplay.text = "Level: " + Globals.CurrentLevel.ToString() + "/" + Globals.TotalLevels.ToString();

        // Pause game and prepare learning screen
        Time.timeScale = 0.0f;
        CurrentLevelVocabList = new List<string>();
        CurrentLevelVocabList = CreateLevelVocabList(VideoManager.VocabWordToPathDict.Keys.ToList(), Globals.CurrentLevel, Globals.TotalLevels);
        vocabListIndex = 0;
        Debug.Log($"Size of levelvocablist = {CurrentLevelVocabList.Count}");
        Debug.Log($"Displaying {CurrentLevelVocabList[vocabListIndex]}");
        vocabTextDisplay.text = CurrentLevelVocabList[vocabListIndex];
        learningVideoPlayer.url = VideoManager.VocabWordToPathDict[CurrentLevelVocabList[vocabListIndex]];
        Debug.Log($"Size of levelvocablist (again) = {CurrentLevelVocabList.Count}");
    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// Creates a miniature list of vocab words compared to the full vocab list specific to each level, where the number in each list increases per level
    /// </summary>
    /// <param name="fullVocabList">The list containing all the vocab words</param>
    /// <param name="levelNumber">The current level number</param>
    /// <param name="totalNumLevels">The total number of levels implemented</param>
    /// <returns>Returns a new list with the shortened list of vocab words</returns>
    public List<string> CreateLevelVocabList(List<string> fullVocabList, int levelNumber, int totalNumLevels)
    {
        Debug.Log($"currentLevelNumber = {levelNumber}");
        Debug.Log($"totalNumLevels = {totalNumLevels}");

        return fullVocabList.GetRange(0, Mathf.FloorToInt(((float) levelNumber / (float) totalNumLevels) * fullVocabList.Count));
    }

    public void OnNextButtonClick()
    {
        Debug.Log($"current index upon button click = {vocabListIndex}");
        Debug.Log($"currentLevelVocabList Count = {CurrentLevelVocabList.Count}");
        if (vocabListIndex < CurrentLevelVocabList.Count - 1)
        {
            vocabListIndex++;
            vocabTextDisplay.text = CurrentLevelVocabList[vocabListIndex];
            learningVideoPlayer.url = VideoManager.VocabWordToPathDict[CurrentLevelVocabList[vocabListIndex]];
            
            if (vocabListIndex == CurrentLevelVocabList.Count - 1)
            {
                TextMeshProUGUI buttonText = button.gameObject.GetComponentInChildren<TextMeshProUGUI>();
                buttonText.text = "Play";
            }
        }
        else
        {
            learnNewVocabPanel.SetActive(false);
            Time.timeScale = 1.0f;
            spawner.StartSpawningWords();
        }
    }
}
