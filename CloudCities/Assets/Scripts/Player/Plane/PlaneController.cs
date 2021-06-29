using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlaneController : MonoBehaviour
{
    //Scripts
    
    //Inputs
    [SerializeField] private float horizontal;
    [SerializeField] private float vertical;


    public float smoothTime;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float startMoveSpeed;

    private Vector3 velocity = Vector3.zero;

    [SerializeField] private Quaternion targetVerticalRotation;
    [SerializeField] private Quaternion targetHorizontalRotation;


    private Rigidbody rb;

    public Transform planeModel;


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
     
    // Start is called before the first frame update
    void Start()
    {
        moveSpeed = startMoveSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("z :" + transform.rotation.z);
        PlaneInputs();
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

        if (horizontal != 0 || vertical != 0)
        {
            transform.Rotate(vertical, 0f, -horizontal);
        }

        #region Assisted Tilt Shifting to ease into a position on release
        //Smooth out to 0 on the horizontal axis 
        if (horizontal == 0)
        {
            if ((transform.rotation.eulerAngles.z < 45f && transform.rotation.eulerAngles.z > 0) || (transform.rotation.eulerAngles.z > 315f  && transform.rotation.eulerAngles.z < 360f))
            {
                Debug.Log("North Position");
                // Define a target position above and behind the target transform
                targetHorizontalRotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, 0);
            }
            else if(transform.rotation.eulerAngles.z > 45f && transform.rotation.eulerAngles.z < 135f)
            {
                Debug.Log("West Position");
                // Define a target position above and behind the target transform
                targetHorizontalRotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, 90f);
            }
            else if(transform.rotation.eulerAngles.z > 135f && transform.rotation.eulerAngles.z < 225)
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
        }
        #endregion
    }

   
    private void PlaneMovement()
    {
        //rb.velocity = new Vector3(horizontal, rb.velocity.y, rb.velocity.z) * moveSpeed;

        transform.position += transform.forward * moveSpeed * Time.deltaTime;

        if (transform.rotation.eulerAngles.x > 30 && transform.rotation.eulerAngles.x < 150)
        {
            moveSpeed += Time.deltaTime;
        }
        else if(transform.rotation.eulerAngles.x < -30 && transform.rotation.eulerAngles.x > -150)
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
            
            if(moveSpeed > startMoveSpeed)
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
