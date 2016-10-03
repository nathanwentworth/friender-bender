using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HUDManager : MonoBehaviour {

	public Image speedometerBar;
	public GameObject pauseCanvas;
	public Text MPHDisplay;
  public Text timer;
  public Text currentPlayerText;
	public GameObject[] turnDisplayText;

	private float speedometerBarFillAmount;
	private float maxSpeed = 150;
	private int[] players;
	private int numberOfPlayers;

	public DataManager data;
  public PlayerSwitching pSwitch;

	void Start() {
		numberOfPlayers = DataManager.Instance.TotalPlayers;
		print("Number of players: " + numberOfPlayers);
		for	(int i = 0; i < numberOfPlayers; i++) {
			turnDisplayText[i].SetActive(true);
		}		
	}
	
	void Update () {
    #if UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX
	    if (Input.GetButtonUp("Pause_mac")) {
    		if (pauseCanvas.activeSelf) {
    			Time.timeScale = 1f;
    			pauseCanvas.SetActive(false);
    		} else {
    			Time.timeScale = 0f;
		    	pauseCanvas.SetActive(true);
    		}
			}
  	#endif
    #if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
			if (Input.GetButtonUp("Pause_mac")) {
				if (pauseCanvas.activeSelf) {
					Time.timeScale(0f);
					pauseCanvas.SetActive(false);
				} else {
					Time.timeScale(1f);
		    	pauseCanvas.SetActive(true);
				}
			}
  	#endif

		float MPH = DataManager.Instance.CurrentMPH;
		MPHDisplay.text = MPH + "";
		speedometerBarFillAmount = (MPH / maxSpeed) * 0.75f;
		speedometerBar.fillAmount = speedometerBarFillAmount;

    timer.text = string.Format("{0:F1}", pSwitch.timer);
    currentPlayerText.text = (pSwitch.currentPlayer + 1).ToString();

		// for	(int i = 0; i < numberOfPlayers; i++) {
		// 	turnDisplayText[i].GetComponent<Text>().text = DataManager.Instance.PlayerArr[i] + "";
		// }
	}
}
