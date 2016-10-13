using UnityEngine;
using InControl;
using System.Collections.Generic;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{

    const int maxPlayers = 4;
    List<InputDevice> PlayerList = new List<InputDevice>(maxPlayers);
    public Text playersText;

    void Update()
    {
        InputDevice inputDevice = InputManager.ActiveDevice;

        if (JoinButtonWasPressedOnDevice(inputDevice) && ThereIsNoPlayerUsingDevice(inputDevice))
        {
            PlayerList.Add(inputDevice);
            DisplayNumberOfPlayers();
        }

        if (LeaveButtonWasPressedOnDevice(inputDevice) && !ThereIsNoPlayerUsingDevice(inputDevice))
        {
            PlayerList.Remove(inputDevice);
            DisplayNumberOfPlayers();
        }

        DataManager.PlayerList = PlayerList;
        DataManager.TotalPlayers = PlayerList.Count;
    }

    bool JoinButtonWasPressedOnDevice(InputDevice inputDevice)
    {
        return inputDevice.Action1.WasPressed;
    }

    bool LeaveButtonWasPressedOnDevice(InputDevice inputDevice)
    {
        return inputDevice.Action2.WasPressed;
    }

    private void DisplayNumberOfPlayers() {
        playersText.text = DataManager.PlayerList.Count + "";
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


}