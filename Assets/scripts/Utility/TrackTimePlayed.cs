using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackTimePlayed : MonoBehaviour {

  private void Start() {
    if (!PlayerPrefs.HasKey("CKYiscwBXR6VhzWh")) {
      PlayerPrefs.SetFloat("CKYiscwBXR6VhzWh", DataManager.TotalTimePlayed);
      Debug.Log("Created playerprefs for total time played");
    } else {
      Debug.Log("total time played on start: " + PlayerPrefs.GetFloat("CKYiscwBXR6VhzWh"));
    }
  }

  private void Update() {
    if (PlayerPrefs.HasKey("CKYiscwBXR6VhzWh")) {
      DataManager.TotalTimePlayed += Time.unscaledDeltaTime;
      Debug.Log(DataManager.TotalTimePlayed);

      if ( DataManager.TotalTimePlayed > 7230f ) {
        DataManager.didntRefund = true;
      }
    }
  }

  private void OnDestroy() {
    PlayerPrefs.SetFloat("CKYiscwBXR6VhzWh", DataManager.TotalTimePlayed);
  }
  private void OnApplicationQuit() {
    PlayerPrefs.SetFloat("CKYiscwBXR6VhzWh", DataManager.TotalTimePlayed);
  }
}
