using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullets : MonoBehaviour
{
    public float moveSpeed;

    private Transform target;
    public Transform hitArea;

    Vector3 dir;

    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.Find("Player").transform;
        hitArea = target;
        dir = hitArea.position - transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null)
        {
            gameObject.SetActive(false);
        }


        float distanceThisFrame = moveSpeed * Time.deltaTime;

        if ((hitArea.position - transform.position).magnitude <= distanceThisFrame)
        {
            BullHit();
        }

        transform.Translate(dir.normalized * distanceThisFrame, Space.World);
    }

    void BullHit()
    {
        gameObject.SetActive(false);
        //do damage
    }
}
