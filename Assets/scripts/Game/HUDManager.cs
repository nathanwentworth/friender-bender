using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using InControl;
using UnityEngine.SceneManagement;

public class HUDManager : MonoBehaviour
{

    [Header("HUD")]

    public Text MPHDisplay;
    public Image speedometerBar;
    public Image timerBar;
    public Text timer;
    public Gradient timerGradient;
    public Text currentPlayerText;

    [Header("Pause")]

    public GameObject pauseCanvas;

    [Header("Notification")]

    public Text notificationText;

    [Header("Overlay")]

    public GameObject overlayPanel;
    public Text overlayText;
    public GameObject gameOverPanel;

    [Header("References")]

    public PlayerSwitching playerSwitch;

    private float speedometerBarFillAmount;
    private float timerBarFillAmount;
    private float maxSpeed = 150;
    private int[] players;
    private int currentIndex;

    void Start()
    {
        notificationText.text = "";
    }

    void Update()
    {
        InputDevice Controller = null;
        currentIndex = playerSwitch.currentIndex;
        if (playerSwitch.DEBUG_MODE) { Controller = InputManager.ActiveDevice; }
        else if (DataManager.CurrentGameMode == DataManager.GameMode.Party) { Controller = DataManager.PlayerList[currentIndex].Controller; }
        else { Controller = DataManager.PlayerList[0].Controller; }

        if (Controller.Command.WasPressed)
        {
            Pause();
        }
        // if (!playerSwitch.DEBUG_MODE)
        // {
            float MPH = DataManager.CurrentMPH;
            MPHDisplay.text = MPH + "";
            speedometerBarFillAmount = (MPH / maxSpeed) * 0.75f;
            speedometerBar.fillAmount = speedometerBarFillAmount;
        // }
        timer.text = string.Format("{0:F1}", playerSwitch.timer);
        timerBarFillAmount = (0.5f + (0.5f * (playerSwitch.timer / playerSwitch.turnTime)));
        timerBar.fillAmount = timerBarFillAmount;
        timerBar.color = timerGradient.Evaluate (playerSwitch.timer / playerSwitch.turnTime);
        currentPlayerText.text = "P" + (playerSwitch.currentIndex + 1);
    }

    public void Pause() {
        if (pauseCanvas.activeSelf) {
            Time.timeScale = 1;
            pauseCanvas.SetActive(false);
        } else {
            Time.timeScale = 0;
            pauseCanvas.SetActive(true);
        }
    }

    public void LoadScene(int scene) {
        SceneManager.LoadScene(scene);
    }

    public IEnumerator DisplayNotificationText(string text) {
        notificationText.text = text;
        yield return new WaitForSeconds(2);
        notificationText.text = "";
    }

    public void DisplayOverlayText(string text) {
        overlayPanel.SetActive(true);
        overlayText.text = text;
        StartCoroutine(DisplayPostGameMenu());
    }

    IEnumerator DisplayPostGameMenu() {
        yield return new WaitForSeconds(3);
        gameOverPanel.SetActive(true);
    }



}
