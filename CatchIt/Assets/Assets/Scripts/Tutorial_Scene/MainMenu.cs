using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MainMenu : MonoBehaviour
{

    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject tutorialPanel;
    [SerializeField] private GameObject vocabPanel;
    [SerializeField] private GameObject difficultyPanel;

    [SerializeField] private TextAsset biologyJson;
    [SerializeField] private TextAsset chemistryJson;

    // Start is called before the first frame update
    void Start()
    {   Globals.level = 1;
        Debug.Log("Started");
        mainMenuPanel.SetActive(true);
        //tutorialPanel.SetActive(false);
        vocabPanel.SetActive(false);
        difficultyPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void onTutorialButtonPress()
    {
        Debug.Log("Tutorial Button Pressed");
        Globals.tutorial = true; 
        mainMenuPanel.SetActive(false);
        SceneManager.LoadScene("Tutorial");
        tutorialPanel.SetActive(true);
    }

    public void onPlayButtonPress()
    {
        Debug.Log("Play Button Pressed");
        Globals.tutorial = false;
        mainMenuPanel.SetActive(false);
        vocabPanel.SetActive(true);
    }

    public void onBackButtonPress()
    {
        mainMenuPanel.SetActive(true);
        tutorialPanel.SetActive(false);
    }

    public void onBiologyButtonPress()
    {
        Globals.jsonFile = biologyJson;
        Globals.vocabSet = "Heredity";
        vocabPanel.SetActive(false);
        difficultyPanel.SetActive(true);
    }

    public void onChemistryButtonPress()
    {
        Globals.jsonFile = chemistryJson;
        Globals.vocabSet = "Chemistry";
        vocabPanel.SetActive(false);
        difficultyPanel.SetActive(true);
    }

    public void onPartsOfTheCellButtonPress()
    {
        Globals.vocabSet = "PartsOfTheCell";
        vocabPanel.SetActive(false);
        difficultyPanel.SetActive(true);
    }

    public void onFoodWebButtonPress()
    {
        Globals.vocabSet = "FoodWeb";
        vocabPanel.SetActive(false);
        difficultyPanel.SetActive(true);
    }

    public void onEasyButtonPress()
    {
        Globals.fallingSpeed = 0.2f;
        Globals.spawnRate = 1.2f;
        Globals.incrementValue = 100;
        SceneManager.LoadScene("Gameplay");
    }

    public void onMediumButtonPress()
    {
        Globals.fallingSpeed = 0.2f;
        Globals.spawnRate = 0.9f;
        Globals.incrementValue = 150;
        SceneManager.LoadScene("Gameplay");
    }

    public void onHardButtonPress()
    {
        Globals.fallingSpeed = 0.4f;
        Globals.spawnRate = 0.7f;
        Globals.incrementValue = 200;
        SceneManager.LoadScene("Gameplay");
    }
}
