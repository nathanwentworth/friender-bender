using UnityEngine;
using InControl;
using System.Collections.Generic;

public class PlayerManager : MonoBehaviour
{

    const int maxPlayers = 4;
    List<PlayerData> PlayerList = new List<PlayerData>(maxPlayers);
    public uiManager manager;

    void OnEnable()
    {
        PlayerList = DataManager.PlayerList;
        manager.DisplayPlayerControllers();
        Debug.Log(DataManager.TotalPlayers);
    }

    void Update()
    {
        InputDevice inputDevice = InputManager.ActiveDevice;

        if (JoinButtonWasPressedOnDevice(inputDevice) && PlayerUsingDevice(inputDevice) == null && ListIsntFull())
        {
            PlayerData player = new PlayerData();
            player.Controller = inputDevice;
            PlayerList.Add(player);
            UpdateDataManager();
            manager.DisplayPlayerControllers();
            Debug.Log(DataManager.TotalPlayers);
        }

        if (LeaveButtonWasPressedOnDevice(inputDevice) && PlayerUsingDevice(inputDevice) != null && DataManager.TotalPlayers > 0)
        {
            PlayerList.Remove(PlayerUsingDevice(inputDevice));
            UpdateDataManager();
            manager.DisplayPlayerControllers();
            Debug.Log(DataManager.TotalPlayers);
        }
    }

    PlayerData PlayerUsingDevice(InputDevice controller)
    {
        foreach (PlayerData item in PlayerList)
        {
            if(controller == item.Controller)
            {
                return item;
            }
        }
        return null;
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

    //bool ThereIsNoPlayerUsingDevice(InputDevice inputDevice)
    //{
    //    foreach (InputDevice player in PlayerList)
    //    {
    //        if (player == inputDevice)
    //        {
    //            return false;
    //        }
    //    }
    //    return true;
    //}

    private void UpdateDataManager()
    {
        DataManager.PlayerList = PlayerList;
        DataManager.TotalPlayers = PlayerList.Count;
    }
}