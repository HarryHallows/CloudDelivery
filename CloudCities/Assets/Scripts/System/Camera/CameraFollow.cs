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
    [SerializeField] private float directionSpeed;

    private void LateUpdate()
    {
        Refresh();
    }

    public void Refresh()
    {
        Vector3 desiredPosition = followTarget.position + offsetPosition;
        Vector3 smoothPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.captureFramerate);

        if (followTarget.gameObject.name == "PlayerPlane")
        {
        
            
        }

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

    public void FollowTarget(Transform _target)
    {
        followTarget = _target;
    }

    public void LookAtTarget(Transform _target)
    {
        lookAtTarget = _target;
    }

    public void CameraRotate(Transform _target, float _direction)
    {
        transform.RotateAround(_target.transform.position, Vector3.up, (_direction * directionSpeed) * Time.deltaTime);
    }
}
