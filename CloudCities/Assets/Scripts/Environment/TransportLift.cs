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

    public void Lift(GameObject _player)
    {
        goingDown = !goingDown;

        _player.transform.position = passengerPosition.transform.position;

        Debug.Log(_player.transform.position = passengerPosition.transform.position);

        if (goingDown == true)
        {
            //make baloon move towards lift target position
        }
    }

    private IEnumerator GoDown()
    {


        yield return new WaitForSeconds(transportTime);

    }
}
