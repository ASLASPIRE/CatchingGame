using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UI; 

public class LevelOperator : MonoBehaviour
{
    private Animator anim;
    //private string currentState;
    private RuntimeAnimatorController currentState;
    public GameObject levelOperator; 

    // Testing List
    
    public Text buttonText; 
    private int index; 
    private bool ready = false; 
    public GameObject spawner; 
    public Spawner spawnSpeed; 
    public Text vocabDisplay; 
    public GameObject gif; 
    //public int level = 1; 
    private List<RuntimeAnimatorController> fullList; 
    private List <RuntimeAnimatorController> levelList;
    public GIFController gifController;
    public WebcamDisplay webcam; 


    // Awake is called before the first frame update and before Start()
    void Awake()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        anim.speed = 1.0f;
        index = 0;
        //ADD level number 1 in here 
        levelList = levelListCreator(gifController.currentVocabList);
        spawnSpeed = spawner.GetComponent<Spawner>();
        //spawnSpeed.SetList();

        gif.SetActive(false);
        nextButton();
        
        
    }

    // Update is called once per frame
    void Update()
    {

    }


    public void UpdateLevelSet(string newLevelSet)
    {
        // switch (newLevelSet)
        // {
        //     case "Level One":
        //         levelList = levelOneVocab;
        //         break;

        //     case "Level Two":
        //         levelList = levelOneVocab;
        //         break;
            
        //     case "Level Three":
        //         levelList = levelOneVocab;
        //         break;
            
        // }
    }

    //I want a function that takes the full vocab list and manages the contents based on what level you are on
    //Level operator takes the full list and grabs the first three words as the current level list
    //Then it passes it to Spawner to be used 

    public  List<RuntimeAnimatorController> levelListCreator(List<RuntimeAnimatorController> gifControllerList){
       
        fullList = gifControllerList;
        //Add three more words each level
        int level = Globals.level;
        if (level == 1){
            levelList = fullList.GetRange(0, 3);

        }

        if (level ==2){
            levelList = fullList.GetRange(0, 6);
        }

        if (level==3){
            levelList = fullList.GetRange(0, 9);
        }

        if (level ==4){
            Debug.Log("Invalid level");
            return levelList; 
        }
        
        
      
        return levelList; 




    }


    public void nextButton(){

        if (index ==0 ){
            webcam.turnOnWebcam();
        }
        if (index< levelList.Count){
        ChangeAnimationState(levelList[index]);
        
        vocabDisplay.text = levelList[index].name; 
        index ++;
        
        }

        if (ready){
            Time.timeScale = 1.0f; 
            spawnSpeed.fallingSpeed = Globals.fallingSpeed;
            gif.SetActive(true);
            webcam.turnOffWebcam();
            spawnSpeed.StartSpawningWords();
            levelOperator.SetActive(false);
        }
        

        if (index == levelList.Count){
            buttonText.text = "Let's play!";
            ready = true;
                    
            

        }

        
        
    }




    public void ChangeAnimationState(RuntimeAnimatorController newState)
    {
        Debug.Log("made it here 1");
        Debug.Log($"newState = {newState.name}");
        if (currentState == newState) return;
        //string toPlay = vocabSetName + "." + newState;
        //Debug.Log($"newStateName = {toPlay}");
        //anim.Play(toPlay);
        //Debug.Log($"name of controller = {levelList[newState].name}");
        //anim.runtimeAnimatorController = levelList[newState] as RuntimeAnimatorController;
        anim.runtimeAnimatorController = newState as RuntimeAnimatorController;
        currentState = newState;
    }
}
