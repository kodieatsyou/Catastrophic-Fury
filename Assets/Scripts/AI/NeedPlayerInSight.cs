using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;

public class NeedPlayerInSight : Action
{
    AudioSource aSource;
    public AIStats stats;
    public SharedVector3 target;
    public Transform player;
    public SharedBool seesPlayer;
    public float persuitWithoutSightOrSoundTime;
    public float maxDistanceAroundLastSeenCanLook;
    float timer;

    public override void OnStart()
    {
        timer = 0f;
        aSource = GetComponent<AudioSource>();
    }
    public override TaskStatus OnUpdate()
    {
        Collider[] heardObjects = Physics.OverlapSphere(new Vector3(transform.position.x, transform.position.y + stats.headYOffset, transform.position.z), stats.currentHearingRadius, stats.playerSoundLayers);
        if (heardObjects.Length > 0 || PlayerWithinSight())
        {
            target.Value = player.transform.position;
            Vector3 direction = player.transform.position - transform.position;
            direction.y = 0f;

            if (direction != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(direction);
            }

            seesPlayer.Value = true;
            return TaskStatus.Running;
        }
        else
        {
            if (timer < persuitWithoutSightOrSoundTime)
            {
                seesPlayer.Value = false;
                timer += Time.time;
                return TaskStatus.Running;
            }
            seesPlayer.Value = false;
            aSource.PlayOneShot(stats.lostPlayerSound);
            stats.angerLevel++;
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
            if (Physics.Raycast(eyelevel, direction, out hit, stats.currentFieldOfViewDistance, stats.visionBlockedLayers))
            {
                // If the object hit is the player, there is a direct line of sight
                if (hit.transform == player)
                {
                    Debug.DrawLine(eyelevel, player.transform.position, Color.green);
                    return true;
                }
                else
                {
                    Debug.Log($"HIT: {hit.transform.name} instead of player");
                    Debug.DrawLine(eyelevel, direction * 100f, Color.blue);
                }
            }
        } else
        {
            Debug.DrawLine(transform.position, transform.position + transform.forward * stats.currentFieldOfViewDistance, Color.red);
        }


        

        return false;
    }
}
