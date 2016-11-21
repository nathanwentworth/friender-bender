using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class OptionsChange : MonoBehaviour {

	[SerializeField]
	private Slider turnTimeSlider;
	[SerializeField]
	private Text turnTimeText;

	[SerializeField]
	private Slider potatoDelaySlider;
	[SerializeField]
	private Text potatoDelayText;

	[SerializeField]
	private Slider partyDelaySlider;
	[SerializeField]
	private Text partyDelayText;

	[SerializeField]
	private Slider powerupCooldownSlider;
	[SerializeField]
	private Text powerupCooldownText;

	[SerializeField]
	private Slider musicVolumeSlider;

	[SerializeField]
	private Slider sfxVolumeSlider;

	[SerializeField]
	private Toggle uiVolToggle;

	[SerializeField]
	private Toggle trekkieTraxToggle;

	[SerializeField]
	private Toggle fullscreenToggle;

	[SerializeField]
	private Dropdown resolutionDropdown;

	[SerializeField]
	private AudioMixer mixer;

  // options menu function
  // when called, sets new length of turn time, changes text display,
  // and saves value when panel is closed

	// when script is enabled
	// set the slider value to be what's currently in the datamanager
	// add a listener to the slider for value changes
	void OnEnable() {
		DataManager.Load();

		resolutionDropdown.options.Clear();

		for (int i = 0; i < Screen.resolutions.Length; i++) {
      resolutionDropdown.options.Add(new Dropdown.OptionData(Screen.resolutions[i].ToString()));
    }

    resolutionDropdown.RefreshShownValue();
		
		turnTimeSlider.value = DataManager.TurnTime;
		potatoDelaySlider.value = DataManager.PotatoDelay;
		partyDelaySlider.value = DataManager.PartyDelay;
		powerupCooldownSlider.value = DataManager.PowerupCooldownTime;
		musicVolumeSlider.value = DataManager.DecibelToLinear(DataManager.MusicVolume);
		sfxVolumeSlider.value = DataManager.DecibelToLinear(DataManager.SfxVolume);
		uiVolToggle.isOn = (DataManager.UiVolume == 0) ? true : false;
		trekkieTraxToggle.isOn = DataManager.IsTrekkieTraxOn;
		fullscreenToggle.isOn = DataManager.IsFullscreenOn;
		resolutionDropdown.value = DataManager.ScreenResolution;


		turnTimeSlider.onValueChanged.AddListener(delegate{
			TurnTimeChange(turnTimeSlider.value, turnTimeText); 
		});
		potatoDelaySlider.onValueChanged.AddListener(delegate{
			PotatoDelayChange(potatoDelaySlider.value, potatoDelayText); 
		});
		partyDelaySlider.onValueChanged.AddListener(delegate{
			PartyDelayChange(partyDelaySlider.value, partyDelayText);
		});
		powerupCooldownSlider.onValueChanged.AddListener(delegate{
			PowerupCooldownChange(powerupCooldownSlider.value, powerupCooldownText); 
		});
		musicVolumeSlider.onValueChanged.AddListener(delegate{
			MusicVolumeChange(musicVolumeSlider.value); 
		});
		sfxVolumeSlider.onValueChanged.AddListener(delegate{
			SfxVolumeChange(sfxVolumeSlider.value); 
		});
		trekkieTraxToggle.onValueChanged.AddListener(delegate{
			TrekkieTraxToggleChange(trekkieTraxToggle.isOn); 
		});
		uiVolToggle.onValueChanged.AddListener(delegate{
			UiVolumeChange(uiVolToggle.isOn); 
		});
		fullscreenToggle.onValueChanged.AddListener(delegate{
			FullscreenToggleChange(fullscreenToggle.isOn); 
		});
		resolutionDropdown.onValueChanged.AddListener(delegate{
			ScreenResolutionChange(resolutionDropdown.value); 
		});

		TurnTimeChange(turnTimeSlider.value, turnTimeText);
		PotatoDelayChange(potatoDelaySlider.value, potatoDelayText);
		PartyDelayChange(partyDelaySlider.value, partyDelayText);
		PowerupCooldownChange(powerupCooldownSlider.value, powerupCooldownText);
		MusicVolumeChange(musicVolumeSlider.value);
		SfxVolumeChange(sfxVolumeSlider.value);
		UiVolumeChange(uiVolToggle.isOn);
		TrekkieTraxToggleChange(trekkieTraxToggle.isOn);
		FullscreenToggleChange(fullscreenToggle.isOn);
		ScreenResolutionChange(resolutionDropdown.value);
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

	public void MusicVolumeChange(float val) {
		val = DataManager.LinearToDecibel(val);
		mixer.SetFloat("musicVol", val);
		DataManager.MusicVolume = val;
		DataManager.Save();
	}

	public void SfxVolumeChange(float val) {
		val = DataManager.LinearToDecibel(val);
		mixer.SetFloat("sfxVol", val);
		DataManager.SfxVolume = val;
		DataManager.Save();
	}

	public void UiVolumeChange(bool check) {
		float val = (check) ? 0f : -80f;
		mixer.SetFloat("uiVol", val);
		DataManager.UiVolume = val;
		DataManager.Save();
	}

	public void TrekkieTraxToggleChange (bool check) {
		DataManager.IsTrekkieTraxOn = check;
	}

	public void FullscreenToggleChange (bool check) {
		DataManager.IsFullscreenOn = check;
		Screen.fullScreen = DataManager.IsFullscreenOn;
	}

	public void ScreenResolutionChange(int val) {
		DataManager.ScreenResolution = val;
		Screen.SetResolution(
			Screen.resolutions[resolutionDropdown.value].width,
			Screen.resolutions[resolutionDropdown.value].height, 
			DataManager.IsFullscreenOn, 
			Screen.resolutions[resolutionDropdown.value].refreshRate
		);
	}

	public void ResetSettings() {
		PlayerPrefs.DeleteAll();

		DataManager.Load();

		turnTimeSlider.value = DataManager.TurnTime;
		potatoDelaySlider.value = DataManager.PotatoDelay;
		partyDelaySlider.value = DataManager.PartyDelay;
		powerupCooldownSlider.value = DataManager.PowerupCooldownTime;
		trekkieTraxToggle.isOn = DataManager.IsTrekkieTraxOn;
	}

	// when the menu is navigated away from
	// saves data and removes listener
	private void OnDisable() {
		DataManager.Save();
		turnTimeSlider.onValueChanged.RemoveAllListeners();
	}

}
