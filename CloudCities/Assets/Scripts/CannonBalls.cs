using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonBalls : MonoBehaviour
{
    public float moveSpeed;
    private Rigidbody rb;

    Transform target;
    public Transform hitArea;

    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.Find("Player").transform;
        rb = GetComponent<Rigidbody>();
        hitArea = target;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 targetLocation = hitArea.position - transform.position;
        float dist = targetLocation.magnitude;
        rb.AddRelativeForce(Vector3.forward * Mathf.Clamp((dist) / 50, 0f, 1f) * moveSpeed);

        if(dist == 0)
        {
            gameObject.SetActive(false);
        }
    }
}
