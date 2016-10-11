using UnityEngine;
using System.Collections;
using InControl;
using System.Collections.Generic;

public class PlayerManager : MonoBehaviour
{

    const int maxPlayers = 4;
    List<InputDevice> PlayerList = new List<InputDevice>(maxPlayers);

    void Update()
    {
        InputDevice inputDevice = InputManager.ActiveDevice;

        if (JoinButtonWasPressedOnDevice(inputDevice) && ThereIsNoPlayerUsingDevice(inputDevice))
        {
            PlayerList.Add(inputDevice);
        }

        if (LeaveButtonWasPressedOnDevice(inputDevice) && !ThereIsNoPlayerUsingDevice(inputDevice))
        {
            PlayerList.Remove(inputDevice);
        }

        DataManager.Instance.PlayerList = PlayerList;
    }

    bool JoinButtonWasPressedOnDevice(InputDevice inputDevice)
    {
        return inputDevice.Action1.WasPressed || inputDevice.Command.WasPressed;
    }

    bool LeaveButtonWasPressedOnDevice(InputDevice inputDevice)
    {
        return inputDevice.Action2.WasPressed;
    }

    bool ThereIsNoPlayerUsingDevice(InputDevice inputDevice)
    {
        foreach (InputDevice player in PlayerList)
        {
            if (player == inputDevice)
            {
                return true;
            }
        }
        return false;
    }


}