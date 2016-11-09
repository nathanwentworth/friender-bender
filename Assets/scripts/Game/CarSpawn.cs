using UnityEngine;
using System.Collections;

public class CarSpawn : MonoBehaviour {

    public GameObject[] cars;

	// Use this for initialization
	void Awake () {
        PlayerSwitching pSwitch = GameObject.Find("GameSystem").GetComponent<PlayerSwitching>();
        GameObject car = Instantiate(cars[DataManager.CarIndex]);
        Transform sp = pSwitch.spawnPoints[DataManager.RandomVal(0, pSwitch.spawnPoints.Length)].transform;
        car.transform.position = sp.position;
        car.transform.rotation = sp.rotation;
    }

}
