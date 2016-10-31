using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using InControl;

public class CarControl : MonoBehaviour
{
    public List<AxleInfo> axleInfos; // the information about each individual axle
    public float maxMotorTorque; // maximum torque the motor can apply to wheel
    public float maxSteeringAngle; // maximum steer angle the wheel can have
    public float maxBrakingTorque; //how fast should you brake
    public float controllerDeadzone = 0.15f;

    [Header("PowerUp Variables")]
    public int speedBoostPower;

    [Header("Data References")]
    public PlayerSwitching playerSwitch;

    private Vector2 x_Input;
    private float accelerationForce = 0;
    private float brakingForce = 0;
    private Rigidbody rigid;
    private int mph;
    //Making this public for Powerup Reference
    [HideInInspector]
    public int currentIndex = 0;
    private bool invincible;
    private bool playing;
    private int trueCurrentIndex;

    [Header("Audio Bits")]
    //public GameObject AudioManagerObj;
    public AudioManager AudioManagerScript;
    public AudioSource carEngine;

    private Vector3 carOriginTrans;

    private void Start()
    {
        rigid = GetComponent<Rigidbody>();
        carOriginTrans = transform.position;
    }

    private void Update()
    {

        if (Time.timeScale == 1) { playing = true; } else { playing = false; }

        if (playing)
        {
            if (Input.GetKeyDown(KeyCode.Space) && mph < 2 && !playerSwitch.playerWin)
            {
                transform.rotation = Quaternion.identity;
                rigid.velocity = Vector3.zero;
                transform.position = carOriginTrans;
            }
            trueCurrentIndex = playerSwitch.currentIndex;
            if (DataManager.CurrentGameMode == DataManager.GameMode.Party) { currentIndex = trueCurrentIndex; }
            else { currentIndex = 0; }

            //CONTROLS
            if (!playerSwitch.playerWin)
            {
                InputDevice controller = null;
                if (playerSwitch.DEBUG_MODE) { controller = InputManager.ActiveDevice; }
                else { controller = DataManager.PlayerList[currentIndex].Controller; }
                brakingForce = controller.LeftTrigger.Value;
                accelerationForce = Mathf.Clamp(controller.RightTrigger.Value, 0.4f, 1.0f);
                x_Input = new Vector2(controller.Direction.X, controller.Direction.Y);
                //Hardcoded deadzone
                if (x_Input.magnitude < controllerDeadzone) x_Input = Vector2.zero;
                else x_Input = x_Input.normalized * ((x_Input.magnitude - controllerDeadzone) / (1 - controllerDeadzone));
            }
            else
            {
                rigid.constraints = RigidbodyConstraints.FreezeAll;
            }

        }


    }

    private void FixedUpdate()
    {
        mph = (int)((rigid.velocity.magnitude * 10) / 2.5);
        DataManager.CurrentMPH = mph;
        float motor = maxMotorTorque * (accelerationForce * 3f);
        float steering = maxSteeringAngle * x_Input.x / ((150f - (mph * 0.75f)) / 150f);

        //Changes the pitch of the engine audioSource
        carEngine.pitch = ((mph * 0.01f) - 0.3f);

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
        }
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
        if (!playerSwitch.DEBUG_MODE && !invincible)
        {
            if(mph > 41)
            {
                int trueCurrentIndex = playerSwitch.currentIndex;
                DataManager.PlayerList[trueCurrentIndex].Lives -= 1;
                Debug.Log("Player " + DataManager.PlayerList[trueCurrentIndex].PlayerNumber.ToString() + " lost a life. They have " + DataManager.PlayerList[trueCurrentIndex].Lives + " lives remaining.");
                if(DataManager.PlayerList[trueCurrentIndex].Lives <= 0)
                {
                    playerSwitch.RemovePlayer();
                }
            }
        }
    }

    IEnumerator DamageCooldown()
    {
        Debug.Log("Currently Invincible.");
        invincible = true;
        StartCoroutine("BlinkEffect");
        yield return new WaitForSeconds(1.5f);
        invincible = false;
        Debug.Log("No Longer Invincible.");
        StopCoroutine("BlinkEffect");
    }

    IEnumerator BlinkEffect()
    {
        bool isDisabled = false;
        Component[] renderers;
        renderers = GetComponentsInChildren<Renderer>();
        while (true)
        {
            if (!isDisabled)
            {
                foreach (Renderer child in renderers)
                {
                    child.enabled = false;
                    isDisabled = true;
                }
            }
            else
            {
                foreach (Renderer child in renderers)
                {
                    child.enabled = true;
                    isDisabled = false;
                }
            }
            yield return new WaitForSeconds(0.25f);
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