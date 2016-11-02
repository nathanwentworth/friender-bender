using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using InControl;

public class HUDManager : MonoBehaviour
{

    [Header("HUD")]

    public Text MPHDisplay;
    public Image speedometerBar;
    public Image timerBar;
    public Text timer;
    public Gradient timerGradient;
    public Text currentPlayerText;
    public Image[] livesDisplay;
    public Sprite livesDisplayInactive;
    public Sprite livesDisplayActive;
    public Text[] powerupText;

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

    Queue<IEnumerator> notifications = new Queue<IEnumerator>();


    void Start()
    {
        notificationText.text = "";
        StartCoroutine(Process());
        UpdateLivesDisplay();
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

    public void EnqueueAction(IEnumerator notif) {
        notifications.Enqueue(notif);
    }

    public void UpdateLivesDisplay() {
        Debug.Log("updating lives display");
        int trueCurrentIndex = playerSwitch.currentIndex;
        for (int i = 0; i < DataManager.LivesCount; i++) {
            if (i < DataManager.PlayerList[trueCurrentIndex].Lives) {
                livesDisplay[i].sprite = livesDisplayActive;
            } else {
                livesDisplay[i].sprite = livesDisplayInactive;
            }
        }
    }

    private IEnumerator Process() {
        while (true) {
            if (notifications.Count > 0) {
                yield return StartCoroutine(notifications.Dequeue());
            }
            else {
                yield return null;
            }
        }
    }

    public void DisplayPowerups(int player, string powerup) {
        powerupText[player].text = player + ": " + powerup;
    }

    public void EnqueueWait(float aWaitTime) {
        notifications.Enqueue(Wait(aWaitTime));
    }

    private IEnumerator Wait(float waitTime) {
        yield return new WaitForSeconds(waitTime);
    }


    public IEnumerator DisplayNotificationText(string text) {
        notificationText.text = text;
        yield return null;
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
