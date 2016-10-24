using UnityEngine;
using System.Collections;
using InControl;

public class PlayerData {

    private InputDevice controller;
    private PowerUps.PowerUpTypes currentPowerUp;
    private int playerNum;
    private string playerName;

    public InputDevice Controller
    {
        get { return controller; }
        set { controller = value; }
    }

    public PowerUps.PowerUpTypes CurrentPowerUp
    {
        get { return currentPowerUp; }
        set { currentPowerUp = value; }
    }

    public int PlayerNumber
    {
        get { return playerNum; }
        set { playerNum = value; }
    }

    public string PlayerName
    {
        get { return playerName; }
        set { playerName = value; }
    }
}
