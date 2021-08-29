using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterNavigationController : MonoBehaviour
{

    Animator _animator;

    public Vector3 destination;
    private Vector3 lastPosition;
    Vector3 velocity;

    public float rotationSpeed = 120f;
    public float movementSpeed = 1f;
    public float stopDistance = 2f;

    private float smoothTime;
  
    public bool reachedDestination;

    Quaternion targetRotation;


    private void Awake()
    {
        movementSpeed = Random.Range(.8f, 2f);
        _animator = GetComponent<Animator>();
    }


    // Start is called before the first frame update
    void Start()
    {
        smoothTime = Random.Range(0, 1);
    }

    // Update is called once per frame
    void Update()
    {
        CharacterMovement();
    }

    private void CharacterMovement()
    {
        //If the attached character has not reached the "destination" (next waypoint Node)
        if (transform.position != destination)
        {
            //Move towards the destinations position 
            Vector3 destinationDirection = destination - transform.position;
            destinationDirection.y = 0;

            float destinationDistance = destinationDirection.magnitude;
            
            //If character hasn't reached destination then calculate the look rotation to the direction of the next node
            if (destinationDistance >= stopDistance)
            {
                reachedDestination = false;
                targetRotation = Quaternion.LookRotation(destinationDirection);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);


                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, smoothTime);

                transform.Translate(Vector3.forward * movementSpeed * Time.deltaTime);
            }
            else
            {
                //Otherwise destination has been reached
                reachedDestination = true;
            }

            velocity = (transform.position - lastPosition) / Time.deltaTime;
            velocity.y = 0;

            var velocityMagnitude = velocity.magnitude;
            velocity = velocity.normalized;
            var forwardDotProduct = Vector3.Dot(transform.forward, velocity);
            var rightDotProduct = Vector3.Dot(transform.right, velocity);
        }

        lastPosition = transform.position;
    }

    //Reset the destination node
    public void SetDestination(Vector3 destination)
    {
        this.destination = destination;
        reachedDestination = false;
    }

}
