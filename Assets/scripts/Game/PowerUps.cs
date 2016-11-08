using UnityEngine;
using System.Collections;
using InControl;
using System;

public class PowerUps : MonoBehaviour {

    public enum PowerUpType
    {
        None,
        SpeedBoost,
        SkipTurn,
        AddSecondsToTurn,
        InvertSteering,
        ScreenDistraction,
        Shield,
        EndTurn,
        LargeObject
    }

    public float powerupCooldownTime = 7;
    [Header("SpeedBoost")]
    public int sb_force;
    [Header("Glitch")]
    public GameObject cam;
    [Header("Data References")]
    public AudioManager audioManager;
    [Header("LargeObject")]
    public GameObject[] objects;
    private Transform spawn;

    [Header("Starting Powerup")]
    [Tooltip("Select None for a random powerup")]
    public PowerUpType StartingPowerUp;

    private PlayerSwitching pSwitch;
    private HUDManager hud;
    private CarControl carControl;
    private bool shieldActive;


    private GameObject car;

    void Start()
    {
        spawn = GameObject.FindGameObjectWithTag("Spawn").transform;
        car = GameObject.FindGameObjectWithTag("Player");
        carControl = car.GetComponent<CarControl>();
        pSwitch = gameObject.GetComponent<PlayerSwitching>();
        hud = pSwitch.hudManager;
        foreach(PlayerData player in DataManager.PlayerList)
        {
            player.Lives = DataManager.LivesCount;
            if (StartingPowerUp != PowerUpType.None)
            {
                player.CurrentPowerUp = StartingPowerUp;
            }
            else {
                RandomPowerup(player);
            }
        }
    }

    void Update()
    {
        InputDevice controller = InputManager.ActiveDevice;
        if (pSwitch.DEBUG_MODE && controller.Action1.WasPressed)
        {
            PowerUpType powerup = StartingPowerUp;
            Execute(powerup);
            Debug.Log("DEBUG MODE: Using Powerup: " + StartingPowerUp);
        }
        else {
            if (controller.Action1.WasPressed && !pSwitch.passingController && !pSwitch.startingGame && !pSwitch.playerWin) 
            {
                PlayerData player;
                if (DataManager.CurrentGameMode == DataManager.GameMode.Party)
                {
                    player = PlayerWhoPressedButton(controller);
                }
                else {
                    int currentIndex = gameObject.GetComponent<PlayerSwitching>().currentIndex;
                    player = DataManager.PlayerList[currentIndex];
                }
                PowerUpType powerup = player.CurrentPowerUp;
                if (powerup == PowerUpType.None)
                {
                    return;
                }
                Debug.Log("Player " + player.PlayerNumber + "is using Powerup: " + powerup.ToString());
                Execute(powerup);
                player.CurrentPowerUp = PowerUpType.None;
                hud.DisplayPowerups(player.PlayerNumber, " ");
                StartCoroutine(Cooldown(player));

            }
        }
    }

    public PlayerData PlayerWhoPressedButton(InputDevice controller)
    {
        foreach (PlayerData player in DataManager.PlayerList)
        {
            if(controller == player.Controller)
            {
                return player;
            }
        }
        Debug.LogError("Powerup: Could not find player that used powerup");
        return null;
    }

    public void Execute(PowerUpType powerup)
    {
        switch (powerup)
        {
            case PowerUpType.SpeedBoost:
                StartCoroutine(SpeedBoost(car, sb_force));
                break;
            case PowerUpType.SkipTurn:
                StartCoroutine(SkipTurn());
                break;
            case PowerUpType.AddSecondsToTurn:
                StartCoroutine(AddSecondsToTurn());
                break;
            case PowerUpType.InvertSteering:
                StartCoroutine(InvertSteering());
                break;
            case PowerUpType.ScreenDistraction:
                StartCoroutine(ScreenDistraction());
                break;
            case PowerUpType.Shield:
                StartCoroutine("Shield");
                break;
            case PowerUpType.EndTurn:
                StartCoroutine(EndTurn());
                break;
            case PowerUpType.LargeObject:
                StartCoroutine(RandomLargeObject());
                break;
            default:
                Debug.LogError("Powerup: Powerup you tried to use doesnt exist.");
                break;
        }
    }

    private IEnumerator SpeedBoost(GameObject car, int maxForce)
    {
        StartCoroutine(audioManager.PowerupSounds("speedBoost"));
        int i = 0;
        while (i < 20)
        {
            Rigidbody rigid = car.GetComponent<Rigidbody>();
            rigid.AddForce(car.transform.forward * maxForce, ForceMode.Acceleration);
            yield return new WaitForSeconds(0.01f);
            i++;
        }
    }

    private IEnumerator SkipTurn()
    {
        pSwitch.SkipPlayer();
        string skippedText = "PLAYER " + (pSwitch.NextPlayer() + 1) + " SKIPPED";
        hud.EnqueueAction(hud.DisplayNotificationText(skippedText));
        yield return null;
    }

    private IEnumerator AddSecondsToTurn()
    {
        pSwitch.timer = pSwitch.timer + 2.5f;
        string timerText = "+2 SECONDS";
        Debug.Log("Adding 2 seconds to time");
        hud.EnqueueAction(hud.DisplayNotificationText(timerText));
        yield return null;
    }

    private IEnumerator InvertSteering()
    {
        carControl.turningMultiplier = -1;
        string timerText = "TURNING REVERSED";
        hud.EnqueueAction(hud.DisplayNotificationText(timerText));
        yield return new WaitForSeconds(3f);
        carControl.turningMultiplier = 1;
    }

    private IEnumerator EndTurn()
    {
        pSwitch.timer = 0;
        yield return null;
    }

    private IEnumerator ScreenDistraction()
    {
        StartCoroutine(audioManager.PowerupSounds("distraction"));
        cam.GetComponent<AnalogGlitch>().enabled = true;
        yield return new WaitForSeconds(3f);
        cam.GetComponent<AnalogGlitch>().enabled = false;
    }

    private IEnumerator Shield()
    {
        carControl.shield = true;
        string timerText = "SHIELD ACTIVE";
        hud.EnqueueAction(hud.DisplayNotificationText(timerText));
        hud.EnqueueWait(1f);
        hud.EnqueueAction(hud.DisplayNotificationText(""));
        yield return null;
    }

    private IEnumerator RandomLargeObject()
    {
        GameObject i = Instantiate(objects[0]);
        i.transform.position = spawn.position;
        yield return null;
    }

    private void RandomPowerup(PlayerData player)
    {
        Array values = Enum.GetValues(typeof(PowerUpType));
        int rand = DataManager.RandomVal(1, values.Length - 1);
        PowerUpType randomPowerup = (PowerUpType)values.GetValue(rand);
        player.CurrentPowerUp = randomPowerup;
        hud.DisplayPowerups(player.PlayerNumber, randomPowerup.ToString());
        Debug.Log("Player " + player.PlayerNumber.ToString() + " was given Powerup: " + randomPowerup.ToString());
    }

    private IEnumerator Cooldown(PlayerData player)
    {
        Debug.Log("Starting Cooldown for Player " + player.PlayerNumber);
        yield return new WaitForSeconds(powerupCooldownTime);
        RandomPowerup(player);
    }
}
