using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandingZone : MonoBehaviour
{

    [SerializeField] private Material mat;

    [SerializeField] private Color color;
    [SerializeField] private bool changeAlpha = false;

    // Start is called before the first frame update
    void Start()
    {
        mat = GetComponent<Renderer>().material;
        color = mat.color;
    }

    // Update is called once per frame
    void Update()
    {
        BlinkAnim();
    }

    private void BlinkAnim()
    {
        mat.color = color;

        Debug.Log(color);

        if (color.a >= 1 && changeAlpha != true)
        {
            changeAlpha = true;
        }

        if (color.a <= 0 && changeAlpha != false)
        {
            changeAlpha = false;
        }


        if (changeAlpha == true)
        {
            color.a += 1 * Time.deltaTime;
        }
        else
        { 
            color.a -= 1 * Time.deltaTime;   
        }
    }
}
