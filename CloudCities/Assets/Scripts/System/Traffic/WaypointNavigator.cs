using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointNavigator : MonoBehaviour
{

    CharacterNavigationController controller;
    public Waypoint currentWaypoint;

    private int direction;

    private void Awake()
    {
        controller = GetComponent<CharacterNavigationController>();
    }

    // Start is called before the first frame update
    void Start()
    {
        controller.SetDestination(currentWaypoint.GetPosition());
        direction = Mathf.RoundToInt(Random.Range(0f, 1f));
    }

    // Update is called once per frame
    void Update()
    {
        WaypointSelection();
    }

    private void WaypointSelection()
    {

        if (controller.reachedDestination)
        {
            bool shouldBranch = false;

            if (currentWaypoint.branches != null && currentWaypoint.branches.Count > 0)
            {
                //random check to decide on a random chance if the branch waypoint should be taken by AI or not.
                shouldBranch = Random.Range(0f, 1f) <= currentWaypoint.branchRatio ? true : false;
            }

            if (shouldBranch == true)
            {
                //if branching is true then we should change the next waypoint position to the branches waypoint position
                currentWaypoint = currentWaypoint.branches[Random.Range(0, currentWaypoint.branches.Count - 1)];
            }
            else
            {
                //preventing deadends
                if (direction == 0)
                {
                    //checking if the next waypoint is null
                    if (currentWaypoint.nextWaypoint != null)
                    {
                        currentWaypoint = currentWaypoint.nextWaypoint;
                    }
                    else
                    {
                        //if the next waypoint IS null then change direction
                        currentWaypoint = currentWaypoint.previousWaypoint;
                        direction = 1;
                    }
                }

                else if (direction == 1)
                {
                    //checking if the previous waypoint is null
                    if (currentWaypoint.previousWaypoint != null)
                    {
                        currentWaypoint = currentWaypoint.previousWaypoint;
                    }
                    else
                    {
                        //if the previous waypoint IS null then change direction
                        currentWaypoint = currentWaypoint.nextWaypoint;
                        direction = 0;
                    }
                }
            }

            controller.SetDestination(currentWaypoint.GetPosition());
        }

    }
}
