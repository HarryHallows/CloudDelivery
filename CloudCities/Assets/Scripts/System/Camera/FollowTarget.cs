using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    [SerializeField] private Transform target;

    [SerializeField] public float smoothSpeed;

    [SerializeField] private bool isMinimap;

    // Update is called once per frame
    void LateUpdate()
    {

        if (isMinimap)
        {
            Minimap();
        }
        else
        {
            Follow();
        }
       
    }

    private void Minimap()
    {
        Vector3 newPosition = target.position;
        newPosition.y = transform.position.y;

        transform.position = newPosition;

        transform.rotation = Quaternion.Euler(90f, target.eulerAngles.y, 0f);
    }

    private void Follow()
    {
        Vector3 desiredPosition = target.position;
        Vector3 smoothPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);

        transform.position = smoothPosition;
    }
}
