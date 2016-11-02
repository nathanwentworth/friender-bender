using UnityEngine;
using System.Collections;
using InControl;
using System;

public class PowerUps : MonoBehaviour {

    public enum PowerUpType
    {
        None,
        SpeedBoost,
        SkipTurn
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
        foreach(PlayerData player in DataManager.PlayerList)
        {
            RandomPowerup(player);
        }
        pSwitch = gameObject.GetComponent<PlayerSwitching>();
        hud = pSwitch.hudManager;
    }

    void Update()
    {
        InputDevice controller = InputManager.ActiveDevice;
        if (DataManager.CurrentGameMode == DataManager.GameMode.Party)
        {
            if (controller.Action1.WasPressed)
            {
                PlayerData player = PlayerWhoPressedButton(controller);
                PowerUpType powerup = player.CurrentPowerUp;
                if (powerup == PowerUpType.None)
                {
                    return;
                }
                Debug.Log("Player " + player.PlayerNumber + "is using Powerup: " + powerup.ToString());
                Execute(powerup);
                player.CurrentPowerUp = PowerUpType.None;
                StartCoroutine(Cooldown(player));
            }
        }
        else if (DataManager.CurrentGameMode == DataManager.GameMode.HotPotato)
        {
            if (controller.Action1.WasPressed)
            {
                int currentIndex = gameObject.GetComponent<PlayerSwitching>().currentIndex;
                PlayerData player = DataManager.PlayerList[currentIndex];
                PowerUpType powerup = player.CurrentPowerUp;
                if (powerup == PowerUpType.None)
                {
                    return;
                }
                Debug.Log("Player " + player.PlayerNumber + " is using Powerup: " + powerup.ToString());
                Execute(powerup);
                player.CurrentPowerUp = PowerUpType.None;
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

    private void RandomPowerup(PlayerData player)
    {
        Array values = Enum.GetValues(typeof(PowerUpType));
        PowerUpType randomPowerup = (PowerUpType)values.GetValue(random.Next(1, values.Length));
        player.CurrentPowerUp = randomPowerup;
        Debug.Log("Player " + player.PlayerNumber.ToString() + " was given Powerup: " + randomPowerup.ToString());
    }

    private IEnumerator Cooldown(PlayerData player)
    {
        yield return new WaitForSeconds(powerupCooldownTime);
        Array values = Enum.GetValues(typeof(PowerUpType));
        PowerUpType randomPowerup = (PowerUpType)values.GetValue(random.Next(1, values.Length));
        player.CurrentPowerUp = randomPowerup;
        Debug.Log("Player " + player.PlayerNumber.ToString() + " was given Powerup: " + randomPowerup.ToString());
        hud.DisplayPowerups(player.PlayerNumber, randomPowerup.ToString());
    }
}
