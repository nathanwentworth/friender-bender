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
	public bool textDisplayed;
	private Color32 playerColor;
	
	public Color32 defaultColor;
	public int playerNumber;
	public GameObject nameEntry;
	public GameObject readyText;
	public PlayerManager playerManager;

	[HideInInspector]
	public string playerName;

	private void OnEnable() {
		textDisplayed = false;
		acceptInput = true;
		nameSaved = false;
		readyText.SetActive(false);
		playerName = "";
		nameEntry.GetComponent<Text>().color = defaultColor;
		playerColor = DataManager.Colors[playerNumber];
		nameEntry.GetComponent<Text>().text = playerName;
	}

	private void Update() {
		if (playerNumber < DataManager.PlayerList.Count) {
			controller = DataManager.PlayerList[playerNumber].Controller;
		} else {
			return;
		}

		if (controller.Action1.WasPressed) {
			Debug.Log("textDisplayed " + textDisplayed);
			if (playerName.Length < 3 && textDisplayed) { EnterChar(); }
			else if (!textDisplayed) { DisplayNameEntry(); }
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
			readyText.SetActive(false);
			if (playerName.Length == 0) {
				textDisplayed = false;
			} else {
				DeleteChar();				
			}
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
		if (!textDisplayed) {
			textDisplayed = true;
		}
		string lastLetter = "<color=#" + "0000ffff" + ">" + letters[index] + "</color>";
		Debug.Log(playerName + lastLetter);
		if (playerName.Length < 3) {
			Debug.Log(playerName.Length);
			nameEntry.GetComponent<Text>().text = playerName + lastLetter;
		} else {
			Debug.Log(playerName.Length);
			nameEntry.GetComponent<Text>().text = playerName;
		}
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
			playerName = playerName + letters[index];
		}
		DisplayNameEntry();
	}

	private void DeleteChar() {
		nameSaved = false;
		playerName = playerName.Substring(0, playerName.Length - 1);
		nameEntry.GetComponent<Text>().color = defaultColor;
		DisplayNameEntry();
	}

	private void SaveName() {
		DataManager.PlayerList[playerNumber].PlayerName = playerName;
		nameEntry.GetComponent<Text>().text = playerName;
		nameEntry.GetComponent<Text>().color = playerColor;
		Debug.Log("Name saved!");
		readyText.SetActive(true);
		nameSaved = true;
	}

	private IEnumerator InputSleep() {
		yield return new WaitForSeconds(.1f);
		acceptInput = true;
	}
 }