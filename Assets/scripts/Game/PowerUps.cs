using UnityEngine;
using System.Collections;
using InControl;
using System;

public class PowerUps : MonoBehaviour {

    public enum PowerUpType
    {
        None,
        SpeedBoost
    }
    public float powerupCooldownTime = 7;
    [Header("SpeedBoost")]
    public int sb_force;

    private GameObject car;

    void Start()
    {
        car = GameObject.FindGameObjectWithTag("Player");
        foreach(PlayerData player in DataManager.PlayerList)
        {
            player.CurrentPowerUp = PowerUpType.SpeedBoost;
            Debug.Log("Giving all players in list SpeedBoost");
        }
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
                int currentIndex = car.GetComponent<CarControl>().currentIndex;
                PlayerData player = DataManager.PlayerList[currentIndex];
                PowerUpType powerup = player.CurrentPowerUp;
                if (powerup == PowerUpType.None)
                {
                    return;
                }
                Debug.Log("Player " + player.PlayerNumber + "is using Powerup: " + powerup.ToString());
                Execute(powerup);
                player.CurrentPowerUp = PowerUpType.None;
                Debug.Log(DataManager.PlayerList[currentIndex].CurrentPowerUp);
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

    private IEnumerator Cooldown(PlayerData player)
    {
        yield return new WaitForSeconds(powerupCooldownTime);
        Array values = Enum.GetValues(typeof(PowerUpType));
        System.Random random = new System.Random();
        PowerUpType randomPowerup = (PowerUpType)values.GetValue(random.Next(1, values.Length));
        player.CurrentPowerUp = randomPowerup;
        Debug.Log("Player " + player.PlayerNumber.ToString() + " was given Powerup: " + randomPowerup.ToString());
    }
}
