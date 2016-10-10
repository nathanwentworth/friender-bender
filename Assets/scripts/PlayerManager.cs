using UnityEngine;
using System.Collections;
using InControl;
using System.Collections.Generic;

public class PlayerManager : MonoBehaviour {

	const int maxPlayers = 4;
	List<PlayerList> playerList = new List<PlayerList>(maxPlayers);
	// List<Player> players = new List<Player>( maxPlayers );

	void Start()
	{
		InputManager.OnDeviceDetached += OnDeviceDetached;
	}

	void Update()
	{
		var inputDevice = InputManager.ActiveDevice;

		if (JoinButtonWasPressedOnDevice( inputDevice ))
		{
			if (ThereIsNoPlayerUsingDevice( inputDevice ))
			{
				CreatePlayer( inputDevice );
			}
		}
	}



	bool JoinButtonWasPressedOnDevice( InputDevice inputDevice )
	{
		return inputDevice.Action1.WasPressed || inputDevice.Action2.WasPressed || inputDevice.Action3.WasPressed || inputDevice.Action4.WasPressed;
	}

	PlayerList FindPlayerUsingDevice( InputDevice inputDevice )
	{
		var playerCount = playerList.Count;
		for (var i = 0; i < playerCount; i++)
		{
			var player = playerList[i];
			if (player.Device == inputDevice)
			{
				return player;
			}
		}

		return null;
	}

	PlayerList CreatePlayer( InputDevice inputDevice ) {
		if (playerList.Count < maxPlayers)
		{

			var player = gameObject.GetComponent<Player>();
			player.Device = inputDevice;
			playerList.Add( player );

			return player;
		}

		return null;
	}

	void RemovePlayer( PlayerList player ) {
		playerList.Remove( player );
		player.Device = null;
		Destroy( player.gameObject );
	}



	bool ThereIsNoPlayerUsingDevice( InputDevice inputDevice ) {
		return FindPlayerUsingDevice( inputDevice ) == null;
	}

	void OnDeviceDetached( InputDevice inputDevice ) {
		var player = FindPlayerUsingDevice( inputDevice );
		if (player != null)
		{
			RemovePlayer( player );
		}
	}



}