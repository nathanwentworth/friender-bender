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
        turnTime = 7;
    private bool
        randomPlayerOrder;
    public bool
        playerWin,
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
            }
        }
    }

    private void SwitchPlayer()
    {
        int nextIndex = currentIndex + 1;
        for(int i = 0; i < totalPlayers; i++)
        {
            if(nextIndex + i == 4)
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
