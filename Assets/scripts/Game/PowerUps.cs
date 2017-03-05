using UnityEngine;
using System.Collections;
using InControl;
using System;

public class PowerUps : MonoBehaviour
{

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
        LargeObject,
        Teleport,
        LandMine
    }

    private bool firstPowerUpsGiven = false;

    public float powerupCooldownTime;
    [Header("SpeedBoost")]
    public int sb_force;
    [Header("Glitch")]
    public GameObject cam;
    [Header("Data References")]
    public AudioManager audioManager;
    [Header("LargeObject")]
    public GameObject[] objects;
    [Header("LandMine")]
    public GameObject mineObject;

    [Header("Starting Powerup")]
    [Tooltip("Select None for a random powerup")]
    public PowerUpType StartingPowerUp;

    private PlayerSwitching pSwitch;
    private HUDManager hud;
    private CarControl carControl;
    private bool shieldActive;

    private GameObject car;
    [SerializeField]
    private CameraNoise cameraNoise;

    void Start()
    {
        powerupCooldownTime = DataManager.PowerupCooldownTime;
        car = GameObject.FindGameObjectWithTag("Player");
        carControl = car.GetComponent<CarControl>();
        pSwitch = gameObject.GetComponent<PlayerSwitching>();
        hud = pSwitch.hudManager;
        foreach (PlayerData player in DataManager.PlayerList)
        {
            player.Lives = DataManager.LivesCount;
            if (StartingPowerUp != PowerUpType.None && pSwitch.DEBUG_MODE)
            {
                player.CurrentPowerUp = StartingPowerUp;
            }
            else {
                StartCoroutine(Cooldown(player));
                // RandomPowerup(player);
            }
        }
    }

    void Update()
    {
        //Debug Mode
        if (pSwitch.DEBUG_MODE)
        {
            if (InputManager.ActiveDevice.Action1.WasPressed)
            {
                PowerUpType powerup = StartingPowerUp;
                Execute(powerup);
                Debug.Log("DEBUG MODE: Using Powerup: " + StartingPowerUp);
            }
        }
        else {
            //Check if there was powerup input
            bool action1WasPressed = false;
            PlayerData playerThatPressedIt = null;
            if (DataManager.CurrentGameMode == DataManager.GameMode.Party)
            {
                foreach (PlayerData i in DataManager.PlayerList)
                {
                    if (i.Controller.Action1.WasPressed && i.CurrentPowerUp != PowerUpType.None)
                    {
                        action1WasPressed = true;
                        playerThatPressedIt = i;
                        break;
                    }
                }
            }
            else
            {
                PlayerData i = DataManager.PlayerList[0];
                if (i.Controller.Action1.WasPressed && i.CurrentPowerUp != PowerUpType.None)
                {
                    action1WasPressed = true;
                    playerThatPressedIt = i;
                }
            }
            //Execute if there was any input
            if (action1WasPressed && !pSwitch.passingController && !pSwitch.startingGame && !pSwitch.playerWin && !hud.Paused)
            {
                PlayerData player;
                if (DataManager.CurrentGameMode == DataManager.GameMode.Party)
                {
                    player = playerThatPressedIt;
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
                Debug.Log("Player " + player.PlayerNumber + " is using Powerup: " + powerup.ToString());
                Execute(powerup);
                player.CurrentPowerUp = PowerUpType.None;
                hud.DisplayPowerups(player.PlayerNumber, " ");
                StartCoroutine(Cooldown(player));
            }
        }
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
            case PowerUpType.Teleport:
                StartCoroutine(Teleport());
                break;
            case PowerUpType.LandMine:
                StartCoroutine(LandMine());
                break;
            default:
                Debug.LogError("Powerup: Powerup you tried to use doesnt exist.");
                break;
        }
    }

    private IEnumerator SpeedBoost(GameObject car, int maxForce)
    {
        StartCoroutine(audioManager.PowerupSounds("speedBoost"));
        StartCoroutine(cameraNoise.ScreenShake(0.3f, 20f));
        int i = 0;
        while (i < 20)
        {
            Rigidbody rigid = car.GetComponent<Rigidbody>();
            rigid.AddForce(car.transform.forward * maxForce, ForceMode.Acceleration);
            if (!car.GetComponent<CarControl>().grounded) {
                DataManager.airBoost = true;
            }
            yield return new WaitForSeconds(0.01f);
            i++;
        }
    }

    private IEnumerator SkipTurn()
    {
        StartCoroutine(audioManager.PowerupSounds("skipTurn"));
        pSwitch.SkipPlayer();
        string skippedText = DataManager.GetPlayerIdentifier(pSwitch.NextPlayer()) + " UP NEXT";
        hud.EnqueueAction(hud.DisplayOverlayText(skippedText));
        yield return null;
    }

    private IEnumerator AddSecondsToTurn()
    {
        StartCoroutine(audioManager.PowerupSounds("addTwoSeconds"));
        pSwitch.timer = pSwitch.timer + 2.5f;
        string text = "+2 SECONDS";
        Debug.Log("Adding 2 seconds to time");
        hud.EnqueueAction(hud.DisplayNotificationText(text));
        StartCoroutine(hud.TimerTextPop());
        yield return null;
    }

    private IEnumerator InvertSteering()
    {
        StartCoroutine(audioManager.PowerupSounds("inverse"));
        carControl.turningMultiplier = -1;
        string text = "TURNING MIRRORED";
        StartCoroutine(hud.DisplayOverlayText(text));
        hud.overlayText.transform.GetComponent<Animator>().SetTrigger("OverlayRotate");
        yield return new WaitForSeconds(3f);
        carControl.turningMultiplier = 1;
    }

    private IEnumerator EndTurn()
    {
        StartCoroutine(audioManager.PowerupSounds("endTurn"));
        StartCoroutine(hud.DisplayOverlayText("TURN ENDED"));
        hud.overlayText.transform.GetComponent<Animator>().SetTrigger("OverlayRotate");
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
        float mph = carControl.MPH;
        float dist = mph / 150f;
        GameObject i = Instantiate(objects[(int)UnityEngine.Random.Range(0, objects.Length)]);
        // car.transform.position + (car.transform.forward + (30f * dist)) + (Vector3.up * 7),
        i.transform.position = car.transform.position + (car.transform.forward * (30f * dist)) + (Vector3.up * 7);
        foreach (Rigidbody rigid in i.GetComponentsInChildren<Rigidbody>())
        {
            rigid.AddForce(Vector3.down * 15, ForceMode.VelocityChange);
        }
        StartCoroutine(audioManager.PowerupSounds("randomLargeObject"));
        yield return null;
    }

    private void RandomPowerup(PlayerData player)
    {
        float rand = UnityEngine.Random.Range(0.00f, 100.00f);
        Debug.Log(rand);
        if (IsNumBetweenValues(rand, 0, 14.17f))
        {
            player.CurrentPowerUp = PowerUpType.SpeedBoost;
        }
        else if (IsNumBetweenValues(rand, 14.18f, 21.18f))
        {
            player.CurrentPowerUp = PowerUpType.SkipTurn;
        }
        else if (IsNumBetweenValues(rand, 21.19f, 32.02f))
        {
            player.CurrentPowerUp = PowerUpType.AddSecondsToTurn;
        }
        else if (IsNumBetweenValues(rand, 32.03f, 41.70f))
        {
            player.CurrentPowerUp = PowerUpType.InvertSteering;
        }
        else if (IsNumBetweenValues(rand, 41.71f, 55.38f))
        {
            player.CurrentPowerUp = PowerUpType.ScreenDistraction;
        }
        else if (IsNumBetweenValues(rand, 55.39f, 67.39f))
        {
            player.CurrentPowerUp = PowerUpType.Shield;
        }
        else if (IsNumBetweenValues(rand, 67.40f, 72.40f))
        {
            player.CurrentPowerUp = PowerUpType.EndTurn;
        }
        else if (IsNumBetweenValues(rand, 72.41f, 84.58f))
        {
            player.CurrentPowerUp = PowerUpType.LargeObject;
        }
        else if (IsNumBetweenValues(rand, 84.59f, 90.42f))
        {
            player.CurrentPowerUp = PowerUpType.Teleport;
        }
        else if (IsNumBetweenValues(rand, 90.43f, 100))
        {
            player.CurrentPowerUp = PowerUpType.LandMine;
        }
        else
        {
            Debug.LogError("Something went wrong with the drop table, defaulting to speedboost");
            player.CurrentPowerUp = PowerUpType.SpeedBoost;
        }
        hud.DisplayPowerups(player.PlayerNumber, GetPowerupName(player.CurrentPowerUp));
        hud.BouncePowerup(player.PlayerNumber);

        firstPowerUpsGiven = true;
    }

    private bool IsNumBetweenValues(float value, float min, float max)
    {
        if (value >= min && value <= max)
        {
            return true;
        }
        return false;
    }

    public string GetPowerupName(PowerUpType powerup)
    {
        string powerupName = "";
        switch (powerup)
        {
            case PowerUpType.SpeedBoost:
                powerupName = "BOOST";
                break;
            case PowerUpType.SkipTurn:
                powerupName = "SKIP NEXT PLAYER";
                break;
            case PowerUpType.AddSecondsToTurn:
                powerupName = "+2 SECONDS";
                break;
            case PowerUpType.InvertSteering:
                powerupName = "MIRROR STEERING";
                break;
            case PowerUpType.ScreenDistraction:
                powerupName = "CAMERA GLITCH";
                break;
            case PowerUpType.Shield:
                powerupName = "SHIELD";
                break;
            case PowerUpType.EndTurn:
                powerupName = "END TURN";
                break;
            case PowerUpType.LargeObject:
                powerupName = "OBSTACLE DROP";
                break;
            case PowerUpType.Teleport:
                powerupName = "TELEPORT";
                break;
            case PowerUpType.LandMine:
                powerupName = "LANDMINE";
                break;
            default:
                Debug.LogError("Powerup: Powerup you tried to use doesnt exist.");
                break;
        }
        return powerupName;
    }

    private IEnumerator Teleport()
    {
        carControl.teleportEffect.GetComponent<ParticleSystem>().Play();
        StartCoroutine(audioManager.PowerupSounds("teleport"));
        yield return new WaitForSeconds(1.2f);
        carControl.teleportEffect.GetComponent<ParticleSystem>().Stop();
        Transform sp = pSwitch.spawnPoints[DataManager.RandomVal(0, pSwitch.spawnPoints.Length - 1)].transform;
        car.transform.position = sp.position;
        yield return null;
    }

    private IEnumerator LandMine()
    {
        StartCoroutine(audioManager.PowerupSounds("landMine"));
        GameObject landMine = Instantiate(mineObject);
        landMine.transform.rotation = car.transform.rotation;
        landMine.transform.position = car.transform.position + ((-car.transform.forward * 1.5f) + (Vector3.up * 0.5f));
        yield return null;
    }


    private IEnumerator Cooldown(PlayerData player)
    {
        float cooldown = powerupCooldownTime;
        while (cooldown > 0)
        {
            float cooldownDisp = cooldown;
            cooldownDisp = Mathf.Round(cooldownDisp * 10f) / 10f;
            hud.DisplayPowerups(player.PlayerNumber, cooldownDisp + "");
            cooldown -= Time.deltaTime;

            if (firstPowerUpsGiven) {

                int usedPowerup = 0;

                foreach (PlayerData _player in DataManager.PlayerList)
                {
                    if (_player.CurrentPowerUp == PowerUpType.None)
                    {
                        usedPowerup++;
                    }
                }

                if (usedPowerup >= DataManager.PlayerList.Count) {
                    DataManager.allPowerupsAtOnce = true;
                }

            }


            yield return new WaitForEndOfFrame();
        }
        // yield return new WaitForSeconds(powerupCooldownTime);
        RandomPowerup(player);
    }
}
