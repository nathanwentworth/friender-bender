using UnityEngine;
using System.Collections.Generic;
using InControl;

public static class DataManager
{

    public static List<PlayerData> PlayerList = new List<PlayerData>();

    private static int
        currentIndex,
        totalPlayers,
        currentMPH,
        livesCount;

    private static GameMode
        currentGameMode;

    private static float
        turnTime;

    private static int[]
        playerArr;

    private static bool
        randomPlayerOrder;

    public enum GameMode
    {
        None,
        Party,
        HotPotato
    }

    public static GameMode CurrentGameMode
    {
        get { return currentGameMode; }
        set { currentGameMode = value; }
    }

    public static int CurrentIndex
    {
        get { return currentIndex; }
        set { currentIndex = value; }
    }
    public static int TotalPlayers
    {
        get { return totalPlayers; }
        set { totalPlayers = value; }
    }
    public static int CurrentMPH
    {
        get { return currentMPH; }
        set { currentMPH = value; }
    }
    public static int LivesCount
    {
        get { return livesCount; }
        set { livesCount = value; }
    }

    public static float TurnTime
    {
        get { return turnTime; }
        set { turnTime = value; }
    }

    public static int[] PlayerArr
    {
        get { return playerArr; }
        set { playerArr = value; }
    }

    public static bool RandomPlayerOrder
    {
        get { return randomPlayerOrder; }
        set { randomPlayerOrder = value; }
    }

    public static void Save() {
        PlayerPrefs.SetFloat("Turn Time", turnTime);
        PlayerPrefs.Save();
        Debug.Log("Saved data");
    }
    public static void Load() {
        turnTime = PlayerPrefs.GetFloat("Turn Time");
        //Default Turn Time Set
        if(turnTime == 0)
        {
            turnTime = 7;
        }
        Debug.Log("Loaded data");
    }
    public static int RandomVal(int min, int max) {
        int i = Mathf.Clamp(Mathf.RoundToInt(Random.value * max), min, max);
        return i;
    }

}
