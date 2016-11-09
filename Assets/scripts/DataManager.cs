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
        car,
        livesCount;

    private static GameMode
        currentGameMode;

    private static float
        turnTime,
        potatoDelay,
        partyDelay,
        powerupCooldownTime;

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

    public static int CarIndex
    {
        get { return car; }
        set { car = value; }
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

    public static float PowerupCooldownTime
    {
        get { return powerupCooldownTime; }
        set { powerupCooldownTime = value; }
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
        PlayerPrefs.SetFloat("Party Delay", partyDelay);
        PlayerPrefs.SetFloat("Potato Delay", potatoDelay);
        PlayerPrefs.SetFloat("Powerup Cooldown", powerupCooldownTime);
        PlayerPrefs.Save();
        Debug.Log("Saved data");
    }

    public static void Load() {
        if (PlayerPrefs.HasKey("Turn Time")) {
            turnTime = PlayerPrefs.GetFloat("Turn Time");            
        } else {
            turnTime = 7f;
        }
        if (PlayerPrefs.HasKey("Party Delay")) {
            partyDelay = PlayerPrefs.GetFloat("Party Delay");            
        } else {
            partyDelay = 0f;
        }
        if (PlayerPrefs.HasKey("Potato Delay")) {
            potatoDelay = PlayerPrefs.GetFloat("Potato Delay");            
        } else {
            potatoDelay = 3f;
        }
        if (PlayerPrefs.HasKey("Powerup Cooldown")) {
            powerupCooldownTime = PlayerPrefs.GetFloat("Powerup Cooldown");            
        } else {
            powerupCooldownTime = 7f;
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
