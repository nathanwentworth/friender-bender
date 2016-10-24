using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PowerUps {

    public enum PowerUpTypes
    {
        SpeedBoost
    }

    public void Execute(PowerUpTypes powerup)
    {
        switch (powerup)
        {
            case PowerUpTypes.SpeedBoost:
                //SpeedBoost();
                break;
        }
    }

    public static IEnumerator SpeedBoost(Transform transform, Rigidbody rigid, int maxForce)
    {
        int i = 0;
        while (i < 20)
        {
            rigid.AddForce(transform.forward * maxForce, ForceMode.Acceleration);
            yield return new WaitForSeconds(0.01f);
            i++;
        }
    }
}
