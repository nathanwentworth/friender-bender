using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class UIControlSet : MonoBehaviour {

	void Awake () {
    #if UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX
			GetComponent<StandaloneInputModule>().horizontalAxis = "Horizontal0_mac";
			GetComponent<StandaloneInputModule>().verticalAxis = "Vertical0_mac";
			GetComponent<StandaloneInputModule>().cancelButton = "Cancel_mac";
			GetComponent<StandaloneInputModule>().submitButton = "Submit_mac";
  	#endif
    #if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
			GetComponent<StandaloneInputModule>().horizontalAxis = "Horizontal0_win";
			GetComponent<StandaloneInputModule>().verticalAxis = "Vertical0_win";
			GetComponent<StandaloneInputModule>().cancelButton = "Cancel_win";
			GetComponent<StandaloneInputModule>().submitButton = "Submit_win";
  	#endif

	}

}
