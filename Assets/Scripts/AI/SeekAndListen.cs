using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;
using UnityStandardAssets.Characters.ThirdPerson;
using UnityEngine.AI;
using Unity.VisualScripting.Antlr3.Runtime.Misc;

public class SeekAndListen : Action
{
   
    Animator animator;
    public SharedVector3 target;
    public SharedBool canSeePlayer;
    public AIStats stats;
    public Transform player;
    ThirdPersonCharacter character;
    AudioSource audioSource;
    NavMeshAgent agent;

    public override void OnStart()
    {
        animator = GetComponent<Animator>();
        character = GetComponent<ThirdPersonCharacter>();
        agent = GetComponent<NavMeshAgent>();
        audioSource = GetComponent<AudioSource>();
        agent.isStopped = false;
        agent.SetDestination(target.Value);
    }

    public override TaskStatus OnUpdate()
    {
        Collider[] heardObjects;
        if (stats.angerLevel > 50)
        {
            //heardObjects = Physics.OverlapSphere(new Vector3(transform.position.x, transform.position.y + stats.headRadiusYOffset, transform.position.z), stats.roamHearingRadius, stats.canHearWhileInvestigating);
        } else
        {
            //heardObjects = Physics.OverlapSphere(new Vector3(transform.position.x, transform.position.y + stats.headRadiusYOffset, transform.position.z), stats.roamHearingRadius, stats.canHearWhileRoaming);
        }
         
        /*if (heardObjects.Length > 0)
        {
            agent.isStopped = true;
            character.Move(Vector3.zero, false, false);
            animator.SetBool("Moving", false);
            transform.LookAt(heardObjects[0].transform.position);
            target.Value = heardObjects[0].transform.position;
            animator.SetTrigger("Startle");
            audioSource.PlayOneShot(stats.startleSound);
            return TaskStatus.Failure;
        }*/
        if (agent.remainingDistance > agent.stoppingDistance)
        {
            animator.SetBool("Moving", true);
            character.Move(agent.desiredVelocity * stats.roamSpeedFactor, false, false);
            return TaskStatus.Running;
        } else
        {
            return TaskStatus.Success;
        }
    }

    public override void OnEnd()
    {
        agent.isStopped = true;
        character.Move(Vector3.zero, false, false);
    }
}
