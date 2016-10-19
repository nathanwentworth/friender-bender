using UnityEngine;
using UnityEngine.UI;

public class OptionsChange : MonoBehaviour {

	public Slider turnTimeSlider;
	public Toggle randomPlayerToggle;
	public Text playerNumberText;
	public Text turnTimeText;

	void OnEnable() {

		turnTimeSlider.value = DataManager.TurnTime;
		randomPlayerToggle.isOn = DataManager.RandomPlayerOrder;


		turnTimeSlider.onValueChanged.AddListener(TurnTimeSet);
		randomPlayerToggle.onValueChanged.AddListener(SetRandomPlayerOrder);
		TurnTimeSet(turnTimeSlider.value);
		SetRandomPlayerOrder(randomPlayerToggle.isOn);
	}

	public void SetRandomPlayerOrder(bool toggle) {
		if (toggle) {
			DataManager.RandomPlayerOrder = true;
		} else {
			DataManager.RandomPlayerOrder = false;
		}
	}

	public void TurnTimeSet(float val) {
		val = Mathf.Round(val * 10f) / 10f;
		DataManager.TurnTime = val;
		turnTimeText.text = val + "";
	}

	void OnDisable() {
		turnTimeSlider.onValueChanged.RemoveAllListeners();
	}

}
