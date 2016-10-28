using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class OptionsChange : MonoBehaviour {

	public Slider turnTimeSlider;
	public Text turnTimeText;
	public Text textSaved;

  // options menu function
  // when called, sets new length of turn time, changes text display,
  // and saves value
  // saving value everytime it's changed might be too much? dunno.

	void OnEnable() {
		turnTimeSlider.value = DataManager.TurnTime;

		turnTimeSlider.onValueChanged.AddListener(TurnTimeSet);
		TurnTimeSet(turnTimeSlider.value);
	}

	public void TurnTimeSet(float val) {
		DataManager.TurnTime = turnTimeSlider.value;
		val = Mathf.Round(val * 10f) / 10f;
		DataManager.TurnTime = val;
		turnTimeText.text = val + "s";
		DataManager.Save();
	}

	void OnDisable() {
		turnTimeSlider.onValueChanged.RemoveAllListeners();
	}

}
