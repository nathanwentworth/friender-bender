using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CarControl : MonoBehaviour
{
    public List<AxleInfo> axleInfos; // the information about each individual axle
    public float maxMotorTorque; // maximum torque the motor can apply to wheel
    public float maxSteeringAngle; // maximum steer angle the wheel can have
    public float maxBrakingTorque; //how fast should you brake
    public int antiRollValue; //prevents car from flipping

    private float x_Input;
    private float accelerationForce = 0;
    private float brakingForce = 0;
    private Rigidbody rigid;
    private int mph;

    public DataManager data;

    private void Start()
    {
        rigid = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        brakingForce = -Input.GetAxis("Brake0");
        accelerationForce = Input.GetAxis("Accelerate0");
        x_Input = Input.GetAxis("Horizontal0");
    }

    public void ApplyLocalPositionToVisuals(WheelCollider collider)
    {
        if (collider.transform.childCount == 0)
        {
            return;
        }

        Transform visualWheel = collider.transform.GetChild(0);

        Vector3 position;
        Quaternion rotation;
        collider.GetWorldPose(out position, out rotation);

        visualWheel.transform.position = position;
        visualWheel.transform.rotation = rotation;
    }

    public void FixedUpdate()
    {
        mph = (int)((rigid.velocity.magnitude * 10) / 2.5);
        data.CurrentMPH = mph;
        // Debug.Log("MPH:" + mph);
        float motor = maxMotorTorque * accelerationForce;
        float steering = maxSteeringAngle * x_Input;

        foreach (AxleInfo axleInfo in axleInfos)
        {
            if (axleInfo.steering)
            {
                axleInfo.leftWheel.steerAngle = steering;
                axleInfo.rightWheel.steerAngle = steering;
            }
            if (axleInfo.motor)
            {
                axleInfo.leftWheel.motorTorque = motor;
                axleInfo.rightWheel.motorTorque = motor;
            }
            axleInfo.leftWheel.brakeTorque = brakingForce * maxBrakingTorque;
            axleInfo.rightWheel.brakeTorque = brakingForce * maxBrakingTorque;
            ApplyLocalPositionToVisuals(axleInfo.leftWheel);
            ApplyLocalPositionToVisuals(axleInfo.rightWheel);

            //WheelHit hit = new WheelHit();
            //float travelL = 1f;
            //float travelR = 1f;
            //bool groundedL = axleInfo.leftWheel.GetGroundHit(out hit);
            //if (groundedL) travelL = (-axleInfo.leftWheel.transform.InverseTransformPoint(hit.point).y - axleInfo.leftWheel.radius) / axleInfo.leftWheel.suspensionDistance;
            //bool groundedR = axleInfo.rightWheel.GetGroundHit(out hit);
            //if (groundedR) travelR = (-axleInfo.rightWheel.transform.InverseTransformPoint(hit.point).y - axleInfo.rightWheel.radius) / axleInfo.rightWheel.suspensionDistance;
            //float antiRollForce = (travelL - travelR) * antiRollValue;
            //if (groundedL) rigid.AddForceAtPosition(axleInfo.leftWheel.transform.up * -antiRollForce, axleInfo.leftWheel.transform.position);
            //if (groundedR) rigid.AddForceAtPosition(axleInfo.rightWheel.transform.up * -antiRollForce, axleInfo.rightWheel.transform.position);
        }
    }
}

[System.Serializable]
public class AxleInfo
{
    public WheelCollider leftWheel;
    public WheelCollider rightWheel;
    public bool motor; // is this wheel attached to motor?
    public bool steering; // does this wheel apply steer angle?
}