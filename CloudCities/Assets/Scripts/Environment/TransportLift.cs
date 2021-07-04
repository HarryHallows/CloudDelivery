using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransportLift : MonoBehaviour
{

    private float transportTime;

    public bool goingDown;

    [SerializeField] Transform passengerPosition;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Lift(Transform _playerPos)
    {
        goingDown = !goingDown;

        _playerPos = passengerPosition;

        if (goingDown == true)
        {

        }
    }

    private IEnumerator GoDown()
    {


        yield return new WaitForSeconds(transportTime);

    }
}
