using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System.Collections;
using InControl;

public class uiManager : MonoBehaviour
{
    private int menuIndex;
    private AudioSource audioSource;
    private GameObject lastSelectedGameObject;
    private bool allPlayersReady;

    [Header("Loading")]
    [SerializeField]
    private GameObject canvasLoad;
    [SerializeField]
    private Image loadingBar;
    [SerializeField]
    private Text loadingText;
    [SerializeField]
    private Gradient backgroundGradientColors;
    [SerializeField]
    private Image backgroundGradient;
    [SerializeField]
    private AudioMixer mixer;

    private AsyncOperation sync;

    [Header("Controller Add Screen")]
    [SerializeField]
    private Image[] controllerIcons;
    [SerializeField]
    private Sprite controllerInactive;
    [SerializeField]
    private Sprite controllerActive;
    [SerializeField]
    private Text[] nameFields;
    [SerializeField]
    private Color32 inactiveColor;

    [Header("Mode Selection")]
    [SerializeField]
    private string[] modeDescriptions;
    [SerializeField]
    private Text modeDescriptionText;

    [Header("Car/Track Selection")]
    [SerializeField]
    private GameObject[] containers;
    [SerializeField]
    private GameObject rotatingModel;
    [SerializeField]
    private GameObject[] carModels;
    [SerializeField]
    private Sprite[] trackSprites;
    [SerializeField]
    private Image trackPreview;

    [Header("Options")]
    [SerializeField]
    private Text turnTimeDisplayText;
    [SerializeField]
    private Slider turnTimeSlider;
    [SerializeField]
    private GameObject creditsPanel;

    [Header("Sounds")]
    public AudioClip submitSound;
    public AudioClip switchSound;

    System.Random random = new System.Random();

    private void Awake()
    {
        Cursor.visible = false;
        Time.timeScale = 1;
        DataManager.Load();
        DataManager.ClearGameData();
        DataManager.LivesCount = 3;
        menuIndex = GetCurrentMenuIndex();
        if (GetComponent<AudioSource>() != null) {
            audioSource = GetComponent<AudioSource>();
        }
        lastSelectedGameObject = GetComponent<EventSystem>().currentSelectedGameObject;
        StartCoroutine(BackgroundGradient());
        allPlayersReady = false;
    }

    private void Start() {
        SetAudio();
    }

    private void SetAudio() {
        mixer.SetFloat("musicVol", DataManager.MusicVolume);
        mixer.SetFloat("sfxVol", DataManager.SfxVolume);
        mixer.SetFloat("uiVol", DataManager.UiVolume);        
    }

    private void Update()
    {
        InputDevice inputDevice = InputManager.ActiveDevice;

        CheckIfHighlightedButtonIsChanged();

        if (menuIndex == 2)
        {
            DisplayModeDescriptions();
            if (inputDevice.Action1.WasPressed && DataManager.TotalPlayers == 0) {
                PlayerData player = new PlayerData();
                player.Controller = inputDevice;
                player.PlayerNumber = DataManager.PlayerList.Count + 1;
                DataManager.PlayerList.Add(player);
                DataManager.TotalPlayers = DataManager.PlayerList.Count;
                Debug.Log("Added Device: " + inputDevice + " as Player " + player.PlayerNumber);                
            }
        }
        else if (menuIndex == 3)
        {
            if (inputDevice.Command.WasPressed && DataManager.TotalPlayers > 1) {
                // If the start button is pressed in the player select screen
                // go to the next menu!

                if (allPlayersReady) {
                    CanvasDisplay(5);
                }                

            }
            else if (inputDevice.Action1.WasPressed) {
                int n = 0;
                for (int i = 0; i < DataManager.PlayerList.Count; i++) {
                    if (DataManager.PlayerList[i].PlayerName != null) {
                        n++;
                    }
                }

                if (n >= DataManager.PlayerList.Count) {
                    allPlayersReady = true;
                }
            }
        }
        else if (menuIndex == 5) {
            SetPlayerLives();
            StartCoroutine(RotateModel(carModels));
            rotatingModel.SetActive(true);
        }
        else if (menuIndex == 6) {
            StartCoroutine(LevelPreview());
        }

        if (menuIndex != 5) {
            rotatingModel.SetActive(false);
        }

        if (menuIndex != 1 && menuIndex != 7 && inputDevice.Action2.WasPressed)
        {
            int backIndex = menuIndex - 1;
            switch (backIndex)
            {
                case -1:
                    if (creditsPanel.activeSelf) {
                        DisplayCredits();
                        backIndex = 0;
                    } else {
                        backIndex = 1;
                    }
                    break;
                case 2:
                    if (DataManager.TotalPlayers == 0)
                    {
                        break;
                    }
                    return;
                case 3:
                    backIndex--;
                    break;
                case 4:
                    if (DataManager.CurrentGameMode == DataManager.GameMode.Party)
                    {
                        backIndex--;
                        DataManager.ClearGameData();
                    }
                    break;
                default:
                    break;
            }
            CanvasDisplay(backIndex);
        }

    }

    private void SetPlayerLives()
    {
        foreach(PlayerData player in DataManager.PlayerList)
        {
            player.Lives = DataManager.LivesCount;
        }
    }

    // checks which panel is currently active, each loaded into an array
    // 0 is options, 1 is main menu, everything goes from there
    private int GetCurrentMenuIndex()
    {
        for (int i = 0; i < containers.Length; i++)
        {
            if (containers[i].activeSelf == true)
            {
                Debug.Log("Current menu index: " + i);
                return i;
            }
        }
        containers[0].SetActive(true);
        return 0;
    }

    // for the party mode screen
    // displays current number of connected controllers by swapping sprites
    public void DisplayPlayerControllers() {
      for (int i = 0; i < 4; i++) {
        if (i < DataManager.TotalPlayers) {
            if (DataManager.PlayerList[i] != null) {
                controllerIcons[i].sprite = controllerActive;
                controllerIcons[i].color = DataManager.Colors[i];
            } else {
                controllerIcons[i].sprite = controllerInactive;
                controllerIcons[i].color = inactiveColor;
                nameFields[i].text = "";
            }
        } else {
          controllerIcons[i].sprite = controllerInactive;
          controllerIcons[i].color = inactiveColor;
          nameFields[i].text = "";
        }
      }
    }

    // public function to load scenes by string name
    public void LoadScene(string levelName)
    {
        StartCoroutine(LoadingScreen(levelName));
    }

    // quits the game
    public void QuitGame()
    {
        Application.Quit();
    }

    private void CheckIfHighlightedButtonIsChanged() {
        if (lastSelectedGameObject == GetComponent<EventSystem>().currentSelectedGameObject) {
            return;
        } else if (lastSelectedGameObject == null) {
            lastSelectedGameObject = GetComponent<EventSystem>().currentSelectedGameObject;
        } else {
            StartCoroutine(PlayAudio(switchSound));
            Debug.Log("Switched UI button, played sound");
            lastSelectedGameObject = GetComponent<EventSystem>().currentSelectedGameObject;
        }
    }

    // will hide or show a canvas
    // hides the currently active one, and replaces it with whatever is being
    // selected, whether that's via a ui button press or back button
    public void CanvasDisplay(int selectedMenu)
    {
        containers[menuIndex].SetActive(false);
        containers[selectedMenu].SetActive(true);
        if (selectedMenu > menuIndex) {
            StartCoroutine(PlayAudio(submitSound));
        } 
        menuIndex = selectedMenu;
        Debug.Log("Current menu index: " + menuIndex);
    }

    public void SetGameMode(int mode)
    {
        DataManager.CurrentGameMode = (mode == 0) ? DataManager.GameMode.Party : DataManager.GameMode.HotPotato;
    }

    // for potato mode
    // sets number of players based on the button pressed
    public void SelectNumberOfPlayers(int players)
    {
        DataManager.TotalPlayers = players;
        for(int i = DataManager.PlayerList.Count; i < players; i++)
        {
            PlayerData player = new PlayerData();
            player.PlayerNumber = DataManager.PlayerList.Count + 1;
            DataManager.PlayerList.Add(player);
            DataManager.TotalPlayers = DataManager.PlayerList.Count;
        }
        CanvasDisplay(5);
    }

    private IEnumerator LevelPreview()
    {
        if (GetComponent<EventSystem>().currentSelectedGameObject == null) { yield return null; }
        string buttonName = GetComponent<EventSystem>().currentSelectedGameObject.transform.name;
        string buttonNumStr = buttonName.Substring(buttonName.Length - 1, 1);
        if(buttonNumStr == "0")
        {
            trackPreview.sprite = trackSprites[0];
        }
        else if(buttonNumStr == "1")
        {
            trackPreview.sprite = trackSprites[1];
        }
        yield return null;
    }

    // for the car/track selection screens
    // rotates a model being loaded in as a mesh from an array
    // todo: add material switching later
    private IEnumerator RotateModel(GameObject[] models)
    {
      int buttonNum;
      GameObject car;
      if (GetComponent<EventSystem>().currentSelectedGameObject == null) { yield return null; }
      string buttonName = GetComponent<EventSystem>().currentSelectedGameObject.transform.name;
      string buttonNumStr = buttonName.Substring(buttonName.Length - 1, 1);
      bool buttonNumSuccess = int.TryParse(buttonNumStr, out buttonNum);
        for (int i = 0; i < models.Length; i++) {
            models[i].SetActive(false);
        }
        if (buttonNumSuccess) {
            car = models[buttonNum];
        } else {
            car = models[models.Length - 1];
        }
        car.SetActive(true);
        yield return null;
    }

    // for the "set game mode" panel
    // displays a short description of each game mode when highlighting a button
    private void DisplayModeDescriptions() {
      int mode = -1;
      if (GetComponent<EventSystem>().currentSelectedGameObject == null) { return; }
      string buttonName = GetComponent<EventSystem>().currentSelectedGameObject.transform.name;
      mode = (buttonName == "btn-party") ? 0 : 1;
      modeDescriptionText.text = modeDescriptions[mode];
    }

    public void SetCar(int selectedCar)
    {
        DataManager.CarIndex = selectedCar;
    }

    public void RandCar()
    {
        int randVal = random.Next(0, 3);
        SetCar(randVal);
    }

    public void DisplayCredits() {
        bool active = (creditsPanel.activeSelf) ? false : true;
        creditsPanel.SetActive(active);
    }

    private IEnumerator PlayAudio(AudioClip sound) {
        audioSource.PlayOneShot(sound);
        yield return null;
    }

    private IEnumerator BackgroundGradient() {
        float timer = 0;
        while (true) {
            timer += .001f;
            if (timer >= 1) timer = 0;
            backgroundGradient.color = backgroundGradientColors.Evaluate (timer);
            yield return null;
        }
    }

    private IEnumerator LoadingScreen(string scene)
    {
        canvasLoad.SetActive(true);
        // yield return new WaitForSeconds(4.5f);
        sync = SceneManager.LoadSceneAsync(scene, LoadSceneMode.Single);
        sync.allowSceneActivation = false;
        // startAnimation = true;
        while (!sync.isDone) {
            float progress = sync.progress / 0.9f;
            loadingBar.fillAmount = progress;
            InputDevice inputDevice = InputManager.ActiveDevice;
            if (sync.progress >= 0.89f) {
                loadingText.text = "PRESS A TO CONTINUE";
                if (inputDevice.Action1.WasPressed) {
                    sync.allowSceneActivation = true;
                    yield return null;
                }
            }
            yield return null;
        }
    }

}
