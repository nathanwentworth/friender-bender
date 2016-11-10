using UnityEngine;
using InControl;
using System.Collections.Generic;

public class PlayerManager : MonoBehaviour
{

    const int maxPlayers = 4;
    List<PlayerData> PlayerList = new List<PlayerData>(maxPlayers);
    public uiManager manager;

    private void OnEnable()
    {
        PlayerList = DataManager.PlayerList;
        manager.DisplayPlayerControllers();
    }

    private void Update()
    {
        InputDevice inputDevice = InputManager.ActiveDevice;

        if (JoinButtonWasPressedOnDevice(inputDevice) && PlayerUsingDevice(inputDevice) == null && ListIsntFull())
        {
            PlayerData player = new PlayerData();
            player.Controller = inputDevice;
            player.PlayerNumber = PlayerList.Count + 1;
            PlayerList.Add(player);
            UpdateDataManager();
            manager.DisplayPlayerControllers();
            Debug.Log("Added Device: " + inputDevice + " as Player " + player.PlayerNumber);
        }

        if (LeaveButtonWasPressedOnDevice(inputDevice) && PlayerUsingDevice(inputDevice) != null && DataManager.TotalPlayers > 0)
        {
            PlayerData player = PlayerUsingDevice(inputDevice);
            PlayerList.Remove(player);
            UpdateDataManager();
            manager.DisplayPlayerControllers();
            Debug.Log("Removed Device: " + inputDevice + " who was Player " + player.PlayerNumber);
            for(int i = 0; i < DataManager.TotalPlayers; i++)
            {
                DataManager.PlayerList[i].PlayerNumber = i + 1;
            }
        }
    }

    public PlayerData PlayerUsingDevice(InputDevice controller)
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

    private bool JoinButtonWasPressedOnDevice(InputDevice inputDevice)
    {
        return inputDevice.Action1.WasPressed;
    }

    private bool LeaveButtonWasPressedOnDevice(InputDevice inputDevice)
    {
        return inputDevice.Action2.WasPressed;
    }

    private bool ListIsntFull()
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