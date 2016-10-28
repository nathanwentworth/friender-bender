using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerSwitching : MonoBehaviour
{

    private int
        totalPlayers,
        remainingPlayers;
    public int
        currentIndex = 0;
    public float
        timer,
        passingControllerTime,
        passTime = 3,
        turnTime;
    public bool
        playerWin,
        passingController,
        DEBUG_MODE;
    private bool[]
        isOut;
    public GameObject
        InControl;
    public HUDManager hudManager;

    private void Awake()
    {
        if(Application.isEditor && DataManager.CurrentGameMode == DataManager.GameMode.None)
        {
            DEBUG_MODE = true;
        }
        if (DEBUG_MODE)
        {
            Instantiate(InControl);
        }
    }

    private void Start()
    {
        Time.timeScale = 1f;
        turnTime = DataManager.TurnTime;
        totalPlayers = DataManager.TotalPlayers;
        remainingPlayers = totalPlayers;
        isOut = new bool[remainingPlayers];
        for (int i = 0; i < totalPlayers; i++)
        {
            isOut[i] = false;
        }
        // Start the timer
        timer = turnTime;
    }

    private void Update()
    {
        if (!DEBUG_MODE)
        {
            if (!playerWin)
            {
                if (timer > 0)
                {
                    timer -= Time.deltaTime;
                }
                else
                {
                    SwitchPlayer();
                }
                if (passingController)
                {
                    StartCoroutine(Sleep(3f));
                    passingController = false;
                }
            }
        }
    }

    private void SwitchPlayer()
    {
        int nextIndex = currentIndex;
        for (int i = 0; i < totalPlayers; i++)
        {
            nextIndex++;
            if (nextIndex + i == totalPlayers)
            {
                nextIndex = 0;
            }
            if (!isOut[nextIndex])
            {
                break;
            }
        }
        // set the current index from the next index var
        currentIndex = nextIndex;
        if (DataManager.CurrentGameMode == DataManager.GameMode.HotPotato)
        {
            string notifText1 = "PLAYER " + (currentIndex + 1) + " IS UP";
            // StartCoroutine(Notifications(notifText1, ""));
            hudManager.EnqueueAction(hudManager.DisplayNotificationText(notifText1));
            hudManager.EnqueueWait(2f);
            hudManager.EnqueueAction(hudManager.DisplayNotificationText(""));
            passingControllerTime = System.DateTime.Now.Second;
            Time.timeScale = 0;
            passingController = true;
        }
        timer = turnTime;
    }

    public void RemovePlayer()
    {
        isOut[currentIndex] = true;
        remainingPlayers--;
        if (remainingPlayers > 1) {
            string notifText1 = "PLAYER " + (currentIndex + 1) + " ELIMINATED!";
            string notifText2 = "PLAYERS LEFT: " + remainingPlayers;
            // StartCoroutine(Notifications(notifText1, notifText2));
            hudManager.EnqueueAction(hudManager.DisplayNotificationText(notifText1));
            hudManager.EnqueueWait(2f);
            hudManager.EnqueueAction(hudManager.DisplayNotificationText(notifText2));
            hudManager.EnqueueWait(2f);
            hudManager.EnqueueAction(hudManager.DisplayNotificationText(""));
        }
        Debug.Log("Removed player " + (currentIndex + 1));
        Debug.Log("Total players remaining: " + remainingPlayers);

        if (remainingPlayers == 1)
        {
            playerWin = true;
            for (int i = 0; i < totalPlayers; i++)
            {
                if (isOut[i] == false)
                {
                    hudManager.DisplayOverlayText("PLAYER " + (i + 1) + " WINS!");
                    Debug.Log("Player " + (i + 1) + " wins!");
                    break;
                }
            }
        }
        else
        {
            SwitchPlayer();
        }

    }



    private IEnumerator StartingCountdown() {
        Time.timeScale = 0.00001f;
        float pauseEndTime = Time.realtimeSinceStartup + 3;
        while (Time.realtimeSinceStartup < pauseEndTime)
        {
            yield return 0;
        }
        Time.timeScale = 1;
    }

    private IEnumerator Sleep(float wait) {
        Time.timeScale = 0.000001f;
        yield return new WaitForSeconds(wait * 0.000001f);
        Time.timeScale = 1f;
    }

    private IEnumerator Notifications(string notifText1, string notifText2) {
        StartCoroutine(hudManager.DisplayNotificationText(notifText1));
        yield return new WaitForSeconds(2);
        StartCoroutine(hudManager.DisplayNotificationText(notifText2));
    }

}
