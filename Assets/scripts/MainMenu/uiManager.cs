using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System.Collections;
using InControl;

public class uiManager : MonoBehaviour
{

    private int selectedCar;
    private int gameMode = 0;
    private int menuIndex;
    private AudioSource audioSource;
    private GameObject lastSelectedGameObject;

    public GameObject canvasLoad;
    public Image loadingBar;
    public Gradient backgroundGradientColors;
    public Image backgroundGradient;

    private AsyncOperation sync;

    [Header("Controller Add Screen")]
    public Image[] controllerIcons;
    public Sprite controllerInactive;
    public Sprite controllerActive;
    public Text[] nameFields;
    public Color32 inactiveColor;

    [Header("Mode Selection")]
    public string[] modeDescriptions;
    public Text modeDescriptionText;

    [Header("Car/Track Selection")]
    public GameObject[] containers;
    public GameObject rotatingModel;
    public GameObject[] carModels;
    public GameObject[] trackModels;

    [Header("Options")]
    public Text turnTimeDisplayText;
    public Slider turnTimeSlider;
    public int numberOfLives = 3;
    public GameObject creditsPanel;

    [Header("Sounds")]
    public AudioClip submitSound;
    public AudioClip cancelSound;
    public AudioClip switchSound;

    System.Random random = new System.Random();

    private void Awake()
    {
        Time.timeScale = 1;
        DataManager.Load();
        DataManager.LivesCount = numberOfLives;
        Debug.Log(DataManager.CurrentGameMode);
        menuIndex = GetCurrentMenuIndex();
        if (GetComponent<AudioSource>() != null) {
            audioSource = GetComponent<AudioSource>();
        }
        lastSelectedGameObject = GetComponent<EventSystem>().currentSelectedGameObject;
        StartCoroutine(BackgroundGradient());
    }

    private void Update()
    {
        InputDevice inputDevice = InputManager.ActiveDevice;

        // if (inputDevice.Action1.WasPressed) {
        //     if (GetComponent<EventSystem>().currentSelectedGameObject == null || GetComponent<EventSystem>().currentSelectedGameObject.GetComponent<Button>() == null) {
        //         return;
        //     } else {
                
        //     }
        // }

        CheckIfHighlightedButtonIsChanged();

        if(menuIndex == 2 && inputDevice.Action1.WasPressed && DataManager.TotalPlayers == 0)
        {
            PlayerData player = new PlayerData();
            player.Controller = inputDevice;
            player.PlayerNumber = DataManager.PlayerList.Count + 1;
            DataManager.PlayerList.Add(player);
            DataManager.TotalPlayers = DataManager.PlayerList.Count;
            Debug.Log("Added Device: " + inputDevice + " as Player " + player.PlayerNumber);
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
                    Debug.Log("Clearing Player List and setting Total Players to 0...");
                    break;
                case 4:
                    rotatingModel.SetActive(false);
                    if (gameMode == 0)
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
        else if (menuIndex == 2) {
          DisplayModeDescriptions();
        }
        else if (menuIndex == 3)
        {
            if (inputDevice.Command.WasPressed && DataManager.TotalPlayers > 1) {
                // If the start button is pressed in the player select screen
                // go to the next menu!
                int n = 0;
                for (int i = 0; i < DataManager.PlayerList.Count; i++) {
                    if (DataManager.PlayerList[i].PlayerName != null) {
                        n++;
                    }
                }
                if (n >= DataManager.PlayerList.Count) CanvasDisplay(5);
            }
            else if (inputDevice.Action1.WasPressed) {
                // flash controller when a is pressed again!
                // do this later lol :^)
                // controllerIcons[i].gameObject.transform.x = 1;
            }
        }
        else if (menuIndex == 5) {
            SetPlayerLives();
            RotateModel(carModels);
            rotatingModel.SetActive(true);
        }
        else if (menuIndex == 6) {
          // RotateModel(trackModels);
        }
        else if (menuIndex == 7) {

        }
        if (menuIndex != 5) {
            rotatingModel.SetActive(false);
        }
    }

    // checks which panel is currently active, each loaded into an array
    // 0 is options, 1 is main menu, everything goes from there
    private void SetPlayerLives()
    {
        foreach(PlayerData player in DataManager.PlayerList)
        {
            player.Lives = numberOfLives;
        }
    }

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
                controllerIcons[i].color = DataManager.Colors[i];
            } else {
                controllerIcons[i].color = inactiveColor;
                nameFields[i].text = "";
            }
        } else {
          // controllerIcons[i].sprite = controllerInactive;
          controllerIcons[i].color = inactiveColor;
          nameFields[i].text = "";
        }
      }
    }

    // public function to load scenes by string name
    public void LoadScene(string levelName)
    {
        // SceneManager.LoadScene(levelName);
        StartCoroutine(LoadingScreen(levelName));
    }

    // quits the game!!!!!!!!
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
        } else {
            StartCoroutine(PlayAudio(cancelSound));
        }
        menuIndex = selectedMenu;
        Debug.Log("Current menu index: " + menuIndex);
    }

    public void SetGameMode(int mode)
    {
        gameMode = mode;
        if (mode == 0)
        {
            Debug.Log("Selected Party mode!");
            DataManager.CurrentGameMode = DataManager.GameMode.Party;
        }
        else if (mode == 1)
        {
            Debug.Log("Selected Hot Potato mode!");
            DataManager.CurrentGameMode = DataManager.GameMode.HotPotato;
        }
    }

    // for potato mode
    // sets number of players based on the button pressed
    public void SelectNumberOfPlayers(int players)
    {
        Debug.Log(players + " total players");
        DataManager.TotalPlayers = players;
        for(int i = DataManager.PlayerList.Count; i < players; i++)
        {
            PlayerData player = new PlayerData();
            player.PlayerNumber = DataManager.PlayerList.Count + 1;
            DataManager.PlayerList.Add(player);
            DataManager.TotalPlayers = DataManager.PlayerList.Count;
            Debug.Log("Added Device: null as Player " + player.PlayerNumber);
        }
        CanvasDisplay(5);
    }
    
    // for the car/track selection screens
    // rotates a model being loaded in as a mesh from an array
    // todo: add material switching later
    private void RotateModel(GameObject[] models)
    {
      int buttonNum;
      GameObject car;
      if (GetComponent<EventSystem>().currentSelectedGameObject == null) { return; }
      string buttonName = GetComponent<EventSystem>().currentSelectedGameObject.transform.name;
      for (int i = 0; i < models.Length; i++) {
        models[i].SetActive(false);
      }
      string buttonNumStr = buttonName.Substring(buttonName.Length - 1, 1);
      bool buttonNumSuccess = System.Int32.TryParse(buttonNumStr, out buttonNum);
      if (buttonNumSuccess) {
        car = models[buttonNum];
      } else {
        car = models[random.Next(0, models.Length)];
      }
      car.SetActive(true);
    }

    // for the "set game mode" panel
    // displays a short description of each game mode when highlighting a button
    private void DisplayModeDescriptions() {
      int mode = -1;
      if (GetComponent<EventSystem>().currentSelectedGameObject == null) { print("null"); return; }
      string buttonName = GetComponent<EventSystem>().currentSelectedGameObject.transform.name;
      mode = (buttonName == "btn-party") ? 0 : 1;
      modeDescriptionText.text = modeDescriptions[mode];
    }

    public void SetCar(int car)
    {
        selectedCar = car;
        DataManager.CarIndex = selectedCar;
        Debug.Log(selectedCar);
        switch (car)
        {
            case 0:
                Debug.Log("Selected Gremlin");
                break;
            case 1:
                Debug.Log("Selected Banana");
                break;
            case 2:
                Debug.Log("Selected Big Wheel");
                break;
            case 3:
                Debug.Log("Selected Verminator");
                break;
            case 4:
                Debug.Log("Selected AE86");
                break;
        }
    }

    public void RandCar()
    {
        int randVal = Random.Range(1, 5);
        SetCar(randVal);
    }

    public void DisplayCredits() {
        if (creditsPanel.activeSelf) {
            creditsPanel.SetActive(false);
        } else {
            creditsPanel.SetActive(true);
        }
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
        Debug.Log("asdfasdfsadf");
        canvasLoad.SetActive(true);
        yield return new WaitForSeconds(4.5f);
        Debug.Log("Loading start");
        sync = SceneManager.LoadSceneAsync(scene, LoadSceneMode.Single);
        // sync.allowSceneActivation = false;
        // startAnimation = true;
        while (!sync.isDone) {
            loadingBar.fillAmount = sync.progress;
            yield return null;
        }
    }

}
