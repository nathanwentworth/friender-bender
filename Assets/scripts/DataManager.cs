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
        turnTime,
        potatoDelay,
        partyDelay;

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

    public static Color32 ColorGreen = new Color32(105, 193, 165, 255);
    public static Color32 ColorOrange = new Color32(246, 143, 104, 255);
    public static Color32 ColorBlue = new Color32(140, 158, 201, 255);
    public static Color32 ColorPurple = new Color32(226, 140, 187, 255);

    public static List<Color32> Colors = new List<Color32> { 
        ColorGreen,
        ColorOrange,
        ColorBlue,
        ColorPurple
    };

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

    public static float PotatoDelay
    {
        get { return potatoDelay; }
        set { potatoDelay = value; }
    }

    public static float PartyDelay
    {
        get { return partyDelay; }
        set { partyDelay = value; }
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
        Debug.Log ("Turn Time: " + turnTime);
        PlayerPrefs.SetFloat("Turn Time", turnTime);
        Debug.Log ("Party Delay: " + partyDelay);
        PlayerPrefs.SetFloat("Party Delay", partyDelay);
        Debug.Log ("Potato Delay: " + potatoDelay);
        PlayerPrefs.SetFloat("Potato Delay", potatoDelay);
        PlayerPrefs.Save();
        Debug.Log("Saved data");
    }
    public static void Load() {
        turnTime = PlayerPrefs.GetFloat("Turn Time");
        Debug.Log ("Turn Time: " + turnTime);
        partyDelay = PlayerPrefs.GetFloat("Party Delay");
        Debug.Log ("Party Delay: " + partyDelay);
        potatoDelay = PlayerPrefs.GetFloat("Potato Delay");
        Debug.Log ("Potato Delay: " + potatoDelay);
        //Default Turn Time Set
        if(turnTime == 0)
        {
            turnTime = 7;
        }
        Debug.Log("Loaded data");
    }
    public static int RandomVal(int min, int max)
    {
        int i = Mathf.Clamp(Mathf.RoundToInt(Random.value * max), min, max);
        return i;
    }

    public static string GetPlayerIdentifier(int index) {
        if (PlayerList.Count > 0) {
            if (PlayerList[index].PlayerName != "" && PlayerList[index].PlayerName != null) {
                return PlayerList[index].PlayerName;
            } else {
                int name = PlayerList[index].PlayerNumber + 1;
                return "P" + name;
            }            
        } else {
            return null;
        }
    }

}
