using UnityEngine;
using System.Collections;

public class playerSwitching : MonoBehaviour {

	private int 
		currentIndex,
		nextIndex,
		totalPlayers;
	private float
		waitTime;
	public int[]
		playerArr;
	private bool
		randomPlayerOrder;

	void Start() {
		// on start, set the current player to player 1
		currentIndex = 0;
		// grab total number of players from the game manager when that's made
		playerArr = new int[4];
		// delete these later! just for testing
		// these should mostly be publically editable in the ui, all stored in the gamemanager
		randomPlayerOrder = true;
		totalPlayers = playerArr.Length;
		waitTime = 5.0f;

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
	}

	void SwitchPlayer() {
		// increment the next player so that it's one above the current player
		nextIndex = currentIndex + 1;
		print ("Next index: " + nextIndex);
		if (nextIndex >= totalPlayers) {
			nextIndex = 0;
		}

		currentIndex = nextIndex;
		print("current player: " + playerArr[currentIndex]);
		print("next player: " + playerArr[nextIndex]);
		StartCoroutine(SwitchTimer());
	}

	// this timer counts down during every player's turn
	IEnumerator SwitchTimer() {
		print ("Switch timer started");
		yield return new WaitForSeconds(waitTime);
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
