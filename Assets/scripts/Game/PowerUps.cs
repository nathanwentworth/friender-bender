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
        AddSecondsToTurn
    }
    public float powerupCooldownTime = 7;
    [Header("SpeedBoost")]
    public int sb_force;

    private PlayerSwitching pSwitch;
    private HUDManager hud;


    private GameObject car;
    System.Random random = new System.Random();

    void Start()
    {
        car = GameObject.FindGameObjectWithTag("Player");
        pSwitch = gameObject.GetComponent<PlayerSwitching>();
        hud = pSwitch.hudManager;
        foreach(PlayerData player in DataManager.PlayerList)
        {
            RandomPowerup(player);
        }
    }

    void Update()
    {
        InputDevice controller = InputManager.ActiveDevice;

        if (controller.Action1.WasPressed && !pSwitch.passingController) {
            PlayerData player;
            if (DataManager.CurrentGameMode == DataManager.GameMode.Party) {
                player = PlayerWhoPressedButton(controller);
            } else {
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
            Debug.LogWarning(player.CurrentPowerUp + "ybyb");
            hud.DisplayPowerups(player.PlayerNumber, " ");
            StartCoroutine(Cooldown(player));

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
            case PowerUpType.ReversedTurning:
                StartCoroutine(ReversedTurning());
                break;
            default:
                Debug.LogError("Powerup: Powerup you tried to use doesnt exist.");
                break;
        }
    }

    private IEnumerator SpeedBoost(GameObject car, int maxForce)
    {
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

    private IEnumerator ReversedTurning()
    {
        pSwitch.timer = pSwitch.timer + 2.5f;
        string timerText = "+2 SECONDS";
        hud.EnqueueAction(hud.DisplayNotificationText(timerText));
        yield return null;
    }

    private void RandomPowerup(PlayerData player)
    {
        Array values = Enum.GetValues(typeof(PowerUpType));
        PowerUpType randomPowerup = (PowerUpType)values.GetValue(UnityEngine.Random.Range(1, values.Length));
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
