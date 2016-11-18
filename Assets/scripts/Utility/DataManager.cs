﻿using UnityEngine;
using System.Collections.Generic;
using InControl;

public static class DataManager
{

    public static List<PlayerData> PlayerList = new List<PlayerData>();

    private static int
        totalPlayers,
        currentMPH,
        car;

    private static GameMode
        currentGameMode;

    private static float
        turnTime,
        potatoDelay,
        partyDelay,
        powerupCooldownTime;

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
    public static int LivesCount { get; set; }

    public static int ScreenResolution { get; set; }

    public static bool IsTrekkieTraxOn { get; set; }

    public static bool IsFullscreenOn { get; set; }

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

    public static void Save() {
        int trekkieInt = IsTrekkieTraxOn ? 1 : 0;
        int fullscreenInt = IsFullscreenOn ? 1 : 0;

        PlayerPrefs.SetFloat("Turn Time", turnTime);
        PlayerPrefs.SetFloat("Party Delay", partyDelay);
        PlayerPrefs.SetFloat("Potato Delay", potatoDelay);
        PlayerPrefs.SetFloat("Powerup Cooldown", powerupCooldownTime);
        PlayerPrefs.SetInt("Trekkie Trax Toggle", trekkieInt);
        PlayerPrefs.SetInt("Fullscreen Toggle", fullscreenInt);
        PlayerPrefs.Save();
        Debug.Log("Saved data");
    }

    public static void ClearGameData()
    {
        PlayerList.Clear();
        TotalPlayers = 0;
        CarIndex = 0;
        CurrentGameMode = GameMode.None;
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

        if (PlayerPrefs.HasKey("Trekkie Trax Toggle")) {
            if (PlayerPrefs.GetInt("Trekkie Trax Toggle") == 1) {
                IsTrekkieTraxOn = true;
            } else {
                IsTrekkieTraxOn = false;
            }
        } else {
            IsTrekkieTraxOn = true;
        }

        if (PlayerPrefs.HasKey("Fullscreen Toggle")) {
            if (PlayerPrefs.GetInt("Fullscreen Toggle") == 1) {
                IsFullscreenOn = true;
            } else {
                IsFullscreenOn = false;
            }
        } else {
            IsFullscreenOn = false;
        }
        

        Screen.fullScreen = IsFullscreenOn;
        Screen.SetResolution(
            Screen.resolutions[ScreenResolution].width,
            Screen.resolutions[ScreenResolution].height,
            IsFullscreenOn,
            Screen.resolutions[ScreenResolution].refreshRate
        );
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
                int name = PlayerList[index].PlayerNumber;
                return "P" + name;
            }            
        } else {
            return null;
        }
    }

}