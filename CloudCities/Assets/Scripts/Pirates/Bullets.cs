using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullets : MonoBehaviour
{
    public float moveSpeed;

    private Transform target;
    public Transform hitArea;

    public int damage;

    Vector3 dir;

    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.Find("Player").transform;
        hitArea = target;
        dir = hitArea.position - transform.position;

        damage = Random.Range(5, 10);
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null)
        {
            gameObject.SetActive(false);
        }


        float distanceThisFrame = moveSpeed * Time.deltaTime;

        if ((hitArea.position - transform.position).magnitude <= distanceThisFrame )
        {
            StartCoroutine(BullHit());
        }

        transform.Translate(dir.normalized * distanceThisFrame, Space.World);

        if(Vector3.Distance(transform.position, target.position) > 30)
        {
            gameObject.SetActive(false);
        }
    }

    IEnumerator BullHit()
    {
        GameObject smoke = ObjectPool.SharedInstance.SpawnFromPool("Smoke", transform.position);
        smoke.SetActive(true);
        yield return new WaitForSeconds(1);
        gameObject.SetActive(false);
        //do damage
    }
}
