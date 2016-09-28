using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerSwitching : MonoBehaviour {

	private int 
		currentIndex,
		nextIndex,
		totalPlayers;
    public int
        currentPlayer;
    public float
        timer;
	private float
		turnTime = 7;
	private bool
		randomPlayerOrder;
	public DataManager data;
    List<PlayerList> players = new List<PlayerList>();

    private void Start() {
		// on start, set the current player to player 1
		currentIndex = 0;
        timer = turnTime;
        RunScript();
    }

	public void RunScript() {
		// get vars from datamanager
		// turnTime = data.TurnTime;
        turnTime = 7f;
        totalPlayers = Input.GetJoystickNames().Length;
        foreach (string x in Input.GetJoystickNames())
        {
            print(x);
        }
        print("Total players: " + totalPlayers);
        // randomPlayerOrder = data.RandomPlayerOrder;
        randomPlayerOrder = false;
		// creates a player array that's the length of the number of players
        
        // create player array
        for (int i = 0; i < totalPlayers; i++) {
            players.Add(new PlayerList(i, "Player " + (i + 1)));
            print(players[i].playerNum);
		}
		// only randomize players if that option is on
		if (randomPlayerOrder) {
			// ShuffleArray(players);
		}
		// run the switcher coroutine
		StartCoroutine(SwitchTimer());
		print ("Turn Time: " + turnTime);
		print ("Total players: " + totalPlayers);
		print ("random players: " + randomPlayerOrder);
	}

	private void SwitchPlayer() {
        totalPlayers = Input.GetJoystickNames().Length;

        // increment the index up one
        nextIndex = currentIndex + 1;
		// if the next index is past the array length, loop it back to zero
		if (nextIndex >= totalPlayers) {
			// maybe put the shuffle in here so that it randomizes constantly?
			// only real downside is that it could have people go more than once in a row
			// but hey that might actually be fun!
			nextIndex = 0;
		}

        // set the current index from the next index var
        currentIndex = nextIndex;
        // delete later, debug prints
        currentPlayer = players[currentIndex].playerNum;
		print("current player: " + currentPlayer);
		
        // once the indexer runs through, start the timer again
        
        StartCoroutine(SwitchTimer());
	}

	// this timer counts down during every player's turn
	IEnumerator SwitchTimer() {
		print ("Switch timer started");
        while (timer >= 0)
        {
            timer -= Time.deltaTime;
            yield return null;
        }
        timer = turnTime;
		SwitchPlayer();
	}

	public static void ShuffleArray<T>(T[] arr) {
		for (int i = arr.Length - 1; i > 0; i--) {
    	int r = Random.Range(0, i);
    	T tmp = arr[i];
    	arr[i] = arr[r];
    	arr[r] = tmp;
    }
  }

    public void RemovePlayer(int p)
    {
        for (int i = 0; i < players.Count; i++)
        {
            if (players[i].playerNum == p)
            {
                players.RemoveAt(i);
                print("removed " + i);
            }
        }

    }

}
