using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using BehaviorDesigner.Runtime;

public class CheckForPlayerInSightTwo : Action
{
    public AIStats stats;
    public Transform player;
    public SharedVector3 target;

    public override TaskStatus OnUpdate()
    {
        if (PlayerWithinSight())
        {
            target.Value = player.transform.position;
            return TaskStatus.Success;
        }
        else
        {
            return TaskStatus.Failure;
        }
    }

    public bool PlayerWithinSight()
    {
        Vector3 eyelevel = new Vector3(transform.position.x, transform.position.y + stats.headYOffset, transform.position.z);
        Vector3 direction = player.transform.position - eyelevel;
        float angle = Vector3.Angle(direction, transform.forward);

        // Check if player is within field of view angle
        if (angle < stats.currentFieldOfView)
        {
            RaycastHit hit;
            // Cast a ray from the enemy to the player
            if (Physics.Raycast(eyelevel, direction, out hit, stats.currentFieldOfViewDistance))
            {
                // If the object hit is the player, there is a direct line of sight
                if (hit.transform == player)
                {
                    Debug.DrawLine(eyelevel, player.transform.position, Color.green);
                    return true;
                }
                else
                {
                    Debug.DrawLine(eyelevel, direction * 100f, Color.blue);
                }
            }
        }


        Debug.DrawLine(transform.position, transform.position + transform.forward * stats.currentFieldOfViewDistance, Color.red);

        return false;
    }
}