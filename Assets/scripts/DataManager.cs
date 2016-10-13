using UnityEngine;
using System.Collections.Generic;
using InControl;

public static class DataManager
{

    public static List<InputDevice> PlayerList = new List<InputDevice>();

    private static int
        currentIndex,
        totalPlayers,
        currentMPH;

    private static float
        turnTime;

    private static int[]
        playerArr;

    private static bool
        randomPlayerOrder;

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

}
