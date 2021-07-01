using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class WaypointManager : EditorWindow
{

    [MenuItem("Tools/Waypoint Editor")]

    public static void Open()
    {
        GetWindow<WaypointManager>();
    }

    public Transform waypointRoot; // Waypoint transform

    public void OnGUI()
    {
        SerializedObject obj = new SerializedObject(this);

        EditorGUILayout.PropertyField(obj.FindProperty("waypointRoot"));

        if (waypointRoot == null)
        {
            EditorGUILayout.HelpBox(("ROOT TRANSFORM MUST BE SELECTED. Please Assign a root transform!!!."), MessageType.Warning);
        }
        else
        {
            EditorGUILayout.BeginVertical("box");
            DrawButtons();
            EditorGUILayout.EndVertical();
        }

        obj.ApplyModifiedProperties();
    }

    private void DrawButtons()
    {
        if (GUILayout.Button("Create Waypoint"))
        {
            CreateWaypoint();
        }

        if (Selection.activeGameObject != null && Selection.activeGameObject.GetComponent<Waypoint>())
        {
            if (GUILayout.Button("Create Waypoint Before"))
            {
                CreateWaypointBefore();
            }

            if (GUILayout.Button("Create Waypoint After"))
            {
                CreateWaypointAfter();
            }

            if (GUILayout.Button("Remove Waypoint"))
            {
                RemoveWaypoint();
            }

            if (GUILayout.Button("Add Branch Waypoint"))
            {
                CreateBranchWaypoint();
            }

        }
    }

    private void CreateWaypoint()
    {
        GameObject waypointObject = new GameObject("Waypoint " + waypointRoot.childCount, typeof(Waypoint));
        waypointObject.transform.SetParent(waypointRoot, false);

        Waypoint waypoint = waypointObject.GetComponent<Waypoint>();

        if (waypointRoot.childCount > 1)
        {
            waypoint.previousWaypoint = waypointRoot.GetChild(waypointRoot.childCount - 2).GetComponent<Waypoint>();
            waypoint.previousWaypoint.nextWaypoint = waypoint;

            //Place the waypoint at the last position 

            waypoint.transform.position = waypoint.previousWaypoint.transform.position;
            waypoint.transform.forward = waypoint.previousWaypoint.transform.forward;
        }

        Selection.activeGameObject = waypoint.gameObject;
    }

    //Creates a waypoint before the selected waypoint
    private void CreateWaypointBefore()
    {
        GameObject waypointObject = new GameObject("Waypoint " + waypointRoot.childCount, typeof(Waypoint));
        waypointObject.transform.SetParent(waypointRoot, false);

        Waypoint newWaypoint = waypointObject.GetComponent<Waypoint>();

        Waypoint selectWaypoint = Selection.activeGameObject.GetComponent<Waypoint>();

        waypointObject.transform.position = selectWaypoint.transform.position;
        waypointObject.transform.forward = selectWaypoint.transform.forward;

        if (selectWaypoint.previousWaypoint != null)
        {
            newWaypoint.previousWaypoint = selectWaypoint.previousWaypoint;
            selectWaypoint.previousWaypoint.nextWaypoint = newWaypoint;
        }

        newWaypoint.nextWaypoint = selectWaypoint;

        selectWaypoint.previousWaypoint = newWaypoint;

        newWaypoint.transform.SetSiblingIndex(selectWaypoint.transform.GetSiblingIndex());

        Selection.activeGameObject = newWaypoint.gameObject;



    }

    //Creates a waypoint after the selected waypoint
    private void CreateWaypointAfter()
    {

        GameObject waypointObject = new GameObject("Waypoint " + waypointRoot.childCount, typeof(Waypoint));
        waypointObject.transform.SetParent(waypointRoot, false);

        Waypoint newWaypoint = waypointObject.GetComponent<Waypoint>();

        Waypoint selectWaypoint = Selection.activeGameObject.GetComponent<Waypoint>();

        waypointObject.transform.position = selectWaypoint.transform.position;
        waypointObject.transform.forward = selectWaypoint.transform.forward;

        newWaypoint.previousWaypoint = selectWaypoint;

        if (selectWaypoint.nextWaypoint != null)
        {
            selectWaypoint.nextWaypoint.previousWaypoint = newWaypoint;
            newWaypoint.nextWaypoint = selectWaypoint.nextWaypoint;

        }


        selectWaypoint.nextWaypoint = newWaypoint;

        newWaypoint.transform.SetSiblingIndex(selectWaypoint.transform.GetSiblingIndex());

        Selection.activeGameObject = newWaypoint.gameObject;

    }

    //Removes selected waypoint
    private void RemoveWaypoint()
    {
        Waypoint selectWaypoint = Selection.activeGameObject.GetComponent<Waypoint>();

        if (selectWaypoint.nextWaypoint != null)
        {
            selectWaypoint.nextWaypoint.previousWaypoint = selectWaypoint.previousWaypoint;
        }

        if (selectWaypoint.previousWaypoint != null)
        {
            selectWaypoint.previousWaypoint.nextWaypoint = selectWaypoint.nextWaypoint;
            Selection.activeGameObject = selectWaypoint.previousWaypoint.gameObject;
        }

        DestroyImmediate(selectWaypoint.gameObject);
    }

    private void CreateBranchWaypoint()
    {
        GameObject waypointObject = new GameObject("Waypoint " + waypointRoot.childCount, typeof(Waypoint));
        waypointObject.transform.SetParent(waypointRoot, false);

        Waypoint waypoint = waypointObject.GetComponent<Waypoint>();

        Waypoint branchedFrom = Selection.activeGameObject.GetComponent<Waypoint>();

        branchedFrom.branches.Add(waypoint);

        waypoint.transform.position = branchedFrom.transform.position;
        waypoint.transform.forward = branchedFrom.transform.forward;

        Selection.activeGameObject = waypoint.gameObject;
    }
}
