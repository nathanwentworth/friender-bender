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
    public Mesh[] carModels;
    public Mesh[] trackModels;

    [Header("Options")]
    public Text turnTimeDisplayText;
    public Slider turnTimeSlider;
    public int numberOfLives = 3;

    [Header("Sounds")]
    public AudioClip submitSound;
    public AudioClip cancelSound;
    public AudioClip switchSound;

    System.Random random = new System.Random();

    private void Awake()
    {
        DataManager.Load();
        DataManager.LivesCount = numberOfLives;
        Debug.Log(DataManager.CurrentGameMode);
        menuIndex = GetCurrentMenuIndex();
        if (GetComponent<AudioSource>() != null) {
            audioSource = GetComponent<AudioSource>();
        }
        lastSelectedGameObject = GetComponent<EventSystem>().currentSelectedGameObject;
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

        if (menuIndex != 1 && inputDevice.Action2.WasPressed)
        {
            int backIndex = menuIndex - 1;
            switch (backIndex)
            {
                case -1:
                    backIndex = 1;
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
                    DataManager.PlayerList.Clear();
                    DataManager.TotalPlayers = 0;
                    break;
                case 4:
                    rotatingModel.SetActive(false);
                    if (gameMode == 0)
                    {
                        backIndex--;
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
                CanvasDisplay(5);     
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
        }
        else if (menuIndex == 6) {
          RotateModel(trackModels);
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
          // controllerIcons[i].sprite = controllerActive;
          controllerIcons[i].color = DataManager.Colors[i];
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
    private void RotateModel(Mesh[] models)
    {
      Mesh activeMesh;
      int buttonNum;
      if (GetComponent<EventSystem>().currentSelectedGameObject == null) { return; }
      string buttonName = GetComponent<EventSystem>().currentSelectedGameObject.transform.name;
      string buttonNumStr = buttonName.Substring(buttonName.Length - 1, 1);
      bool buttonNumSuccess = System.Int32.TryParse(buttonNumStr, out buttonNum);
      if (buttonNumSuccess) {
        activeMesh = models[buttonNum];
      } else {
        activeMesh = models[random.Next(0, 4)];
      }
      
      rotatingModel.GetComponent<MeshFilter>().sharedMesh = activeMesh;
      rotatingModel.SetActive(true);
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

    private IEnumerator PlayAudio(AudioClip sound) {
        audioSource.PlayOneShot(sound);
        yield return null;
    }

    IEnumerator LoadingScreen(string scene)
    {
        canvasLoad.SetActive(true);
        yield return new WaitForSeconds(4.5f);
        sync = SceneManager.LoadSceneAsync(scene, LoadSceneMode.Single);
        // sync.allowSceneActivation = false;
        // startAnimation = true;
        while (!sync.isDone) {
            loadingBar.fillAmount = sync.progress;
            yield return null;
        }
    }

}
