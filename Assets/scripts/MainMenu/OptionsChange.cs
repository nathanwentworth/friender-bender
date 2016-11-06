using UnityEngine;
using UnityEngine.UI;

public class OptionsChange : MonoBehaviour {

	public Slider turnTimeSlider;
	public Text turnTimeText;

	public Slider potatoDelaySlider;
	public Text potatoDelayText;

	public Slider partyDelaySlider;
	public Text partyDelayText;

	public Slider powerupCooldownSlider;
	public Text powerupCooldownText;

	public Text textSaved;

  // options menu function
  // when called, sets new length of turn time, changes text display,
  // and saves value when panel is closed

	// when script is enabled
	// set the slider value to be what's currently in the datamanager
	// add a listener to the slider for value changes
	void OnEnable() {
		DataManager.Load();
		turnTimeSlider.value = DataManager.TurnTime;
		potatoDelaySlider.value = DataManager.PotatoDelay;
		partyDelaySlider.value = DataManager.PartyDelay;
		powerupCooldownSlider.value = DataManager.PowerupCooldownTime;

		turnTimeSlider.onValueChanged.AddListener(delegate{ TurnTimeChange(turnTimeSlider.value, turnTimeText); });
		potatoDelaySlider.onValueChanged.AddListener(delegate{ PotatoDelayChange(potatoDelaySlider.value, potatoDelayText); });
		partyDelaySlider.onValueChanged.AddListener(delegate{ PartyDelayChange(partyDelaySlider.value, partyDelayText); });
		powerupCooldownSlider.onValueChanged.AddListener(delegate{ PowerupCooldownChange(powerupCooldownSlider.value, powerupCooldownText); });

		TurnTimeChange(turnTimeSlider.value, turnTimeText);
		PotatoDelayChange(potatoDelaySlider.value, potatoDelayText);
		PartyDelayChange(partyDelaySlider.value, partyDelayText);
		PowerupCooldownChange(powerupCooldownSlider.value, powerupCooldownText);
	}

	// when slider is changed
	// sets the global TurnTime value
	// changes the text display to reflect the exact turn time
	public void TurnTimeChange(float val, Text dispText) {
		val = Mathf.Round(val * 10f) / 10f;
		DataManager.TurnTime = val;
		dispText.text = val + "s";
		DataManager.Save();
	}

	public void PotatoDelayChange(float val, Text dispText) {
		val = Mathf.Round(val * 10f) / 10f;
		DataManager.PotatoDelay = val;
		dispText.text = val + "s";
		DataManager.Save();
	}

	public void PartyDelayChange(float val, Text dispText) {
		val = Mathf.Round(val * 10f) / 10f;
		DataManager.PartyDelay = val;
		dispText.text = val + "s";
		DataManager.Save();
	}

	public void PowerupCooldownChange(float val, Text dispText) {
		val = Mathf.Round(val * 10f) / 10f;
		DataManager.PowerupCooldownTime = val;
		dispText.text = val + "s";
		DataManager.Save();
	}

	// when the menu is navigated away from
	// saves data and removes listener
	void OnDisable() {
		DataManager.Save();
		turnTimeSlider.onValueChanged.RemoveAllListeners();
	}

}
