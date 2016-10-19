using UnityEngine;
using InControl;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class uiManager : MonoBehaviour
{

    private int selectedCar;
    private int gameMode = 0;
    private int selectedTrack;
    private int menuIndex;

    public GameObject[] containers;
    public GameObject rotatingModel;
    public Mesh[] carModels;
    public Mesh[] trackModels;
    public Image[] controllerIcons;
    public Sprite controllerInactive;
    public Sprite controllerActive;
    public string[] modeDescriptions;
    public Text modeDescriptionText;

    System.Random random = new System.Random();

    void Start()
    {
        Debug.Log(DataManager.CurrentGameMode);
        menuIndex = GetCurrentMenuIndex();
        //doing this to get rid of dumb warning, can delete later
        // FLOCKA
        int hey = selectedCar + gameMode + selectedTrack;
        hey += 1;
    }

    private void Update()
    {
        InputDevice inputDevice = InputManager.ActiveDevice;

        if(menuIndex == 2 && inputDevice.AnyButtonWasPressed)
        {
            DataManager.PlayerList.Add(inputDevice);
            DataManager.TotalPlayers = 1;
            Debug.Log("Added Device: " + inputDevice);
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
                    rotatingModel.SetActive(false);
                    break;
                case 4:
                    if(gameMode == 0)
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
        else if (menuIndex == 3 && inputDevice.Command.WasPressed && DataManager.TotalPlayers > 1)
        {
            // If the start button is pressed in the player select screen
            // go to the next menu!
            CanvasDisplay(5);
        }
        else if (menuIndex == 5) {
            RotateModel(carModels);
        }
        else if (menuIndex == 6) {
          RotateModel(trackModels);
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

    public void DisplayPlayerControllers() {
      for (int i = 0; i < 4; i++) {
        if (i < DataManager.TotalPlayers) {
          controllerIcons[i].sprite = controllerActive;
        } else {
          controllerIcons[i].sprite = controllerInactive;
        }
      }
    }

    public void LoadScene(string levelName)
    {
        SceneManager.LoadScene(levelName);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void CanvasDisplay(int selectedMenu)
    {
        containers[menuIndex].SetActive(false);
        containers[selectedMenu].SetActive(true);
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

    public void SelectTrack(int track)
    {
        selectedTrack = track;
        switch (track)
        {
            case 1:
                Debug.Log("Selected Easy");
                break;
            case 2:
                Debug.Log("Selected Med");
                break;
            case 3:
                Debug.Log("Selected Hard");
                break;
        }
    }

    public void SelectNumberOfPlayers(int players)
    {
        Debug.Log(players + " total players");
        DataManager.TotalPlayers = players;
        CanvasDisplay(5);
    }

    void RotateModel(Mesh[] models)
    {
      Mesh activeMesh;
      int buttonNum;
      string buttonName = GetComponent<UnityEngine.EventSystems.EventSystem>().currentSelectedGameObject.transform.name;
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

    private void DisplayModeDescriptions() {
      int mode = -1;
      string buttonName = GetComponent<UnityEngine.EventSystems.EventSystem>().currentSelectedGameObject.transform.name;
      if (buttonName == "btn-party") {
        mode = 0;
      } else {
        mode = 1;
      }
      modeDescriptionText.text = modeDescriptions[mode];
    }

    public void SetCar(int car)
    {
        selectedCar = car;
        switch (car)
        {
            case 1:
                Debug.Log("Selected Gremlin");
                break;
            case 2:
                Debug.Log("Selected Banana");
                break;
            case 3:
                Debug.Log("Selected Big Wheel");
                break;
            case 4:
                Debug.Log("Selected Verminator");
                break;
            case 5:
                Debug.Log("Selected AE86");
                break;
        }
    }

    public void RandCar()
    {
        int randVal = Random.Range(1, 6);
        SetCar(randVal);
    }

}
