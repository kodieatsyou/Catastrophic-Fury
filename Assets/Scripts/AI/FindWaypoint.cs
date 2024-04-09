using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;
using Unity.VisualScripting;

public class FindWaypoint : Action
{
    public SharedVector3 target;
    public GameObject waypointsHolder;
    public Transform[] waypoints;
    public int currentWaypointIndex = 0;

    public override void OnAwake()
    {
        waypoints = waypointsHolder.transform.GetComponentsInChildren<Transform>();
    }
    public override void OnStart()
    {
        if(currentWaypointIndex >= waypoints.Length - 1)
        {
            currentWaypointIndex = 0;
        } else
        {
            currentWaypointIndex++;
        }
    }

    public override TaskStatus OnUpdate()
    {
        Transform newWaypoint = waypoints[currentWaypointIndex];
        if(newWaypoint != null)
        {
            target.Value = newWaypoint.position;
            return TaskStatus.Success;
        }
        return TaskStatus.Failure;  
    }
}
