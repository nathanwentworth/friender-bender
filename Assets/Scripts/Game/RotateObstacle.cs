using UnityEngine;
using System.Collections;

public class RotateObstacle : MonoBehaviour {

    private Rigidbody rigid;

	// Use this for initialization
	void Start () {
        rigid = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
        rigid.AddForceAtPosition(Vector3.one * 10, new Vector3(transform.position.x + 1, transform.position.y, transform.position.z), ForceMode.VelocityChange);
	}
}
