using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using InControl;

public class NameEntry : MonoBehaviour {

	private char[] letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890".ToCharArray();
	private int index;
	private char[] playerChars;
	private PlayerData player;
	private InputDevice controller;
	private bool acceptInput;
	private bool nameSaved;
	private bool textDisplayed;
	private Color32 playerColor;
	
	public Color32 defaultColor;
	public int playerNumber;
	public GameObject nameEntry;
	public PlayerManager playerManager;

	[HideInInspector]
	public string playerName;

	private void OnEnable() {
		textDisplayed = false;
		acceptInput = true;
		nameSaved = false;
		playerName = "";
		nameEntry.GetComponent<Text>().color = defaultColor;
		playerColor = DataManager.Colors[playerNumber];
	}

	private void OnDisable() {
		textDisplayed = false;
		acceptInput = false;
		playerName = "";
	}

	private void Update() {
		if (playerNumber < DataManager.PlayerList.Count) {
			controller = DataManager.PlayerList[playerNumber].Controller;
		} else {
			return;
		}

		if (controller.Action1.WasPressed) {
			if (playerName.Length < 3 && textDisplayed) { EnterChar(); }
			else if (playerName.Length < 3 && !textDisplayed) { DisplayNameEntry(); }
		}
		else if (controller.Command.WasPressed && playerName.Length > 0) {
			SaveName();
		} 
		else if (controller.Direction.Y > 0.5 || controller.Direction.Y < -0.5) {
			if (acceptInput && !nameSaved && playerName.Length < 3 && textDisplayed) {
				ChangeNameEntryChar();
				acceptInput = false;
				StartCoroutine("InputSleep");
			}
		}
		else if (controller.Action2.WasPressed) {
			DeleteChar();
		}
	}

	private void ChangeNameEntryChar() {
		if (controller.Direction.Y > 0.5 && index > 0) {
			index--;
		} else if (controller.Direction.Y < -0.5 && index < letters.Length - 1) {
			index++;
		}
		DisplayNameEntry();
	}

	private void DisplayNameEntry() {
		textDisplayed = true;
		string lastLetter = "<color=#" + "0000ffff" + ">" + letters[index] + "</color>";
		Debug.Log(playerName + lastLetter);
		nameEntry.GetComponent<Text>().text = playerName + lastLetter;
	}

	// private void HideText() {
	// 	textDisplayed = false;
	// 	playerName = "";
	// 	nameEntry.GetComponent<Text>().text = playerName;
	// 	nameEntry.GetComponent<Text>().color = defaultColor;
	// }

	private void EnterChar() {
		if (!textDisplayed) {
			textDisplayed = true;
		} else {
			// char[] tempName = new char[playerChars.Length + 1];
			// tempName[tempName.Length - 1] = letters[index];

			// for (int i = 0; i < tempName.Length; i++) {
			// 	playerChars[i] = tempName[i];
			// 	playerName = playerName + tempName[i];
			// }

			playerName = playerName + letters[index];
		}
		if (playerName.Length < 3) DisplayNameEntry();
	}

	private void DeleteChar() {
		nameSaved = false;
		playerName = playerName.Substring(0, playerName.Length - 1);
		nameEntry.GetComponent<Text>().color = defaultColor;
		DisplayNameEntry();
		if (playerName.Length == 0) {
			textDisplayed = false;
		}
	}

	private void SaveName() {
		DataManager.PlayerList[playerNumber].PlayerName = playerName;
		nameEntry.GetComponent<Text>().text = playerName;
		nameEntry.GetComponent<Text>().color = playerColor;
		Debug.Log("Name saved!");
		nameSaved = true;
	}

	private IEnumerator InputSleep() {
		yield return new WaitForSeconds(.1f);
		acceptInput = true;
	}
 


}