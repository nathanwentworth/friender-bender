using UnityEngine;
using System.Collections;
using InControl;

public class PlayerData {

    private InputDevice controller;
    private PowerUps.PowerUpType currentPowerUp;
    private int playerNum;
    private string playerName;
    private int lives;

    public bool deviceDetatched { get; set; }

    public InputDevice Controller
    {
        get { return controller; }
        set { controller = value; }
    }

    public PowerUps.PowerUpType CurrentPowerUp
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

    public int Lives
    {
        get { return lives; }
        set { lives = value; }
    }
}
