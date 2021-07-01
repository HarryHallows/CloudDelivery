using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
    public Waypoint previousWaypoint;
    public Waypoint nextWaypoint;

    [Tooltip("Decides how wide the path will be")]
    [Range(0f, 5f)]
    public float width = 1f;

    [Tooltip("decides how likely the AI are to take this branch")]
    [Range(0f, 1f)]
    public float branchRatio = 0.5f;

    public List<Waypoint> branches;

    public Vector3 GetPosition()
    {
        //position boundaries Maximum and Minimum providing the character so moving space between each waypoint
        Vector3 minBound = transform.position + transform.right * width / 2f;
        Vector3 maxBound = transform.position - transform.right * width / 2f;

        //Lerp between the different boundaries at a random time between 0 and 1
        return Vector3.Lerp(minBound, maxBound, Random.Range(0f, 1f));
    }
}
