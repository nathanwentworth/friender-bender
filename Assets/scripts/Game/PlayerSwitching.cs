using UnityEngine;
using System.Collections;

public class PlayerSwitching : MonoBehaviour
{

    private int
        totalPlayers,
        remainingPlayers;
    public int
        currentIndex = 0;
    private float
        passingControllerTime;
    [HideInInspector]
    public float
        timer;
    public float
        passTime = 3,
        turnTime = 7;
    private bool
        skipTurn,
        passingController;
    public bool
        playerWin,
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
        if (DataManager.CurrentGameMode == DataManager.GameMode.HotPotato)
        {
            string notifText1 = "PLAYER " + (currentIndex + 1) + " IS UP";
            StartCoroutine(Notifications(notifText1, ""));
            passingControllerTime = System.DateTime.Now.Second;
            Time.timeScale = 0;
            passingController = true;
        }
        timer = turnTime;
    }

    public int NextPlayer()
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
            string notifText1 = "PLAYER " + (currentIndex + 1) + " ELIMINATED!";
            string notifText2 = "Players left: " + remainingPlayers;
            StartCoroutine(Notifications(notifText1, notifText2));
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


    private IEnumerator Notifications(string notifText1, string notifText2) {
        StartCoroutine(hudManager.DisplayNotificationText(notifText1));
        yield return new WaitForSeconds(2);
        StartCoroutine(hudManager.DisplayNotificationText(notifText2));
    }

}
