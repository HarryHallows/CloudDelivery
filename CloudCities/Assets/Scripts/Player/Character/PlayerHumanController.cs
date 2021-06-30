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
    private float moveSpeed, jumpHeight;
    private float gravityAmount;

    private Vector3 movement;
    private Vector3 playerVelocity;

    private bool sprint, playerGrounded;

    [SerializeField] private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        cam.GetComponent<CameraFollow>().TargetSwitch(gameObject.transform);
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

        movement = new Vector3(horizontal, 0, vertical);

        if (movement != Vector3.zero)
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
