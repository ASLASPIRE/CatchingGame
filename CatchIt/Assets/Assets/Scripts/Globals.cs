using PlayFab.ClientModels;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Globals : MonoBehaviour
{
    public enum Difficulty
    {
        Easy,
        Medium,
        Hard
    }

    public enum Vocab
    {
        Chemistry,
        Biology,
        FoodWeb,
        PartsOfTheCell
    }
    public static TextAsset jsonFile;
    public static string vocabSet = "PartsOfTheCell"; //to be deprecated
    public static float fallingSpeed = 0.2f;
    public static float spawnRate = 0.9f;
    public static int incrementValue = 100;
    public static List<PlayerLeaderboardEntry> leaderboardEntries = new List<PlayerLeaderboardEntry>();
    public static  bool tutorial = false;
    public static Difficulty difficulty = Difficulty.Easy;
    public static Vocab vocabList = Vocab.Chemistry;
    public static int CurrentLevel = 1;
    public static int TotalLevels = 4;

}
