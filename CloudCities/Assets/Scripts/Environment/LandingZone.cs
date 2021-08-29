using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandingZone : MonoBehaviour
{

    [SerializeField] private Material mat;

    [SerializeField] private Color color;
    [SerializeField] private bool changeAlpha = false;


    //Radius Check for the island
    public float landingRadius = 20f;

    // Start is called before the first frame update
    void Start()
    {
        mat = GetComponent<Renderer>().material;
        color = mat.color;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, landingRadius);
    }
}
