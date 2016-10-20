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
        passingControllerTime,
        passTime = 3,
        turnTime = 7;
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
                    if (System.DateTime.Now.Second - passingControllerTime >= passTime)
                    {
                        Time.timeScale = 1;
                        passingController = false;
                    }
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

}
