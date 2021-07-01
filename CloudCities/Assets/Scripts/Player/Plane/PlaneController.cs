using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlaneController : MonoBehaviour
{

    [SerializeField] PlayerController playerCharacter;

    [SerializeField] private Camera cam;
    
    //Inputs
    [SerializeField] public float horizontal;
    [SerializeField] public float vertical;


    public float smoothTime;
    [SerializeField] private float thrust;
    [SerializeField] public float moveSpeed, minMoveSpeed, maxMoveSpeed;
    [SerializeField] private float startMoveSpeed;

    private Vector3 velocity = Vector3.zero;

    [SerializeField] private Quaternion targetVerticalRotation;
    [SerializeField] private Quaternion targetHorizontalRotation;


    private Rigidbody rb;

    public Transform planeModel;

    public bool landed, takeOff;
    public bool excellerating, decellerating;


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
     
    // Start is called before the first frame update
    void Start()
    {
        playerCharacter = FindObjectOfType<PlayerController>();
        cam.GetComponent<CameraFollow>().FollowTarget(gameObject.transform);
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("z :" + transform.rotation.z);
        PlaneInputs();
        PlaneLanding();
    }

    private void FixedUpdate()
    {
        PlaneMovement();  
        //ShootingLogic()
    }


    private void PlaneInputs()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        if (takeOff == false)
        {
            if (Input.GetKey("e"))
            {
                Debug.Log("I A M PRESSING BUTTON!!");
                landed = true;
            }

            if (Input.GetKey(KeyCode.LeftShift) && thrust < startMoveSpeed)
            {
                transform.position += transform.forward * thrust * Time.deltaTime;
                thrust += Time.deltaTime * 10;
            }

            if (thrust > startMoveSpeed)
            {
                takeOff = true;
                moveSpeed = startMoveSpeed;
            }
        }
        

        if (takeOff == true)
        {
            if (horizontal != 0 || vertical != 0)
            {
                transform.Rotate((vertical), 0f, -horizontal * 2);
            }

            if (Input.GetKey(KeyCode.LeftControl))
            {
                if (decellerating == true)
                {
                    if (moveSpeed < minMoveSpeed)
                    {
                        decellerating = false;
                        moveSpeed -= Time.deltaTime;
                    }
                }
                else
                {
                    decellerating = true;
                    excellerating = false;
                }

            }
            else
            {
                decellerating = false;
            }

            if (Input.GetKey(KeyCode.LeftShift))
            {
                if (excellerating == true)
                {
                    moveSpeed += Time.deltaTime;

                    if (moveSpeed > maxMoveSpeed)
                    {
                        excellerating = false;
                    }
                }
                else
                {
                    excellerating = true;
                    decellerating = false;
                }
            }
            else
            {
                excellerating = false;
            }

            #region Assisted Tilt Shifting to ease into a position on release
            //Smooth out to 0 on the horizontal axis 
           /* if (horizontal == 0)
            {
                if ((transform.rotation.eulerAngles.z < 45f && transform.rotation.eulerAngles.z > 0) || (transform.rotation.eulerAngles.z > 315f && transform.rotation.eulerAngles.z < 360f))
                {
                    Debug.Log("North Position");
                    // Define a target position above and behind the target transform
                    targetHorizontalRotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, 0);
                }
                else if (transform.rotation.eulerAngles.z > 45f && transform.rotation.eulerAngles.z < 135f)
                {
                    Debug.Log("West Position");
                    // Define a target position above and behind the target transform
                    targetHorizontalRotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, 90f);
                }
                else if (transform.rotation.eulerAngles.z > 135f && transform.rotation.eulerAngles.z < 225)
                {
                    Debug.Log("South Position");
                    // Define a target position above and behind the target transform
                    targetHorizontalRotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, 180f);
                }
                else if (transform.rotation.eulerAngles.z > 225f && transform.rotation.eulerAngles.z < 315f)
                {
                    Debug.Log("East Position");
                    // Define a target position above and behind the target transform
                    targetHorizontalRotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, -90f);
                }

                // Debug.Log(transform.rotation.eulerAngles.z);


                // Smoothly move the camera towards that target position
                transform.rotation = Quaternion.Slerp(transform.rotation, targetHorizontalRotation, smoothTime);
            }*/
            #endregion

            if (horizontal != 0 || vertical != 0)
            {
                transform.Rotate(vertical, 0f, -horizontal);
            }
        }
       
    }

   
    private void PlaneMovement()
    {
        //rb.velocity = new Vector3(horizontal, rb.velocity.y, rb.velocity.z) * moveSpeed;

        if (takeOff == true)
        {
            transform.position += transform.forward * moveSpeed * Time.deltaTime;

            if (transform.rotation.eulerAngles.x > 30 && transform.rotation.eulerAngles.x < 150)
            {
                moveSpeed += Time.deltaTime;
            }
            else if (transform.rotation.eulerAngles.x < -30 && transform.rotation.eulerAngles.x > -150)
            {
                moveSpeed -= Time.deltaTime;

                if (moveSpeed == 0)
                {
                    rb.useGravity = true;
                }
                else
                {
                    rb.useGravity = false;
                }
            }

            if (transform.rotation.eulerAngles.x < 30 && transform.rotation.eulerAngles.x > -30)
            {
                if (moveSpeed < startMoveSpeed)
                {
                    moveSpeed += Time.deltaTime;

                    if (moveSpeed == startMoveSpeed)
                    {
                        moveSpeed = startMoveSpeed;
                    }
                }

                if (moveSpeed > startMoveSpeed)
                {
                    moveSpeed -= Time.deltaTime;

                    if (moveSpeed == startMoveSpeed)
                    {
                        moveSpeed = startMoveSpeed;
                    }
                }
            }
        }
      

    }


    private void PlaneLanding()
    {
      //  Debug.Log("I AM TRYING TO LAND??");

        if (landed == true)
        {
            Debug.Log("I AM TRYING TO LAND??");

            playerCharacter.gameObject.GetComponent<PlayerHumanController>().enabled = true;

            if (playerCharacter.gameObject.GetComponent<PlayerHumanController>().enabled == true)
            {
                GetComponent<PlaneController>().enabled = false;
            }
            
        }
    }
}
