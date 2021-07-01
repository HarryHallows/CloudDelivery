using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [Header("Component Calls")]
    private Animator anim;
    private Rigidbody rb;

    [Header("Floats")]
    //Speeds
    [SerializeField] private float moveSpeed, thurstSpeed;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float turnSmoothVelocity;

    //Input values
    [SerializeField] public float vertical;
    [SerializeField] public float horizontal;

    [SerializeField] private bool isPlane;

    [SerializeField] Vector3 velocityCheck;
    [SerializeField] Vector3 velcoity;

    [SerializeField] Vector3 direction;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        PlayerInputs();
    }

    private void FixedUpdate()
    {
        Rotate();
        Movement();

        velocityCheck = rb.velocity;
    }

    private void PlayerInputs()
    {
        vertical = Input.GetAxis("Vertical");
        horizontal = Input.GetAxis("Horizontal");


        direction = new Vector3(horizontal, 0, vertical).normalized;
    }

    private void Movement()
    {
        if (isPlane == false)
        {
            rb.velocity += (transform.forward * vertical) * moveSpeed * Time.fixedDeltaTime;
        }
        else
        {
            rb.velocity += ((transform.forward * thurstSpeed) * Time.fixedDeltaTime);
            
            if (horizontal != 0)
            {
                rb.velocity += (horizontal * transform.right) * thurstSpeed * Time.fixedDeltaTime;

            }

            if (vertical != 0)
            {
                rb.velocity += (transform.up * vertical) * thurstSpeed * Time.fixedDeltaTime;
            }
            
        }

    }

    private void Rotate()
    {
        /*
        if (isPlane == true)
        {
            if (horizontal != 0)
            {

            }
            transform.Rotate((transform.right * vertical) * rotationSpeed * Time.fixedDeltaTime);
            transform.Rotate((transform.forward * horizontal) * rotationSpeed * Time.fixedDeltaTime);
        }
        else
        {
            transform.Rotate((transform.up * horizontal) * rotationSpeed * Time.fixedDeltaTime);

        }*/
    }
}
