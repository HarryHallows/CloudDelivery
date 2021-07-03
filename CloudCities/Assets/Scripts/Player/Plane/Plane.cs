using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Plane : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Rigidbody rb;

    [Header("Health")]
    public int planeHealth;

    [Header("Physics")]
    [Tooltip("Force to push plane forwards")] public float thrust = 100f;
    [Tooltip("Pitch, Yaw, Roll")] public Vector3 turnTorque = new Vector3(90f, 25f, 45f);
    [Tooltip("Multiplier for the smount the forces (pitch, yaw, roll) move")] public float forceMultiplier = 100f, rollMultiplier;


    [Header("Inputs")] 
    [SerializeField] [Range(-1f, 1f)] private float pitch = 0f;
    [SerializeField] [Range(-1f, 1f)] private float yaw = 0f;
    [SerializeField] [Range(-1f, 1f)] private float roll = 0f;

    public float Pitch { set { pitch = Mathf.Clamp(value, -1f, 1f); } get { return pitch; } }
    public float Yaw { set { yaw = Mathf.Clamp(value, -1f, 1f); } get { return yaw; } }
    public float Roll { set { roll = Mathf.Clamp(value, -1f, 1f); } get { return roll; } }


    [Header("Autopilot controls")]
    [Tooltip("Sensitivity for autopilot flight")] public float sensitivity = 2.5f;
    [Tooltip("Angle at which airplane banks fully into target.")] public float aggressiveTurnAngle = 10f;

    //Overrides to check if the player using input to turn off assisted piloting to straighten plane
    private bool rollOverride, pitchOverride;


    [Header("FlightConditions")]
    private bool takeOff, docking, landed;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        float inputRoll = Input.GetAxis("Horizontal");
        float inputPitch = Input.GetAxis("Vertical");

        Inputs(-inputRoll, inputPitch);
    }

    private void FixedUpdate()
    {
        // Adds a relative force from the current coordinate position of the plane/object
        rb.AddRelativeForce(Vector3.forward * thrust * forceMultiplier, ForceMode.Force);

        // Uses Torque to turn the plane/object
        rb.AddRelativeTorque(new Vector3(turnTorque.x * pitch, turnTorque.y * yaw, turnTorque.z * roll) * forceMultiplier * Time.fixedDeltaTime, ForceMode.Force);
    }

    private void Inputs(float _roll, float _pitch)
    {
        //Debug.Log($"roll input value : {roll} + pitch input value is : {pitch}");
        //Debug.Log((Mathf.Abs(_roll) > .25f) + (": roll " + true));
        //Debug.Log((Mathf.Abs(_pitch) > .25f) + (": pitch " + true));

        rollOverride = false;
        pitchOverride = false;

        if (Mathf.Abs(_roll) > .25f)
        {
            roll = _roll * rollMultiplier;
            //Debug.Log("roll " + true);
            rollOverride = true;
        }
        else
        {
            roll = 0;
        }

        if (Mathf.Abs(_pitch) > .25f)
        {
            //Debug.Log("pitch " + true);
            pitch = _pitch;

            pitchOverride = true;
            rollOverride = true;
        }
        else
        {
            pitch = 0f;
        }

        yaw = 0f;

        if (Input.GetButton("Fire1"))
        {
            Debug.Log("Pew pew pew");
        }

        if (Input.GetButton("Fire2"))
        {
            Debug.Log("CameraRotate");
        }

        AutoPilot(new Vector3(transform.position.x, transform.position.y, transform.position.z), out yaw, out pitch, out roll);
    }

    private void AutoPilot(Vector3 flyingTarget, out float yaw, out float pitch, out float roll)
    {
        //Turn on autopilot and change camera from look around to look at mouse direction (to move the players perspective so they can see enemies)
        if (Input.GetButton("Fire2"))
        {
            yaw = 0;
            pitch = 0;
            roll = 0;

            //Call Camera function to change viewing style to rotate around target == plane by 180degrees roughly
            CameraRotate(gameObject, )
            return;
        }

        var localFlyTarget = transform.InverseTransformPoint(flyingTarget).normalized * sensitivity;
        var angleOffTarget = Vector3.Angle(transform.forward, flyingTarget - transform.position);


        yaw = Mathf.Clamp(localFlyTarget.x, -1f, 1f);
        pitch = -Mathf.Clamp(localFlyTarget.y, -1, 1f);


        // Roll is a little special because there are two different roll commands depending
        // on the situation. When the target is off axis, then the plane should roll into it.
        // When the target is directly in front, the plane should fly wings level.

        // An "aggressive roll" is input such that the aircraft rolls into the target so
        // that pitching up (handled above) will put the nose onto the target. This is
        // done by rolling such that the X component of the target's position is zeroed.
        var agressiveRoll = Mathf.Clamp(localFlyTarget.x, -1f, 1f);

        // A "wings level roll" is a roll commands the aircraft to fly wings level.
        // This can be done by zeroing out the Y component of the aircraft's right.
        var wingsLevelRoll = transform.right.y;

        // Blend between auto level and banking into the target.
        var wingsLevelInfluence = Mathf.InverseLerp(0f, aggressiveTurnAngle, angleOffTarget);
        roll = Mathf.Lerp(wingsLevelRoll, agressiveRoll, wingsLevelInfluence);
    }

    public void TakeDamage(int _damage)
    {
        Debug.Log("Ouch!!");
        planeHealth -= _damage;

        if (planeHealth <= 0)
        {
            Debug.Log("Dead");

            //Call lose conditions
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("CannonBall"))
        {
            //Spawn hit effect
            //Trigger camera shake anim
        }
    }

    private void OnTriggerEnter(Collider trigger)
    {
        if (trigger.gameObject.CompareTag("LandingZone") && takeOff == true)
        {
            if (Input.GetButtonDown("Interact"))
            {
                Debug.Log("Docking");

                docking = true;
            }
        }
    }

}
