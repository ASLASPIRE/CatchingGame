using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class LearningManager : MonoBehaviour
{
    private bool stepOne = false; 
    private bool stepTwo = false;
    private bool stepThree = false;
    private bool replay = false;
    public Text tutorialtext;
    public GameObject swipeIcon;
    public GameObject practiceGIF;  
    public GameObject spawner;
    public BasketController basket;
    public Text buttonText; 
    public GameObject tryAgain;
    public GameObject progressionButton;
    public GameObject playButton;
    public GameObject powerups;
    public GameObject powerupTransition;
    public GameObject tutorialText;
    public GameMechanics gameMechanics;
    
    
    // Start is called before the first frame update
    void Start()
    {
        tutorialtext.text = "Welcome to the Orchard! Move your basket by swiping left or right.";
        practiceGIF.SetActive(false);
        spawner.SetActive(false);
        tryAgain.SetActive(false);
        Globals.tutorial = true; 
        playButton.SetActive(false);
        //lastStep.SetActive(false);
        powerups.SetActive(false);
        powerupTransition.SetActive(false);
        progressionButton.SetActive(false);
        
        //PauseGame();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (gameMechanics.Score > 0){
            stepThreeManager(); 
        }

        if(GameMechanics.Lives < 5){
            regressionStep();
        }

        if (stepOne){
            progressionButton.SetActive(true);
        }
    }


    void PauseGame ()
    {
        Time.timeScale = 0;
    }
void ResumeGame ()
    {
        Time.timeScale = 1;
    }

void FixedUpdate()
{
    //STEP 1: Move the object

    // Handle keyboard input
    if (Input.GetKey(KeyCode.LeftArrow) && (stepOne == false))
    {
        tutorialtext.text = "Great! Press 'Next' to continue";
        
        stepOne = true; 
    }
    else if (Input.GetKey(KeyCode.RightArrow)&& (stepOne == false))
    {
        tutorialtext.text = "Great! Press 'Next' to continue";
        stepOne = true;
        
        
    }

    // Handle touch input
    if (Input.touchCount > 0 && (stepOne == false))
    {
        Touch touch = Input.GetTouch(0);
        float touchX = touch.position.x;
        float screenMiddleX = Screen.width / 2f;

        if (touchX < screenMiddleX)
        {
            tutorialtext.text = "Great! Press 'Next' to continue";
            stepOne = true;
        }
        else if (touchX > screenMiddleX)
        {
            tutorialtext.text = "Great! Press 'Next' to continue";
            stepOne = true;
        }
    }

}

public void stepTwoManager(){
    if (stepOne && !stepTwo){
    Time.timeScale = 1; 
    GameMechanics.Lives = 5; 
    gameMechanics.Score = 0;
    swipeIcon.SetActive(false);
    tutorialtext.text = "Words are falling from the sky! Catch the word that is being signed.";
    spawner.SetActive(true);
    practiceGIF.SetActive(true);
    stepTwo = true; 
    

    }

}


public void stepThreeManager(){
    tutorialtext.text = "Nice job! When you catch the RIGHT word, your score goes up. When you catch the WRONG word, you lose a life!";
    //Time.timeScale = 0;
    spawner.SetActive(false);
    progressionButton.SetActive(false);
    //lastStep.SetActive(true);
    tryAgain.SetActive(false);
    powerupTransition.SetActive(true); 
    practiceGIF.SetActive(false);
}

public void regressionStep(){
    tutorialtext.text = "Oops! Catching the WRONG word will make you lose a life. Try again!";
    Time.timeScale = 0;
    buttonText.text = "Try Again!";
    progressionButton.SetActive(false);
    stepTwo = false;
    tryAgain.SetActive(true);
    
    
    //make button text say "Try Again"
}


public void powerupShow(){
    tutorialText.SetActive(false);
    powerups.SetActive(true);
    playButton.SetActive(true);
}




public void switchScenes(){
    Globals.tutorial = false;
    SceneManager.LoadScene("Main Menu");

}




}
