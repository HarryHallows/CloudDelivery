using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField]
    private Transform followTarget;
    [SerializeField]
    private Transform lookAtTarget;

    [SerializeField]
    private Vector3 offsetPosition;

    [SerializeField]
    private Space offsetPositionSpace = Space.Self;

    [SerializeField]
    private bool lookAt = true;

    [SerializeField]
    private float smoothSpeed;

    private void LateUpdate()
    {
        Refresh();
    }

    public void Refresh()
    {
        Vector3 desiredPosition = followTarget.position + offsetPosition;
        Vector3 smoothPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);


        if (followTarget == null || lookAtTarget == null)
        {
            Debug.LogWarning("Missing target ref !", this);

            return;
        }

        // compute position
        if (offsetPositionSpace == Space.Self)
        {
            transform.position = followTarget.TransformPoint(offsetPosition);
        }
        else
        {
            transform.position = smoothPosition;
        }

        // compute rotation
        if (lookAt)
        {
            transform.LookAt(lookAtTarget);
        }
        else
        {
            Debug.Log(lookAtTarget);
            transform.rotation = lookAtTarget.rotation;
        }
    }
}
