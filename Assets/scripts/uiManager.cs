using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using InControl;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class uiManager : MonoBehaviour
{

    private int selectedCar;
    private int gameMode;
    private int selectedTrack;
    private int menuIndex;

    public GameObject[] containers;

    // void Update() {
    // 	print(GetComponent<EventSystem>().currentSelectedGameObject);
    // }

    void Start()
    {
        menuIndex = GetCurrentMenuIndex();
        //doing this to get rid of dumb warning, can delete later
        int hey = selectedCar + gameMode + selectedTrack;
        hey += 1;
    }

    private void Update() {
        InputDevice inputDevice = InputManager.ActiveDevice;

        if (inputDevice.Action2.WasPressed && menuIndex > 0) {
            int backIndex = menuIndex - 1;
            if (backIndex == 2 && gameMode == 1) {backIndex--;}
            else if (backIndex == 4) {backIndex = 0;}
            if (menuIndex == 2) {
                if (DataManager.Instance.PlayerList.Count <= 1) {
                    CanvasDisplay(backIndex);
                }
            } else {
                CanvasDisplay(backIndex);                
            }
        }
        else if (inputDevice.Command.WasPressed && menuIndex == 2) {
            // If the start button is pressed in the player select screen
            // go to the next menu!
            CanvasDisplay(3);
        }
    }

    private int GetCurrentMenuIndex() {
        for (int i = 0; i < containers.Length; i++) {
            if (containers[i].activeSelf == true) {
                print("Current menu index: " + i);
                return i;
            }
        }
        containers[0].SetActive(true);
        return 0;
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
        print("Current menu index: " + menuIndex);
    }

    public void SetGameMode(int mode)
    {
        gameMode = mode;
        if (mode == 0)
        {
            print("selected normal mode!");
        }
        else if (mode == 1)
        {
            print("selected potato mode!");
        }
        else {
            print("<color=red>error!</color> a nonexistent game mode was selected");
        }
    }

    public void SelectTrack(int track)
    {
        selectedTrack = track;
        switch (track)
        {
            case 1:
                print("Selected Easy");
                break;
            case 2:
                print("Selected Med");
                break;
            case 3:
                print("Selected Hard");
                break;
            default:
                print("Dunno what happened there, but something went wrong.");
                break;
        }
    }

    void RotateCarModel()
    {

    }

    public void SetCar(int car)
    {
        selectedCar = car;
        switch (car)
        {
            case 1:
                print("Selected Gremlin");
                break;
            case 2:
                print("Selected Banana");
                break;
            case 3:
                print("Selected Big Wheel");
                break;
            case 4:
                print("Selected Verminator");
                break;
            case 5:
                print("Selected AE86");
                break;
            default:
                print("Dunno what happened there, but something went wrong.");
                break;
        }
    }

    public void RandCar()
    {
        int randVal = Random.Range(1, 6);
        SetCar(randVal);
    }

}
