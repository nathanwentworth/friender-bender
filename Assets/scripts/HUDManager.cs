using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour {

	public Text MPHDisplay;
	public Image speedometerBar;
	
	public GameObject[] turnDisplayText;

	private float speedometerBarFillAmount;
	private float maxSpeed = 150;
	private int[] players;
	private int numberOfPlayers;

	public DataManager data;

	void Start() {
		numberOfPlayers = data.TotalPlayers;
		for	(int i = 0; i < numberOfPlayers; i++) {
			turnDisplayText[i].SetActive(true);
		}

		
	}
	
	void Update () {
		float MPH = data.CurrentMPH;
		MPHDisplay.text = MPH + "";
		speedometerBarFillAmount = (MPH / maxSpeed) * 0.75f;
		speedometerBar.fillAmount = speedometerBarFillAmount;

		// for	(int i = 0; i < numberOfPlayers; i++) {
		// 	turnDisplayText[i].GetComponent<Text>().text = data.PlayerArr[i] + "";
		// }
	}
}
