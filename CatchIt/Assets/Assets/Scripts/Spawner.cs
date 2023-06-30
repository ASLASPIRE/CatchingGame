using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class Spawner : MonoBehaviour
{
    [Header("Prefabs")]
    // Word prefab already contains TextMeshPro, or at least it should
    [SerializeField] private GameObject word;
    // Power-up prefabs in array
    [SerializeField] private GameObject[] powerUps;

    [Header("World Bounds and Parameters")]
    // Set bounds for where words spawn
    public float xBoundLeft;
    public float xBoundRight;
    public float yBound;
    public float fallingSpeed = 0.2f;
    public float spawnRate = 1.0f;

    // Videoplayer
    [SerializeField] private VideoPlayer videoPlayer;
    private RawImage rawImage;

    private List<string> currentWordsToSpawn = new List<string>();
    private int currentWordsToSpawnSize = 6;

    // Specific correct word/link chosen at period
    public string CorrectWord = "";
    private bool isSpawnerActive;

    //private List<string> vidVocabList;

    // Start is called before the first frame update
    void Start()
    {
        // Make videoplayer transparent
        rawImage = videoPlayer.gameObject.GetComponent<RawImage>();
        rawImage.color = new Color32(255, 255, 255, 0);

        // Handle difficulty setting
        switch (Globals.difficulty)
		{
			case Globals.Difficulty.Easy:
                fallingSpeed = 0.2f;
                spawnRate = 1.2f;
				break;
			case Globals.Difficulty.Medium:
				fallingSpeed = 0.3f;
                spawnRate = 1.0f;
				break;
			case Globals.Difficulty.Hard:
				fallingSpeed = 0.4f;
                spawnRate = 0.8f;
				break;
		}
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartSpawningWords()
    {
        isSpawnerActive = true;
        ChangeCorrectWord();
        rawImage.color = new Color32(255, 255, 255, 255);
        StartCoroutine(SpawnRandomGameObject());
    }

    public void StopSpawningWords()
    {
        isSpawnerActive = false;
        StopCoroutine(SpawnRandomGameObject());
    }

    public IEnumerator SpawnRandomGameObject()
    {
        if (!isSpawnerActive) yield break;

        yield return new WaitForSeconds(spawnRate);
        int randomVocabWordIndex = Random.Range(0, currentWordsToSpawn.Count);
        if (word.GetComponent<TextMeshPro>() != null)
        {
            word.GetComponent<TextMeshPro>().text = currentWordsToSpawn[randomVocabWordIndex];
            Rigidbody2D wordRigidBody = word.GetComponent<Rigidbody2D>();
            wordRigidBody.gravityScale = fallingSpeed;
            GameObject tmp = Instantiate(word, new Vector2(Random.Range(xBoundLeft, xBoundRight), yBound), Quaternion.identity);
            tmp.transform.SetParent(transform, false);
        }
        else
        {
            Debug.Log("Problem with word! TextMeshPro doesn't exist!");
        }

        if (Random.Range(0, 10) == 0)
        {
            StartCoroutine(SpawnRandomPowerUp());
        }
        StartCoroutine(SpawnRandomGameObject());

        yield return new WaitForSeconds(Globals.spawnRate);
    }

    IEnumerator SpawnRandomPowerUp()
    {
        yield return new WaitForSeconds(Random.Range(1, 2));

        int randomPowerUp = Random.Range(0, powerUps.Length);
        GameObject tmp = Instantiate(powerUps[randomPowerUp], new Vector2(Random.Range(xBoundLeft, xBoundRight), yBound), Quaternion.identity);
        tmp.transform.SetParent(transform, false);

    }

    // public void ReadFromFileJSON()
    // {
    //     //Debug.Log("about to read file");
    //     // feed in textasset.text, add json file as text asset to a game object (forces load)
    //     Questions questionsjson = JsonUtility.FromJson<Questions>(jsonFile.text);
    //     //Debug.Log("file read");
    //     foreach (Question q in questionsjson.questions)
    //     {
    //         links.Add(q.Link);
    //         words.Add(q.Word);
    //     }
    // }

    public void ChangeCorrectWord()
    {
        List<string> levelVocabList = LevelOperator.CurrentLevelVocabList;
        
        int randomWordIndex = Random.Range(0, levelVocabList.Count);
        string randomWord = levelVocabList[randomWordIndex];
        CorrectWord = randomWord;
        currentWordsToSpawn.Clear();
    
        if (levelVocabList.Count <= currentWordsToSpawnSize)
        {
            currentWordsToSpawn.AddRange(levelVocabList);
        }
        else
        {
            currentWordsToSpawn.Add(CorrectWord);
            
            for (int i = 0; i < currentWordsToSpawnSize; i++)
            {
                int randomVocabWordIndex = Random.Range(0, levelVocabList.Count);
                currentWordsToSpawn.Add(levelVocabList[randomVocabWordIndex]);
                
            }
            
        }
        
        Debug.Log("about to play video");
        //videoPlayerController.PlayVideo(correctWord);
        videoPlayer.url = VideoManager.VocabWordToPathDict[CorrectWord];
        videoPlayer.Play();
    }
}

// ----------------------------------------------- JSON READING CLASSES ---------------------------------------------

[System.Serializable]
public class Question
{
    //these variables are case sensitive and must match the strings "Word" and "Link" in the JSON.
    public string Word;
    public string Link;
}

[System.Serializable]
public class Questions
{
    //Questions is case sensitive and must match the string "questions" in the JSON.
    public Question[] questions;
}