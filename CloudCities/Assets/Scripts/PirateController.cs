using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PirateController : MonoBehaviour
{
    public float lookRadius = 10f;
    public float shootingDistance;
    public float stoppingDistance;
    public float thrust;
    private Rigidbody rb;

    public Transform target;
    public Transform hitArea;

    void Start()
    {
        //target = GameObject.Find("Player").transform;
        rb = GetComponent<Rigidbody>();        
    }


    void FixedUpdate()
    {
        float distance = Vector3.Distance(target.position, transform.position);

        if(distance <= lookRadius)
        {
            //long combat
            transform.LookAt(target);

            Vector3 targetLocation = target.position - transform.position;
            float dist = targetLocation.magnitude;
            rb.AddRelativeForce(Vector3.forward * Mathf.Clamp((dist - stoppingDistance) / 50, 0f, 1f) * thrust);

            if (distance <= stoppingDistance)
            {
                //close combat
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
    }
}
