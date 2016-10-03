using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class OptionsChange : MonoBehaviour {

	public DataManager data;
	public Slider turnTimeSlider;
	public Slider playerNumberSlider;
	public Toggle randomPlayerToggle;
	public Text playerNumberText;
	public Text turnTimeText;

	void OnEnable() {

		turnTimeSlider.value = DataManager.Instance.TurnTime;
		playerNumberSlider.value = DataManager.Instance.TotalPlayers;
		randomPlayerToggle.isOn = DataManager.Instance.RandomPlayerOrder;


		turnTimeSlider.onValueChanged.AddListener(TurnTimeSet);
		playerNumberSlider.onValueChanged.AddListener(PlayerNumberSet);
		randomPlayerToggle.onValueChanged.AddListener(SetRandomPlayerOrder);
		TurnTimeSet(turnTimeSlider.value);
		PlayerNumberSet(playerNumberSlider.value);
		SetRandomPlayerOrder(randomPlayerToggle.isOn);
	}

	public void SaveButton() {
		DataManager.Instance.Save();
	}

	public void SetRandomPlayerOrder(bool toggle) {
		if (toggle) {
			DataManager.Instance.RandomPlayerOrder = true;
		} else {
			DataManager.Instance.RandomPlayerOrder = false;
		}
	}

	public void PlayerNumberSet(float val) {
		float playerSliderVal = val;
		int roundedVal = (int)Mathf.Round(playerSliderVal);
		DataManager.Instance.TotalPlayers = roundedVal;
		playerNumberText.text = roundedVal + "";
	}

	public void TurnTimeSet(float val) {
		val = Mathf.Round(val * 10f) / 10f;
		DataManager.Instance.TurnTime = val;
		turnTimeText.text = val + "";
	}

	void OnDisable() {
		turnTimeSlider.onValueChanged.RemoveAllListeners();
		playerNumberSlider.onValueChanged.RemoveAllListeners();
	}

}
