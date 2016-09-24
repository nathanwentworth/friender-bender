using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName="DataManager", menuName="", order = 1)]
public class DataManager : ScriptableObject {

	private int
		currentIndex,
		totalPlayers;

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
