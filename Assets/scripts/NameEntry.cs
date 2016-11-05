using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using InControl;

public class NameEntry : MonoBehaviour {

	private char[] letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890".ToCharArray();
	private int index;
	private string playerName;
	private PlayerData player;
	private InputDevice controller;
	private bool acceptInput;
	private bool textDisplayed;
	private Color32 playerColor;
	
	public Color32 defaultColor;
	public int playerNumber;
	public GameObject nameEntry;
	public PlayerManager playerManager;

	private void OnEnable() {
		textDisplayed = false;
		acceptInput = true;
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
			if (playerName.Length < 3) { EnterChar(); }
			else { SaveName(); }
		} else if (controller.Action2.WasPressed) {
			playerName = "";
			nameEntry.GetComponent<Text>().color = defaultColor;
		} else if (controller.Action4.WasPressed) {
			if (playerName.Length > 0) {
				DeleteChar();
			}
		} else if (controller.Direction.Y > 0.5 || controller.Direction.Y < -0.5) {
			if (acceptInput && playerName.Length < 3) {
				ChangeNameEntryChar();
				acceptInput = false;
				StartCoroutine("InputSleep");
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
		nameEntry.GetComponent<Text>().text = playerName + letters[index];
	}

	private void EnterChar() {
		if (!textDisplayed) {
			textDisplayed = true;
		} else {
			playerName = playerName + letters[index];				
		}
		if (playerName.Length < 3) DisplayNameEntry();
	}

	private void DeleteChar() {
		playerName = playerName.Substring(0, playerName.Length - 1);
		nameEntry.GetComponent<Text>().color = defaultColor;
		DisplayNameEntry();
	}

	private void SaveName() {
		DataManager.PlayerList[playerNumber].PlayerName = playerName;
		nameEntry.GetComponent<Text>().color = playerColor;
		Debug.Log("Name saved!");
	}

	private IEnumerator InputSleep() {
		yield return new WaitForSeconds(.1f);
		acceptInput = true;
	}
 


}