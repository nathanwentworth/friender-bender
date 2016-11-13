using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TextColorGradient : MonoBehaviour {

	[SerializeField]
	private Gradient gradient;

	private void OnEnable() {
		StartCoroutine("ColorCycle");
	}

	private IEnumerator ColorCycle() {
    float timer = 0;
    while (true) {
      timer += .01f;
      if (timer >= 1) timer = 0;
      GetComponent<Text>().color = gradient.Evaluate(timer);
      yield return null;
    }
	}

}