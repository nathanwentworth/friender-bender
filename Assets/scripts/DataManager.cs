using UnityEngine;
using System.Collections;

public class DataManager : MonoBehaviour {

	private static DataManager instance;
  public static DataManager Instance { get; private set; }

  private void Awake() {
    if (Instance != null && Instance != this) {
      Destroy(this.gameObject);
    } else {
      Instance = this;
    }
    DontDestroyOnLoad(transform.gameObject);
  }

	private int
		currentIndex,
		totalPlayers,
		currentMPH;

	private float
		turnTime;

	private int[]
		playerArr;

	private bool
		randomPlayerOrder;

	public int CurrentIndex {
		get {return currentIndex;}
		set {currentIndex = value;}
	}
	public int TotalPlayers {
		get {return totalPlayers;}
		set {totalPlayers = value;}
	}
	public int CurrentMPH {
		get {return currentMPH;}
		set {currentMPH = value;}
	}

	public float TurnTime {
		get {return turnTime;}
		set {turnTime = value;}
	}

	public int[] PlayerArr {
		get {return playerArr;}
		set {playerArr = value;}
	}

	public bool RandomPlayerOrder {
		get {return randomPlayerOrder;}
		set {randomPlayerOrder = value;}
	}

}
