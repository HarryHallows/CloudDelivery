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
        Vector3 smoothPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.captureFramerate);

        if (followTarget.gameObject.name == "PlayerPlane")
        {
            if (followTarget.gameObject.GetComponent<PlaneController>().takeOff == true)
            {
                if (followTarget.gameObject.GetComponent<PlaneController>().decellerating == true)
                {
                    offsetPosition.z += 5f * Time.deltaTime;

                    Debug.Log("decreasing distance due to speed decrease");

                    if (offsetPosition.z >= -5f)
                    {
                        offsetPosition.z = -5f;
                    }
                }

                if (followTarget.gameObject.GetComponent<PlaneController>().excellerating == true)
                {
                    offsetPosition.z -= 5f * Time.deltaTime;

                    Debug.Log("Increasing distance due to speed increase");

                    if (offsetPosition.z <= -30f)
                    {
                       
                        offsetPosition.z = -30f;
                    }
                }

                if (followTarget.gameObject.GetComponent<PlaneController>().excellerating == false && followTarget.gameObject.GetComponent<PlaneController>().decellerating == false)
                {
                   
                    if (offsetPosition.z < -20f)
                    {
                        offsetPosition.z += 5f * Time.deltaTime;
                        Debug.Log("decreasing distance due to speed decrease");

                        
                        if (offsetPosition.z >= -20f)
                        {
                            Debug.Log("I SHOULD CLMAP PLEASE!!!");
                            offsetPosition.z = -20f;
                        }
                    }
                    else
                    {
                        if (offsetPosition.z > -20f)
                        {
                            offsetPosition.z -= 5f * Time.deltaTime;
                            Debug.Log("Increasing distance due to speed increase");

                            if (offsetPosition.z <= -20f)
                            {
                                Debug.Log("I SHOULD CLMAP PLEASE!!!");
                                offsetPosition.z = -20f;
                            }
                        }                       
                    }

                }
            }
            else
            {
                offsetPosition.z = -10f;
            }
            
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

    public void FollowTarget(Transform target)
    {
        followTarget = target;
    }

    public void LookAtTarget(Transform target)
    {
        lookAtTarget = target;
    }
}
