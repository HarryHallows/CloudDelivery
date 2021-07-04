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
    public bool rotateAround, cameraFollow = true;

    [SerializeField]
    private float smoothSpeed;
    [SerializeField] private float directionSpeed;
    [SerializeField] private float direction;

    [Tooltip("X Axis")][SerializeField] private float camYaw;
    [Tooltip("Y Axis")][SerializeField] private float camPitch;

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

       

        if (rotateAround == false)
        {
            if (cameraFollow == true)
            {
                // compute position
                if (offsetPositionSpace == Space.Self)
                {
                    transform.position = followTarget.TransformPoint(offsetPosition);
                }
                else
                {
                    transform.position = smoothPosition;
                }
            }  
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

    public void CameraRotate(GameObject _target)
    {
        Debug.Log(_target.transform.position -= gameObject.transform.position);

        camYaw += directionSpeed * Input.GetAxis("Mouse X");

        // transform.eulerAngles = new Vector3(transform.rotation.eulerAngles.x, camYaw, transform.rotation.eulerAngles.z);
        _target.transform.position = new Vector3(offsetPosition.x, _target.transform.position.y, offsetPosition.z);

        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, 0f);

        transform.RotateAround(_target.transform.position, Vector3.up, camYaw * Time.deltaTime);
    }
}
