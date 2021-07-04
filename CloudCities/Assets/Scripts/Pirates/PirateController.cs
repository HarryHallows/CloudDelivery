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
    public GameObject[] cannon;

    public float fireRate = .5f;
    private float fireRateLeft = 0f;

    public float bulletRate = .5f;
    private float bulletRateLeft = 0f;

    public bool closeCombat;

    public Transform shipGFX;
    public float rotSpeed = 5;


    


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
            gameObject.GetComponent<CharacterNavigationController>().enabled = false;
            gameObject.GetComponent<WaypointNavigator>().enabled = false;

            fireRateLeft -= Time.deltaTime;
            if(fireRateLeft <= 00 && !closeCombat)
            {
                FireCannon();
                fireRateLeft = fireRate;

                Debug.Log("Long Combat");
            }


            shipGFX.rotation = Quaternion.Lerp(shipGFX.rotation, Quaternion.Euler(0, 90, 0) ,Time.deltaTime * rotSpeed);
            transform.LookAt(target);
            

            Vector3 targetLocation = target.position - transform.position;
            float dist = targetLocation.magnitude;
            rb.AddRelativeForce(Vector3.forward * Mathf.Clamp((dist - stoppingDistance) / 50, 0f, 1f) * thrust);

            if (distance <= shootingDistance)
            {
                //close combat
                closeCombat = true;
                bulletRateLeft -= Time.deltaTime;
                if (bulletRateLeft <= 00 && closeCombat)
                {
                    FireBullet();
                    bulletRateLeft = bulletRate;
                    Debug.Log("Close Combat");
                }
            }
            else
            {
                closeCombat = false;
            }
        }

        if(distance >= lookRadius)
        {
            Patrol();
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
    }

    private void FireCannon()
    {
        GameObject cannonBall = ObjectPool.SharedInstance.SpawnFromPool("Cannon Balls", cannon[Random.Range(0, 1)].transform.position);
        cannonBall.SetActive(true);
    }

    private void FireBullet()
    {
        GameObject bullet = ObjectPool.SharedInstance.SpawnFromPool("Bullets", cannon[Random.Range(0,1)].transform.position);
        bullet.SetActive(true);
    }

    void Patrol()
    {
        shipGFX.rotation = shipGFX.parent.rotation;
        gameObject.GetComponent<CharacterNavigationController>().enabled = true;
        gameObject.GetComponent<WaypointNavigator>().enabled = true;
    }
}
