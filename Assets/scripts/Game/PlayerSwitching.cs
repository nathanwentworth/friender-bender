using UnityEngine;
using System.Collections;

public class PlayerSwitching : MonoBehaviour
{

    private int
        totalPlayers,
        remainingPlayers;
    public int
        currentIndex = 0;
    public float
        timer,
        passingControllerTimer,
        passTime = 3,
        turnTime = 7;
    private bool
        randomPlayerOrder;
    public bool
        playerWin,
        passingController,
        DEBUG_MODE;
    private bool[]
        isOut;

    private void Start()
    {
        totalPlayers = DataManager.TotalPlayers;
        remainingPlayers = totalPlayers;
        isOut = new bool[remainingPlayers];
        for(int i = 0; i < totalPlayers; i++)
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
                    passingControllerTimer -= Time.deltaTime;
                    if(passingControllerTimer <= 0)
                    {

                    }
                }
            }
        }
    }

    private void SwitchPlayer()
    {
        if (DataManager.CurrentGameMode == DataManager.GameMode.Party)
        {
            int nextIndex = currentIndex + 1;
            for (int i = 0; i < totalPlayers; i++)
            {
                if (nextIndex + i == 4)
                {
                    nextIndex = 0;
                }
                else if (!isOut[nextIndex])
                {
                    break;
                }
            }
            // set the current index from the next index var
            currentIndex = nextIndex;
            // once the indexer runs through, start the timer again
            timer = turnTime;
        }
        else
        {
            Time.timeScale = 0;
            passingControllerTimer = passTime;
            passingController = true;
        }
    }

    public void RemovePlayer()
    {
        if (remainingPlayers > 1)
        {
            for (int i = 0; i < totalPlayers; i++)
            {
                if (i == currentIndex)
                {
                    isOut[i] = true;
                    remainingPlayers--;
                    Debug.Log("Removed player " + i + 1);
                    Debug.Log("Total players remaining: " + remainingPlayers);
                }
            }
        }
        if (remainingPlayers == 1)
        {
            playerWin = true;
        }
        else
        {
            SwitchPlayer();
        }

    }

}
