using UnityEngine;
using System.Collections;

public class RotateWindmill : MonoBehaviour {

    public float speed;

    private Vector3 angle;

	void Update () {
        angle = gameObject.transform.localRotation.eulerAngles;
        gameObject.transform.localRotation = Quaternion.Euler(new Vector3(angle.x, angle.y, angle.z + (Time.deltaTime * speed)));
	}
}
