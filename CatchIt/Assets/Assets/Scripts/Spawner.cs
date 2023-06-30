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
    // Word prefab already contains TextMeshPro, or at least it should
    // Power-up prefabs in array
    [SerializeField] private GameObject word;
    [SerializeField] private GameObject[] powerUps;

    // Set bounds for where words spawn
    public float xBoundLeft, xBoundRight, yBound;
    public float fallingSpeed = 0.2f;

    // JSON file containing words
    //[SerializeField] private TextAsset jsonFile;
    //[SerializeField] private string vocabSet;

    // Videoplayer
    [SerializeField] private VideoPlayer videoPlayer;
    private RawImage rawImage;
    //[SerializeField] private GIFController gifController;

    // Lists containing words and links to the video playing them
    public List<string> links = new List<string>();
    public List<string> words = new List<string>();

    private List<string> currentWordsToSpawn = new List<string>();
    public int currentWordsToSpawnSize = 6;

    // Specific correct word/link chosen at period
    //public string correctLink = "";
    public string correctWord = "";
    //public RuntimeAnimatorController correctController;

    // Time between correct word changes
    public float changeDelay;
    public LevelOperator levelOperator; 
    public List<RuntimeAnimatorController> vocabList;
    public VideoPlayerController videoPlayerController;

    private bool isSpawnerActive;

    //private List<string> vidVocabList;

    // Start is called before the first frame update
    void Start()
    {
        // if (Globals.jsonFile)
        // {
        //     jsonFile = Globals.jsonFile;
        // }
        // if (Globals.vocabSet != null) {
        //     vocabSet = Globals.vocabSet;
        // }

        // if (Globals.tutorial){
        //     vocabSet = "Tutorial";
        //     fallingSpeed = Globals.fallingSpeed;
        // }
        // if(!Globals.tutorial){
        //     fallingSpeed = 0;
        // }

        
        //gifController.UpdateVocabSet(vocabSet);

        //vocabList = gifController.currentVocabList;
        ////ReadFromFileJSON();
        //InvokeRepeating("ChangeCorrectWord", 1f, changeDelay);

        rawImage = videoPlayer.gameObject.GetComponent<RawImage>();
        rawImage.color = new Color32(255, 255, 255, 0);
        
        
        //StartCoroutine(SpawnRandomGameObject());
    }

    // Update is called once per frame
    void Update()
    {
        // Empty
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

        yield return new WaitForSeconds(Globals.spawnRate);
        int randomVocabWordIndex = Random.Range(0, currentWordsToSpawn.Count);
        if (word.GetComponent<TextMeshPro>() != null)
        {
            word.GetComponent<TextMeshPro>().text = currentWordsToSpawn[randomVocabWordIndex];
            Rigidbody2D wordRigidBody = word.GetComponent<Rigidbody2D>();
            wordRigidBody.gravityScale = fallingSpeed;
            Instantiate(word, new Vector2(Random.Range(xBoundLeft, xBoundRight), yBound), Quaternion.identity);
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
        
        //int randomVocabWordIndex = Random.Range(0, smallWords.Count);
        ////Debug.Log($"randomVocabWordIndex: {randomVocabWordIndex}");
        //Debug.Log($"Size/Count of smallWords: {smallWords.Count}");
        ////Debug.Log("\n");
        //if (word.GetComponent<TextMeshPro>() != null )
        //{
        //    word.GetComponent<TextMeshPro>().text = smallWords[randomVocabWordIndex];
        //    Rigidbody2D wordRigidBody = word.GetComponent<Rigidbody2D>();
        //    wordRigidBody.gravityScale = fallingSpeed;
        //    Instantiate(word, new Vector2(Random.Range(xBoundLeft, xBoundRight), yBound), Quaternion.identity);
        //} else
        //{
        //    Debug.Log("Problem with word! TextMeshPro doesn't exist!");
        //}

        ////System.Random random = new System.Random();
        ////if (random.Next(10) == 0)
        //if (Random.Range(0, 10) == 0)
        //{
        //    StartCoroutine(SpawnRandomPowerUp());
        //}
        //StartCoroutine(SpawnRandomGameObject());
    }

    IEnumerator SpawnRandomPowerUp()
    {
        yield return new WaitForSeconds(Random.Range(1, 2));

        int randomPowerUp = Random.Range(0, powerUps.Length);
        Instantiate(powerUps[randomPowerUp], new Vector2(Random.Range(xBoundLeft, xBoundRight), yBound), Quaternion.identity);

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

    // public void SetList(){
    //     vocabList = gifController.currentVocabList;
    //     //Assign by level 
    //     if (!Globals.tutorial){
    //     vocabList = levelOperator.levelListCreator(gifController.currentVocabList);
       
    //     }
    // }

    public void ChangeCorrectWord()
    {
        List<string> levelVocabList = LevelOperator.CurrentLevelVocabList;
        Debug.Log($"Size of levelvocablist (again 2) = {levelVocabList.Count}");
        int randomWordIndex = Random.Range(0, levelVocabList.Count);
        string randomWord = levelVocabList[randomWordIndex];
        correctWord = randomWord;
        currentWordsToSpawn.Clear();
    
        if (levelVocabList.Count >= currentWordsToSpawnSize)
        {
            currentWordsToSpawn.AddRange(levelVocabList);
        }
        else
        {
            currentWordsToSpawn.Add(correctWord);
            Debug.Log($"Size of levelvocablist (again 2.5) = {levelVocabList.Count}");
            for (int i = 0; i < currentWordsToSpawnSize; i++)
            {
                int randomVocabWordIndex = Random.Range(0, levelVocabList.Count);
                currentWordsToSpawn.Add(levelVocabList[randomVocabWordIndex]);
                Debug.Log($"Size of levelvocablist (looping) = {levelVocabList.Count}");
            }
            Debug.Log($"Size of levelvocablist (again 2.75) = {levelVocabList.Count}");
        }
        
        
        Debug.Log("about to play video");
        //videoPlayerController.PlayVideo(correctWord);
        videoPlayer.url = VideoManager.VocabWordToPathDict[correctWord];
        videoPlayer.Play();

        Debug.Log($"Size of levelvocablist (again 3) = {levelVocabList.Count}");


        //// // Choose random correct word
        //vocabList = gifController.currentVocabList;
        ////Assign by level 
        //if (!Globals.tutorial){
        //vocabList = levelOperator.levelListCreator(gifController.currentVocabList);
       
        //}



        //int randomWordIndex = Random.Range(0, vocabList.Count);
        //if (Globals.tutorial){ randomWordIndex = 0; }
        //Debug.Log($"randomWordIndex = {randomWordIndex}");
        //string randomWord = vocabList[randomWordIndex].name;

        //correctController = vocabList[randomWordIndex];
        //correctWord = randomWord;

        //// Make smaller list of words to fall, including correct word
        //smallWords.Clear();
        //smallWords.Add(correctWord);
        //for (int i = 0; i < vocabListSize; i++)
        //{
        //    int randomVocabWordIndex = Random.Range(0, vocabList.Count);
        //    smallWords.Add(vocabList[randomVocabWordIndex].name);
        //    smallWords.Add(correctWord);
        //}

        //// Play video/GIF
        //Debug.Log("made it here 1");
        //gifController.ChangeAnimationState(correctController);

        //int randomIndex = Random.Range(0, words.Count);
        //correctLink = links[randomIndex];
        //correctWord = words[randomIndex];
        //smallWords.Clear();
        //smallWords.Add(correctWord);
        //for (int i = 0; i < vocabListSize; i++)
        //{
        //    int randomVocabWordIndex = Random.Range(0, words.Count);
        //    smallWords.Add(words[randomVocabWordIndex]);
        //}
        //videoPlayer.url = correctLink;
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