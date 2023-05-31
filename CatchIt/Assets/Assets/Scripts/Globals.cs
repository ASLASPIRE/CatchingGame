using PlayFab.ClientModels;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Globals : MonoBehaviour
{
    public static TextAsset jsonFile;
    public static string vocabSet = "PartsOfTheCell";
    public static int level = 1; 
    public static float fallingSpeed = 0.2f;
    public static float spawnRate = 0.9f;
    public static int incrementValue = 150;
    public static List<PlayerLeaderboardEntry> leaderboardEntries = new List<PlayerLeaderboardEntry>();
    public static  bool tutorial = false;
}
