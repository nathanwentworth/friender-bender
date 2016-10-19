using UnityEngine;
using InControl;
using System.Collections.Generic;

public class PlayerManager : MonoBehaviour
{

    const int maxPlayers = 4;
    List<InputDevice> PlayerList = new List<InputDevice>(maxPlayers);
    public uiManager manager;

    void OnEnable()
    {
        DisplayNumberOfPlayers();
        PlayerList = DataManager.PlayerList;
    }

    void Update()
    {
        InputDevice inputDevice = InputManager.ActiveDevice;

        if (JoinButtonWasPressedOnDevice(inputDevice) && ThereIsNoPlayerUsingDevice(inputDevice) && ListIsntFull())
        {
            PlayerList.Add(inputDevice);
            UpdateDataManager();
            manager.DisplayPlayerControllers();
        }

        if (LeaveButtonWasPressedOnDevice(inputDevice) && !ThereIsNoPlayerUsingDevice(inputDevice) && DataManager.TotalPlayers > 0)
        {
            Debug.Log("bloop");
            PlayerList.Remove(inputDevice);
            UpdateDataManager();
            manager.DisplayPlayerControllers();
        }
    }

    bool JoinButtonWasPressedOnDevice(InputDevice inputDevice)
    {
        return inputDevice.Action1.WasPressed;
    }

    bool LeaveButtonWasPressedOnDevice(InputDevice inputDevice)
    {
        return inputDevice.Action2.WasPressed;
    }

    bool ListIsntFull()
    {
        if(DataManager.TotalPlayers == maxPlayers)
        {
            return false;
        }
        return true;
    }

    bool ThereIsNoPlayerUsingDevice(InputDevice inputDevice)
    {
        foreach (InputDevice player in PlayerList)
        {
            if (player == inputDevice)
            {
                return false;
            }
        }
        return true;
    }

    private void UpdateDataManager()
    {
        DataManager.PlayerList = PlayerList;
        DataManager.TotalPlayers = PlayerList.Count;
    }
}