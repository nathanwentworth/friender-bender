using UnityEngine;
using System.Collections;

public class CarSpawn : MonoBehaviour {

    public GameObject[] cars;
    public int debugCarIndex;

    private void Awake() {
        PlayerSwitching pSwitch = GameObject.Find("GameSystem").GetComponent<PlayerSwitching>();
        GameObject car = null;
        if (pSwitch.DEBUG_MODE)
        {
            car = Instantiate(cars[debugCarIndex]);
        }
        else {
            car = Instantiate(cars[DataManager.CarIndex]);
            // if (DataManager.CarsSelected[selectedCar] = "f") {
            //     DataManager.CarsSelected[selectedCar] = true;
            // }
        }
        Transform sp = pSwitch.spawnPoints[DataManager.RandomVal(0, pSwitch.spawnPoints.Length - 1)].transform;
        car.transform.position = sp.position;
        car.transform.rotation = sp.rotation;
    }

}
