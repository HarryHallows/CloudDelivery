using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

[RequireComponent(typeof(Rigidbody))]
public class Plane : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Rigidbody rb;
    [SerializeField] private CinemachineFreeLook cam;

    [Header("Health")]
    public int planeHealth;

    [Header("Physics")]
    [Tooltip("Force to push plane forwards")] public float thrust = 100f;
    [Tooltip("Pitch, Yaw, Roll")] public Vector3 turnTorque = new Vector3(90f, 25f, 45f);
    [Tooltip("Multiplier for the smount the forces (pitch, yaw, roll) move")] public float forceMultiplier = 100f, rollMultiplier, pitchMultiplier;

    [Tooltip("Decides how long you can be flying before landing")] public float fuel;
    [SerializeField] private float startFuel = 100f;

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
    [SerializeField] private bool takeOff, docking, landed;

    [Header("Landing")]
    [SerializeField] private float landingTime, landingCheckRange, dockingRange;
    [SerializeField] private GameObject landingTarget;
    [SerializeField] public RaycastHit landingHit, dockingHit;
    public LayerMask landingLayerMask, dockingLayerMask;

    // Start is called before the first frame update
    void Start()
    {
        //cam = Camera.main.GetComponent<CinemachineFreeLook>();
        rb = GetComponent<Rigidbody>();

        fuel = startFuel;
    }

    // Update is called once per frame
    void Update()
    {
        float inputRoll = Input.GetAxis("Horizontal");
        float inputPitch = Input.GetAxis("Vertical");

        Inputs(-inputRoll, inputPitch);
        RayChecks();
    }

    private void FixedUpdate()
    {
        Movement();
    }

    private void RayChecks()
    {
        if (Physics.Raycast(transform.position, transform.TransformDirection(-Vector3.up), out landingHit, landingCheckRange, landingLayerMask))
        {
            Debug.Log("Hitting Runway");
            landed = true;
            docking = false;
        }
        else
        {
            Debug.Log("Not Hitting runway");
            landed = false;
            takeOff = true;
        }

        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out dockingHit, dockingRange, dockingLayerMask))
        {
            Debug.Log("hitting dock box");

            if (dockingHit.collider.tag == "LandingZone")
            {
                Debug.Log("Docking Attempt");
                landingTarget = dockingHit.collider.gameObject.transform.GetChild(0).gameObject;
                docking = true;
            }
        }

    }

    private void Movement()
    {
        if (takeOff == true)
        {
            if (fuel > 0)
            {
                fuel -= Time.deltaTime / 2;

                // Adds a relative force from the current coordinate position of the plane/object
                rb.AddRelativeForce(Vector3.forward * thrust * forceMultiplier, ForceMode.Force);

                // Uses Torque to turn the plane/object
                rb.AddRelativeTorque(new Vector3(turnTorque.x * pitch, turnTorque.y * yaw, turnTorque.z * roll) * forceMultiplier * Time.fixedDeltaTime, ForceMode.Force);
            }
            else
            {
                rb.useGravity = true;
                cam.GetComponent<CinemachineFreeLook>().m_Follow = null;
            }
        }
        else
        {
            if (landed == true)
            {
                fuel = startFuel;

                if (Input.GetKey(KeyCode.LeftShift))
                {
                    thrust += Time.deltaTime * 5;

                    // Adds a relative force from the current coordinate position of the plane/object
                    rb.AddRelativeForce(Vector3.forward * thrust * forceMultiplier, ForceMode.Force);
                }   
            }
        }

        if (docking == true)
        {
            Debug.Log("Docking");
            takeOff = false;

            thrust = 0;


            Quaternion dockingRotation = Quaternion.identity;
            transform.rotation = Quaternion.Slerp(transform.rotation, dockingRotation, landingTime);
            transform.position = Vector3.Lerp(transform.position, landingTarget.transform.position, landingTime);

        }
       
    }

    private void Inputs(float _roll, float _pitch)
    {
        rollOverride = false;
        pitchOverride = false;

        if (Mathf.Abs(_roll) > .25f)
        {
            roll = _roll * rollMultiplier;
            rollOverride = true;
        }
        else
        {
            roll = 0;
        }

        if (Mathf.Abs(_pitch) > .25f)
        {
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
            AutoPilot(new Vector3(transform.position.x, transform.position.y, transform.position.z), out yaw, out pitch, out roll);
        }
        else
        {
            cam.GetComponent<CinemachineFreeLook>().m_XAxis.m_MaxSpeed = 0f;
        }

    }

    private void AutoPilot(Vector3 flyingTarget, out float yaw, out float pitch, out float roll)
    {
        //Turn on autopilot and change camera from look around to look at mouse direction (to move the players perspective so they can see enemies)
        if (Input.GetButton("Fire2"))
        {
            Debug.Log("Pilot Lock");

            yaw = 0;
            pitch = 0;
            roll = 0;

            //Call Camera function to change viewing style to rotate around target == plane by 180degrees roughly
            cam.GetComponent<CinemachineFreeLook>().m_XAxis.m_MaxSpeed = 200f;

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

    private void OnDrawGizmos()
    {
        Debug.DrawRay(transform.position, transform.TransformDirection(-Vector3.up) * landingHit.distance, Color.green);
        Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * dockingHit.distance, Color.blue);
    }
}
