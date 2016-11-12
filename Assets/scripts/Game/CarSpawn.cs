using UnityEngine;
using System.Collections;

public class CarSpawn : MonoBehaviour {

    public GameObject[] cars;
    //
    void Awake()
    {
        PlayerSwitching pSwitch = GameObject.Find("GameSystem").GetComponent<PlayerSwitching>();
        GameObject car = Instantiate(cars[DataManager.CarIndex]);
        Transform sp = pSwitch.spawnPoints[DataManager.RandomVal(0, pSwitch.spawnPoints.Length - 1)].transform;
        car.transform.position = sp.position;
        car.transform.rotation = sp.rotation;
    }

}
