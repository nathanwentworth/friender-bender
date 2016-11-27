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
    public Text nextPlayerText;

    [Header("Pause")]

    public GameObject pauseCanvas;

    [Header("Notification")]

    public Text notificationText;

    [Header("Overlay")]

    [SerializeField]
    private GameObject overlayPanel;
    public Text overlayText;
    [SerializeField]
    private GameObject gameOverPanel;

    [Header("References")]

    public PlayerSwitching playerSwitch;

    private float maxSpeed = 175;
    private int[] players;
    private int currentIndex;
    private bool deviceDetatched;
    
    public bool Paused { get; set; }
    public float notifTimer { private get; set; }
    public float overlayTimer { private get; set; }

    Queue<IEnumerator> notifications = new Queue<IEnumerator>();

    private void Start()
    {
        InputManager.OnDeviceDetached += OnDeviceDetached;
        InputManager.OnDeviceAttached += OnDeviceReattached;
        for (int i = 0; i < livesDisplay.Length; i++)
        {
            livesDisplay[i] = GameObject.Find("heart-" + i.ToString()).GetComponent<Image>();
        }
        currentPlayerText = GameObject.Find("player").GetComponent<Text>();
        notificationText.text = "";
        overlayText.text = "";
        StartCoroutine(Process());
        UpdateLivesDisplay();
        PowerupSizeSet();
        Paused = false;
    }

    private void OnDeviceDetached(InputDevice controller)
    {
        deviceDetatched = true;
        PlayerData player = PlayerUsingDevice(controller);
        Debug.Log(player.PlayerName + " disconnected.");
        string disconnectMessage = DataManager.GetPlayerIdentifier(player.PlayerNumber -1) + " DISCONNECTED";
        StartCoroutine(DisplayOverlayText(disconnectMessage));
        player.deviceDetatched = deviceDetatched;
        Time.timeScale = 0;
    }

    private void OnDeviceReattached(InputDevice controller)
    {
        if (deviceDetatched)
        {
            foreach (PlayerData item in DataManager.PlayerList)
            {
                if (item.deviceDetatched && DataManager.PlayerList.Count == DataManager.TotalPlayers)
                {
                    item.Controller = controller;
                    deviceDetatched = false;
                    item.deviceDetatched = deviceDetatched;
                    Time.timeScale = 1;
                    Debug.Log(item.PlayerName + " reconnected.");
                    StartCoroutine(DisplayOverlayText(""));
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
        if (Input.GetKeyDown(KeyCode.F)) {
            StartCoroutine(DisplayOverlayText("cool"));
        }
        if (Input.GetKeyDown(KeyCode.G)) {
            StartCoroutine(DisplayOverlayText("ahh!"));
        }

        InputDevice Controller = null;
        currentIndex = playerSwitch.currentIndex;
        if (playerSwitch.DEBUG_MODE) { Controller = InputManager.ActiveDevice; }
        else if (DataManager.CurrentGameMode == DataManager.GameMode.Party) { Controller = DataManager.PlayerList[currentIndex].Controller; }
        else { Controller = DataManager.PlayerList[0].Controller; }

        if (Controller.Command.WasPressed && !playerSwitch.playerWin)
        {
            Pause();
        }

        float MPH = DataManager.CurrentMPH;
        MPHDisplay.text = MPH + "";
        speedometerBar.fillAmount = (MPH / maxSpeed) * 0.75f;

        timer.text = string.Format("{0:F1}", playerSwitch.timer);
        timerBar.fillAmount = (0.5f + (0.5f * (playerSwitch.timer / playerSwitch.turnTime)));
        timerBar.color = timerGradient.Evaluate (playerSwitch.timer / playerSwitch.turnTime);

        currentPlayerText.text = DataManager.GetPlayerIdentifier(playerSwitch.currentIndex);

        nextPlayerText.text = "NEXT: " + DataManager.GetPlayerIdentifier(playerSwitch.NextPlayer());
    }

    public void Pause() {
        if (pauseCanvas.activeSelf) {
            Time.timeScale = 1;
            pauseCanvas.SetActive(false);
            Paused = false;
        } else {
            Time.timeScale = 0;
            pauseCanvas.SetActive(true);
            Paused = true;
        }
    }

    public void LoadScene(int scene) {
        SceneManager.LoadScene(scene);
    }

    public void EnqueueAction(IEnumerator notif) {
        notifications.Enqueue(notif);
    }

    public void UpdateLivesDisplay() {
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

    // public void DisplayNextPlayer(int index) {
    //     nextPlayerText.text = "NEXT: " + DataManager.GetPlayerIdentifier(index);
    // }

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
        powerupText[player - 1].text = DataManager.GetPlayerIdentifier(player - 1) + ": " + powerup;
    }

    public void PowerupBackgroundDisable(int player) {
        powerupContainers[player].GetComponent<Image>().color = new Color (0, 0, 0, 1);
    }

    private void PowerupSizeSet() {
        for (int i = DataManager.TotalPlayers - 1; i >= 0; i--) {
            powerupContainers[i].GetComponent<RectTransform>().anchoredPosition = new Vector2(8, 56 + ((DataManager.TotalPlayers - 1 - i) * 104));
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
        // float t = notifTimer + 1.5f;
        // while (t > 0) {
        //     t = t - Time.deltaTime;
        //     yield return null;
        // }
        // notificationText.text = "";
        yield return null;
    }

    public IEnumerator DisplayOverlayText(string text) {
        overlayText.text = text;
        // overlayTimer = overlayTimer + 1.5f;
        // while (overlayTimer > 0) {
        //     overlayTimer = overlayTimer - Time.deltaTime;
        //     yield return null;
        // }
        // overlayText.text = "";
        yield return null;
    }

    public IEnumerator DisplayPostGameMenu() {
        gameOverPanel.SetActive(true);
        yield return null;
    }

    public IEnumerator TimerTextPop() {
        float t = 0.5f;
        while (t > 0) {
            t = t - Time.deltaTime;
            timer.fontSize = (int)Mathf.SmoothStep(110, 155, t);
            yield return null;
        }
        yield return null;
    }
}
