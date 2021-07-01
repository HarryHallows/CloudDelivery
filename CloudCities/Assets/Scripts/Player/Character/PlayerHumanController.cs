using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHumanController : MonoBehaviour
{
  
    [SerializeField] private Camera cam;

    [Header("Floats")]
    //Inputs
    private float horizontal;
    private float vertical;
    
    //Movements
    [SerializeField] private float moveSpeed, jumpHeight;
    [SerializeField] private float gravityAmount;

    [SerializeField] private float smoothTime;
    private float turnSmoothVelocity;

    private Vector3 direction;
    private Vector3 playerVelocity;

    [SerializeField] private bool sprint, playerGrounded;

    [SerializeField] private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;

        cam.GetComponent<CameraFollow>().FollowTarget(gameObject.transform);
        cam.GetComponent<CameraFollow>().LookAtTarget(gameObject.transform);

        anim = GetComponent<Animator>();    
    }

    // Update is called once per frame
    void Update()
    {
        PlayerInputs();
    }


    private void PlayerInputs()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (sprint == false)
            {
                sprint = true;
            }
        }
        else
        {
            if (sprint == true)
            {
                sprint = false;
            }
        }

        direction = new Vector3(horizontal, 0, vertical).normalized;

        if (direction.magnitude >= 0.1f)
        {
            Rotation();
            GroundedMovement();
        }


        if (Input.GetButton("Jump") && playerGrounded)
        {
            Jump();
        }

        Animations();

        playerVelocity.y += gravityAmount * Time.deltaTime;
    }

    private void Animations()
    {
        if (horizontal != 0)
        {
            if (sprint == false)
            {
                anim.SetTrigger("isWalking");
            }
            else
            {
                anim.SetTrigger("isRunning");
            }
        }
    }

    private void Rotation()
    {
        float targetAngle = Mathf.Atan2(direction.x, direction.z);
        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, smoothTime);

        transform.rotation = Quaternion.Euler(0f, angle, 0f);
    }

    private void GroundedMovement()
    {
        gameObject.transform.forward = direction;
    }

    private void Jump()
    {
        playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityAmount);
    }
}
