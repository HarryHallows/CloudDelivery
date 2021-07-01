using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHumanController : MonoBehaviour
{
    private CharacterController controller;

    [SerializeField] private Camera cam;

    [Header("Floats")]
    //Inputs
    private float horizontal;
    private float vertical;
    
    //Movements
    [SerializeField] private float moveSpeed, jumpHeight;
    [SerializeField] private float gravityAmount;

    private Vector3 movement;
    private Vector3 playerVelocity;

    [SerializeField] private bool sprint, playerGrounded;

    [SerializeField] private Animator anim;

    // Start is called before the first frame update
    void Start()
    {

        cam = Camera.main;

        cam.GetComponent<CameraFollow>().FollowTarget(gameObject.transform);
        cam.GetComponent<CameraFollow>().LookAtTarget(gameObject.transform);

        controller = GetComponent<CharacterController>();
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

        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (sprint == false)
            {
                sprint = true;
            }

            if(sprint == true)
            {
                sprint = false;
            }
        }

        movement = new Vector3(horizontal, 0, vertical);

        if (horizontal != 0 || vertical != 0)
        {
            GroundedMovement();
        }

        if (Input.GetButtonDown("Jump") && playerGrounded)
        {
            Jump();
        }

        Animations();

        playerVelocity.y += gravityAmount * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
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

    private void GroundedMovement()
    {
        gameObject.transform.forward = movement;
    }

    private void Jump()
    {
        playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityAmount);
    }
}
