using UnityEngine;
using System.Collections;

public class PlayerSwitching : MonoBehaviour {

	private int 
		currentIndex,
		nextIndex,
		totalPlayers;
	private float
		turnTime;
	public int[]
		playerArr;
	private bool
		randomPlayerOrder;
	public DataManager data;

	void Start() {
		// on start, set the current player to player 1
		currentIndex = 0;
	}

	public void RunScript() {
		// get vars from datamanager
		turnTime = data.TurnTime;
		totalPlayers = data.TotalPlayers;
		randomPlayerOrder = data.RandomPlayerOrder;
		// creates a player array that's the length of the number of players
		playerArr = new int[totalPlayers];
		// create player array
		for (int i = 0; i < totalPlayers; i++) {
			playerArr[i] = i;
		}
		// only randomize players if that option is on
		if (randomPlayerOrder) {
			ShuffleArray(playerArr);
		}
		// run the switcher coroutine
		StartCoroutine(SwitchTimer());
		print ("Turn Time: " + turnTime);
		print ("Total players: " + totalPlayers);
		print ("random players: " + randomPlayerOrder);
	}

	void SwitchPlayer() {
		// increment the index up one
		nextIndex = currentIndex + 1;
		print ("Next index: " + nextIndex);
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
		print("current player: " + playerArr[currentIndex]);
		print("next player: " + playerArr[nextIndex]);
		// once the indexer runs through, start the timer again
		StartCoroutine(SwitchTimer());
	}

	// this timer counts down during every player's turn
	IEnumerator SwitchTimer() {
		print ("Switch timer started");
		yield return new WaitForSeconds(turnTime);
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

}
