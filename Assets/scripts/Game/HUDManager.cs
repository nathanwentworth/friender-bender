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
    private Text currentPlayerText;
    public Image[] livesDisplay = new Image[3];
    public Sprite livesDisplayInactive;
    public Sprite livesDisplayActive;
    public Text[] powerupText;
    public GameObject[] powerupContainers;

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
    private bool deviceDetatched;
    
    private bool paused;
    public bool Paused
    {
        get { return paused; }
        set { paused = value; }
    }



    Queue<IEnumerator> notifications = new Queue<IEnumerator>();


    void Start()
    {
        InputManager.OnDeviceDetached += OnDeviceDetached;
        InputManager.OnDeviceAttached += OnDeviceReattached;
        for (int i = 0; i < livesDisplay.Length; i++)
        {
            livesDisplay[i] = GameObject.Find("heart-" + i.ToString()).GetComponent<Image>();
        }
        currentPlayerText = GameObject.Find("player").GetComponent<Text>();
        notificationText.text = "";
        StartCoroutine(Process());
        UpdateLivesDisplay();
        PowerupSizeSet();
        paused = false;
    }

    private void OnDeviceDetached(InputDevice controller)
    {
        deviceDetatched = true;
        PlayerData player = PlayerUsingDevice(controller);
        Debug.Log(player.PlayerName + " disconnected.");
        player.deviceDetatched = deviceDetatched;
        Time.timeScale = 0;
    }

    private void OnDeviceReattached(InputDevice controller)
    {
        if (deviceDetatched)
        {
            foreach(PlayerData item in DataManager.PlayerList)
            {
                if (item.deviceDetatched)
                {
                    item.Controller = controller;
                    deviceDetatched = false;
                    item.deviceDetatched = deviceDetatched;
                    Time.timeScale = 1;
                    Debug.Log(item.PlayerName + " reconnected.");
                    break;
                }
            }
        }
    }

    public PlayerData PlayerUsingDevice(InputDevice controller)
    {
        foreach (PlayerData item in DataManager.PlayerList)
        {
            if (controller == item.Controller)
            {
                return item;
            }
        }
        return null;
    }

    void Update()
    {
        InputDevice Controller = null;
        currentIndex = playerSwitch.currentIndex;
        if (playerSwitch.DEBUG_MODE) { Controller = InputManager.ActiveDevice; }
        else if (DataManager.CurrentGameMode == DataManager.GameMode.Party) { Controller = DataManager.PlayerList[currentIndex].Controller; }
        else { Controller = DataManager.PlayerList[0].Controller; }

        if (Controller.Command.WasPressed && !playerSwitch.playerWin)
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

        currentPlayerText.text = DataManager.GetPlayerIdentifier(playerSwitch.currentIndex);
    }

    public void Pause() {
        if (pauseCanvas.activeSelf) {
            Time.timeScale = 1;
            pauseCanvas.SetActive(false);
            paused = false;
        } else {
            Time.timeScale = 0;
            pauseCanvas.SetActive(true);
            paused = true;
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
                livesDisplay[i].color = DataManager.Colors[trueCurrentIndex];
            } else {
                livesDisplay[i].sprite = livesDisplayInactive;
                livesDisplay[i].color = Color.white;
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
        powerupText[player - 1].text = player + ":   " + powerup;
    }

    private void PowerupSizeSet() {
        for (int i = DataManager.TotalPlayers - 1; i >= 0; i--) {
            powerupContainers[i].GetComponent<RectTransform>().anchoredPosition = new Vector2(16, 64 + ((DataManager.TotalPlayers - 1 - i) * 96));
            powerupContainers[i].SetActive(true);
        }
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

    public IEnumerator DisplayOverlayText(string text) {
        if (overlayPanel.activeSelf) {
            overlayPanel.SetActive(false);
        } else {
            overlayPanel.SetActive(true);
        }
        overlayText.text = text;
        yield return null;
    }

    public IEnumerator DisplayPostGameMenu() {
        gameOverPanel.SetActive(true);
        yield return null;
    }
}
