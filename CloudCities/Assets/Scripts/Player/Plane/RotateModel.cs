using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateModel : MonoBehaviour
{
    [SerializeField] private PlaneController planeControl;
    public float xRot, yRot;
    [SerializeField] private float smoothTime;

    [SerializeField] private Quaternion startRotation;
    [SerializeField] private Quaternion targetVerticalRotation;

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
        transform.rotation = Quaternion.Slerp(transform.rotation, planeControl.transform.rotation, smoothTime);
    }
}
