using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using InControl;

public class NameEntry : MonoBehaviour {

	private readonly char[] letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890".ToCharArray();
	
	private int index;
	private char[] playerChars;
	private PlayerData player;
	private InputDevice controller;
	private bool acceptInput;
	private bool nameSaved;
	private Color32 playerColor;
	
	[SerializeField]
	private Color32 defaultColor;
	[SerializeField]
	private GameObject nameEntry;
	[SerializeField]
	private GameObject readyText;
	[SerializeField]
	private PlayerManager playerManager;
	[SerializeField]
	private int playerNumber;

	public bool TextDisplayed { get; set; }
	public string PlayerName { get; set; }

	private void OnEnable() {
		Reset();
	}

	public void Reset() {
		TextDisplayed = false;
		acceptInput = true;
		nameSaved = false;
		readyText.SetActive(false);
		PlayerName = "";
		nameEntry.GetComponent<Text>().color = defaultColor;
		playerColor = DataManager.Colors[playerNumber];
		nameEntry.GetComponent<Text>().text = PlayerName;
	}

	private void Update() {
		if (playerNumber < DataManager.PlayerList.Count) {
			controller = DataManager.PlayerList[playerNumber].Controller;
		} else {
			return; 
		}

		if (controller.Action1.WasPressed) {
			if (PlayerName.Length < 3 && TextDisplayed) { EnterChar(); }
			else if (!TextDisplayed) { DisplayNameEntry(); }
			else if (PlayerName.Length > 0) { SaveName(); }
		}
		// else if (controller.Command.WasPressed && PlayerName.Length > 0) {
			
		// } 
		else if (controller.Direction.Y > 0.5 || controller.Direction.Y < -0.5) {
			if (acceptInput && !nameSaved && PlayerName.Length < 3 && TextDisplayed) {
				ChangeNameEntryChar();
				acceptInput = false;
				StartCoroutine("InputSleep");
			}
		}
		else if (controller.Action2.WasPressed) {
			if (PlayerName.Length > 0) {
				DeleteChar();
			}
		}
	}

	private void ChangeNameEntryChar() {
		if (controller.Direction.Y > 0.5) {
			if (index > 0) {
				index--;
			} else {
				index = letters.Length - 1;
			}
		} else if (controller.Direction.Y < -0.5) {
			if (index < letters.Length - 1) {
				index++;
			} else {
				index = 0;
			}
		}
		DisplayNameEntry();
	}

	public void DisplayNameEntry() {
		if (!TextDisplayed) {
			TextDisplayed = true;
		}
		
		string lastLetter = (PlayerName.Length < 3) ? "<color=#" + ColorToHex(playerColor) + ">" + letters[index] + "</color>" : "";
		nameEntry.GetComponent<Text>().text = PlayerName + lastLetter;
	}

	private void EnterChar() {
		PlayerName = PlayerName + letters[index];

		DisplayNameEntry();
	}

	private void DeleteChar() {
		nameSaved = false;
		readyText.SetActive(false);
		PlayerName = PlayerName.Substring(0, PlayerName.Length - 1);
		nameEntry.GetComponent<Text>().color = defaultColor;
		DisplayNameEntry();
	}

	private void SaveName() {
		DataManager.PlayerList[playerNumber].PlayerName = PlayerName;
		nameEntry.GetComponent<Text>().text = PlayerName;
		nameEntry.GetComponent<Text>().color = playerColor;
		Debug.Log("Name saved!");
		readyText.SetActive(true);
		nameSaved = true;
	}

	private IEnumerator InputSleep() {
		yield return new WaitForSeconds(.1f);
		acceptInput = true;
	}
	private string ColorToHex(Color32 color) {
		string hex = color.r.ToString("X2") + color.g.ToString("X2") + color.b.ToString("X2");
		return hex;
	}
}
