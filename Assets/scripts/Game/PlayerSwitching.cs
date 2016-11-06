﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerSwitching : MonoBehaviour
{

    private int
        totalPlayers,
        remainingPlayers;
    public int
        currentIndex = 0;
    [HideInInspector]
    public float
        timer;
    public float
        turnTime;
    private bool
        skipTurn;
    public bool
        playerWin,
        startingGame,
        DEBUG_MODE,
        passingController;
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
        if (!DEBUG_MODE)
        {
            StartCoroutine(StartingCountdown());
        }
    }

    private void Update()
    {
        if (!DEBUG_MODE)
        {
            if (!playerWin && !startingGame)
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
                    StartCoroutine(Sleep(DataManager.PotatoDelay));
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
            if (nextIndex >= totalPlayers)
            {
                nextIndex = 0;
            }
            if (!isOut[nextIndex] && skipTurn)
            {
                skipTurn = false;
            }
            else if(!isOut[nextIndex] && !skipTurn)
            {
                break;
            }
        }
        // set the current index from the next index var
        currentIndex = nextIndex;

        string notifText1 = DataManager.GetPlayerIdentifier(currentIndex) + " IS UP";

        hudManager.EnqueueAction(hudManager.DisplayNotificationText(notifText1));
        hudManager.EnqueueWait(2f);
        hudManager.EnqueueAction(hudManager.DisplayNotificationText(""));

        if (DataManager.CurrentGameMode == DataManager.GameMode.HotPotato)
        {
            // passingController = true;
            StartCoroutine(Sleep(DataManager.PotatoDelay));
        } else {
            StartCoroutine(Sleep(DataManager.PartyDelay));
        }
        hudManager.UpdateLivesDisplay();
        timer = turnTime;
        GameObject.FindGameObjectWithTag("Player").GetComponent<CarControl>().shield = false;
    }

    public int NextPlayer()
    {
        int nextIndex = currentIndex;
        for (int i = 0; i < totalPlayers; i++)
        {
            nextIndex++;
            if (nextIndex >= totalPlayers)
            {
                nextIndex = 0;
            }
            if (!isOut[nextIndex])
            {
                return nextIndex;
            }
        }
        return -1;
    }

    public void SkipPlayer()
    {
        skipTurn = true;
    }

    public void RemovePlayer()
    {
        isOut[currentIndex] = true;
        remainingPlayers--;
        if (remainingPlayers > 1) {
            string notifText1 = DataManager.GetPlayerIdentifier(currentIndex) + " ELIMINATED!";
            string notifText2 = "PLAYERS LEFT: " + remainingPlayers;
            // StartCoroutine(Notifications(notifText1, notifText2));
            hudManager.EnqueueAction(hudManager.DisplayNotificationText(notifText1));
            hudManager.EnqueueWait(2f);
            hudManager.EnqueueAction(hudManager.DisplayNotificationText(notifText2));
            hudManager.EnqueueWait(2f);
            hudManager.EnqueueAction(hudManager.DisplayNotificationText(""));
        }

        if (remainingPlayers == 1)
        {
            playerWin = true;
            for (int i = 0; i < totalPlayers; i++)
            {
                if (isOut[i] == false)
                {

                    hudManager.DisplayOverlayText(DataManager.GetPlayerIdentifier(i) + " WINS!");
                    StartCoroutine(hudManager.DisplayPostGameMenu());
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
        startingGame = true;
        Rigidbody carRigid = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody>();
        carRigid.constraints = RigidbodyConstraints.FreezeAll;
        hudManager.EnqueueAction(hudManager.DisplayNotificationText("3"));
        hudManager.EnqueueWait(1f);
        hudManager.EnqueueAction(hudManager.DisplayNotificationText("2"));
        hudManager.EnqueueWait(1f);
        hudManager.EnqueueAction(hudManager.DisplayNotificationText("1"));
        hudManager.EnqueueWait(1f);
        hudManager.EnqueueAction(hudManager.DisplayNotificationText("BEND YOUR FRIENDS!"));
        hudManager.EnqueueWait(1f);
        hudManager.EnqueueAction(hudManager.DisplayNotificationText(""));
        yield return new WaitForSeconds(3);
        startingGame = false;
        carRigid.constraints = RigidbodyConstraints.None;
    }


    private IEnumerator Sleep(float wait) {
        Time.timeScale = 0.000001f;
        yield return new WaitForSeconds(wait * 0.000001f);
        passingController = false;
        Time.timeScale = 1f;
    }

    private IEnumerator Notifications(string notifText1, string notifText2) {
        StartCoroutine(hudManager.DisplayNotificationText(notifText1));
        yield return new WaitForSeconds(2);
        StartCoroutine(hudManager.DisplayNotificationText(notifText2));
    }

}
