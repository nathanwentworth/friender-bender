using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AutoPlayMovie : MonoBehaviour {

	MovieTexture m;

	void Start () {
		m = (MovieTexture)GetComponent<RawImage>().texture;
		m.loop = true;
		m.Play ();
	}
}