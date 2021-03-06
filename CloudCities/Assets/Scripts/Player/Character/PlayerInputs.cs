using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputs : MonoBehaviour
{

    [Header("Component")]
    [SerializeField] private PlayerController playerControl;

    [Header("Character Input Values")]
    public Vector2 move;
    public Vector2 look;

    public bool interact;
    public bool jump;
    public bool sprint;

    [Header("Movement Settings")]
    public bool analogMovement;

    // Start is called before the first frame update
    void Start()
    {
        playerControl = GetComponent<PlayerController>(); 
    }

    // Update is called once per frame
    void Update()
    {
        Inputs();
    }

    private void Inputs()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        move = new Vector2(horizontal, vertical).normalized;

        float camYaw = Input.GetAxis("Mouse X");
        float camPitch = Input.GetAxis("Mouse Y");

        look = new Vector2(camYaw, camPitch);

        if (Input.GetKey(KeyCode.LeftShift) && (horizontal != 0 || vertical != 0))
        {
            sprint = true;
        }
        else
        {
            sprint = false;
        }

        if (Input.GetButton("Jump"))
        {
            Debug.Log("Should be jumping");
            jump = true;
        }
        else
        {
            jump = false;
        }
    }


    public void MoveInput(Vector2 newMoveDirection)
    {
        move = newMoveDirection;
    }

    public void LookInput(Vector2 newLookDirection)
    {
        look = newLookDirection;
    }

    public void JumpInput(bool newJumpState)
    {
        jump = newJumpState;
    }

    public void SprintInput(bool newSprintState)
    {
        sprint = newSprintState;
    }
}
