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
    public int carHealth = 100;
    public float controllerDeadzone = 0.15f;
    //public Vector3 newCom;

    [Header("Data References")]
    public DataManager data;
    public PlayerSwitching pSwitch;

    private Vector2 x_Input;
    private float accelerationForce = 0;
    private float brakingForce = 0;
    private Rigidbody rigid;
    private int mph;
    private int currentPlayer = 0;
    private Vector3 originCom;

    private void Start()
    {
        rigid = GetComponent<Rigidbody>();
        originCom = rigid.centerOfMass;
    }

    private void Update()
    {
        //Gamestate stuff
        currentPlayer = pSwitch.currentPlayer;
        if (!pSwitch.DEBUG_MODE)
        {
            if (carHealth <= 0)
            {
                Debug.Log("KABOOM. Your car blew up! Player " + currentPlayer + " was eliminated!");
                pSwitch.RemovePlayer();
                carHealth = 100;
            }
        }

        //CONTROLS
        if (!pSwitch.playerWin)
        {
            #if UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX
                brakingForce = -Input.GetAxis("Brake" + currentPlayer + "_mac");
                accelerationForce = Mathf.Clamp(Input.GetAxis("Accelerate" + currentPlayer + "_mac"), 0.4f, 1.0f);
                x_Input = new Vector2(Input.GetAxis("Horizontal" + currentPlayer + "_mac"), Input.GetAxis("Vertical0_mac"));
            #endif
            #if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
                brakingForce = -Input.GetAxis("Brake" + currentPlayer + "_win");
                accelerationForce = Mathf.Clamp(Input.GetAxis("Accelerate" + currentPlayer + "_win"), 0.4f, 1.0f);
                x_Input = new Vector2(Input.GetAxis("Horizontal" + currentPlayer + "_win"), Input.GetAxis("Vertical0_win"));
            #endif
            // brakingForce = -Input.GetAxis("Brake" + currentPlayer);
            // print("currentPlayer: " + currentPlayer + ", accelerationForce: " + accelerationForce);
            // x_Input = new Vector2(Input.GetAxis("Horizontal" + currentPlayer), Input.GetAxis("Vertical0"));
            //Hardcoded deadzone
            if (x_Input.magnitude < controllerDeadzone) x_Input = Vector2.zero;
            else x_Input = x_Input.normalized * ((x_Input.magnitude - controllerDeadzone) / (1 - controllerDeadzone));
        }
        else
        {
            rigid.constraints = RigidbodyConstraints.FreezeAll;
        }

    }

    private void FixedUpdate()
    {
        mph = (int)((rigid.velocity.magnitude * 10) / 2.5);
        DataManager.Instance.CurrentMPH = mph;
        float motor = maxMotorTorque * (accelerationForce * 3f);
        float steering = maxSteeringAngle * x_Input.x * ((150f - (mph * 0.5f)) / 150f);
        // ((1200f - (motor * 0.75f)) / 1200f) old motor calculation

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
            //if (!axleInfo.leftWheel.isGrounded && !axleInfo.rightWheel.isGrounded)
            //{
            //    rigid.centerOfMass = newCom;
            //}
            //else
            //{
            //    rigid.centerOfMass = originCom;
            //}
            //Anti Roll Bar Code: DOESNT WORK
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
        //Clamp that speed
        // rigid.velocity = new Vector3(Mathf.Clamp(rigid.velocity.x, -30, 30), Mathf.Clamp(rigid.velocity.y, -30, 30), Mathf.Clamp(rigid.velocity.z, -30, 30));
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

    private void OnCollisionEnter(Collision other)
    {
        if (mph > 0 && mph < 50)
        {
            Debug.Log("Boop. No Damage.");
        }
        else if (mph > 51 && mph < 80)
        {
            Debug.Log("Bonk. Minor Damage");
            carHealth -= 20;
        }
        else if (mph > 81 && mph < 110)
        {
            Debug.Log("CRRCH. Medium Damage");
            carHealth -= 40;
        }
        else if (mph > 111 && mph < 130)
        {
            Debug.Log("REEEEEEEEEEEEGFEGWFHE. High Damage");
            carHealth -= 60;
        }
        else
        {
            carHealth -= 100;
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