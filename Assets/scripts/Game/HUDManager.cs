using UnityEngine;
using UnityEngine.UI;
using InControl;

public class HUDManager : MonoBehaviour
{

    public Text MPHDisplay;
    public Image speedometerBar;
    public GameObject pauseCanvas;

    public GameObject[] turnDisplayText;
    public Text timer;
    public Text currentPlayerText;

    private float speedometerBarFillAmount;
    private float maxSpeed = 150;
    private int[] players;
    private int totalPlayers;

    public PlayerSwitching playerSwitch;

    void Start()
    {
        totalPlayers = DataManager.TotalPlayers;
        for (int i = 0; i < totalPlayers; i++)
        {
            turnDisplayText[i].SetActive(true);
        }
    }

    void Update()
    {
        InputDevice Controller = DataManager.PlayerList[playerSwitch.currentIndex];
        if (Controller.Command.WasPressed)
        {
            if (pauseCanvas.activeSelf)
            {
                pauseCanvas.SetActive(false);
            }
            else
            {
                pauseCanvas.SetActive(true);
            }
        }
        if (!playerSwitch.DEBUG_MODE)
        {
            float MPH = DataManager.CurrentMPH;
            MPHDisplay.text = MPH + "";
            speedometerBarFillAmount = (MPH / maxSpeed) * 0.75f;
            speedometerBar.fillAmount = speedometerBarFillAmount;
        }
        timer.text = string.Format("{0:F1}", playerSwitch.timer);
        currentPlayerText.text = (playerSwitch.currentIndex + 1).ToString();
    }
}
