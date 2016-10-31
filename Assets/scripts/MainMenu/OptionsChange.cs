using UnityEngine;
using UnityEngine.UI;

public class OptionsChange : MonoBehaviour {

	public Slider turnTimeSlider;
	public Text turnTimeText;
	public Text textSaved;

  // options menu function
  // when called, sets new length of turn time, changes text display,
  // and saves value when panel is closed

	// when script is enabled
	// set the slider value to be what's currently in the datamanager
	// add a listener to the slider for value changes
	void OnEnable() {
		turnTimeSlider.value = DataManager.TurnTime;

		turnTimeSlider.onValueChanged.AddListener(TurnTimeSet);
		TurnTimeSet(turnTimeSlider.value);
	}

	// when slider is changed
	// sets the global TurnTime value
	// changes the text display to reflect the exact turn time
	public void TurnTimeSet(float val) {
		DataManager.TurnTime = turnTimeSlider.value;
		val = Mathf.Round(val * 10f) / 10f;
		DataManager.TurnTime = val;
		turnTimeText.text = val + "s";
	}

	// when the menu is navigated away from
	// saves data and removes listener
	void OnDisable() {
		DataManager.Save();
		turnTimeSlider.onValueChanged.RemoveAllListeners();
	}

}
