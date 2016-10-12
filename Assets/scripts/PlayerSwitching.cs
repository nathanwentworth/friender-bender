using UnityEngine;
using System.Collections;
using System.Collections.Generic;
// remove scenemanagement later!
using UnityEngine.SceneManagement;

public class PlayerSwitching : MonoBehaviour
{

    private int
        currentIndex = 0,
        nextIndex,
        totalPlayers;
    public int
        currentPlayer;
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
    public DataManager data;

    private void Start()
    {
        // randomPlayerOrder = DataManager.Instance.RandomPlayerOrder;
        randomPlayerOrder = false;
        // creates a player array that's the length of the number of players
  
        totalPlayers = DataManager.Instance.PlayerList.Count;
        isOut = new bool[totalPlayers];
        // only randomize players if that option is on
        if (randomPlayerOrder)
        {
            // ShuffleArray(players);
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
        if (!playerWin)
        {
            // increment the index up one
            nextIndex = currentIndex + 1;
            // if the next index is past the array length, loop it back to zero
            if (nextIndex >= totalPlayers)
            {
                // maybe put the shuffle in here so that it randomizes constantly?
                // only real downside is that it could have people go more than once in a row
                // but hey that might actually be fun!
                nextIndex = 0;
            }
            // set the current index from the next index var
            currentIndex = nextIndex;
            // once the indexer runs through, start the timer again
            timer = turnTime;
        }
    }

    public static void ShuffleArray<T>(T[] arr)
    {
        for (int i = arr.Length - 1; i > 0; i--)
        {
            int r = Random.Range(0, i);
            T tmp = arr[i];
            arr[i] = arr[r];
            arr[r] = tmp;
        }
    }

    public void RemovePlayer()
    {
        if (totalPlayers > 1)
        {
            for (int i = 0; i < DataManager.Instance.PlayerList.Count; i++)
            {
                if (i == currentIndex)
                {
                    isOut[i] = true;
                    totalPlayers--;
                    Debug.Log("Removed player " + i + 1);
                    Debug.Log("Total players remaining: " + totalPlayers);
                }
            }
        }
        if (totalPlayers == 1)
        {
            playerWin = true;
        }

    }

}
