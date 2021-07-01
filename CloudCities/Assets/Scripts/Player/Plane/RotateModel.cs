using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateModel : MonoBehaviour
{
    [SerializeField] private PlayerController planeControl;
    public float xRot, yRot;
    [SerializeField] private float rotationSpeed;

    [SerializeField] private Quaternion startRotation;
    [SerializeField] private Quaternion targetRotationX;
    [SerializeField] private Quaternion targetRotationY;

    [SerializeField] Camera cam;
 
    // Start is called before the first frame update
    void Start()
    {
        startRotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
        
        cam.GetComponent<CameraFollow>().LookAtTarget(gameObject.transform);
    }

    // Update is called once per frame
    void Update()
    {
        RotateSelf();
    }

    private void RotateSelf()
    {
        // Smoothly move the camera towards that target position


        if (planeControl.horizontal > 0)
        {        
            targetRotationX = Quaternion.Euler(90f, transform.eulerAngles.y, transform.eulerAngles.z);

            if (transform.rotation.eulerAngles.x >= 90)
            {
                transform.rotation = Quaternion.Euler(90f, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
            }

            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotationX, rotationSpeed);

            //transform.Rotate((new Vector3(transform.eulerAngles.x, startRotation.y, startRotation.z) * planeControl.horizontal) * rotationSpeed * Time.fixedDeltaTime);

            //transform.Rotate(90.0f, 0.0f, 0.0f, Space.Self);
        }
        else if(planeControl.horizontal < 0f)
        {

            targetRotationX = Quaternion.Euler(-90f, 90f, transform.eulerAngles.z);
        
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotationX, rotationSpeed);
        }

        if (planeControl.vertical < 0f)
        {
            targetRotationY = Quaternion.Euler(transform.rotation.eulerAngles.x, 90f, -90f);

            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotationY, rotationSpeed);
        }
        else if(planeControl.vertical < 0f)
        {
            targetRotationY = Quaternion.Euler(transform.rotation.eulerAngles.x, 90f, 90f);

            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotationY, rotationSpeed);
        }
    }
}
