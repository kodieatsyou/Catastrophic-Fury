using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;
using UnityEngine.AI;
using UnityStandardAssets.Characters.ThirdPerson;

public class Investigate : Action
{
    Animator animator;
    ThirdPersonCharacter character;
    NavMeshAgent agent;
    AudioSource aSource;
    public AIStats stats;
    public Transform player;
    public SharedVector3 target;


    public override void OnStart()
    {
        aSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        character = GetComponent<ThirdPersonCharacter>();
        agent = GetComponent<NavMeshAgent>();
        agent.isStopped = false;
        agent.SetDestination(target.Value);
        aSource.PlayOneShot(stats.investigateSound);
    }
    public override TaskStatus OnUpdate()
    {
        if (agent.remainingDistance > agent.stoppingDistance)
        {
            animator.SetBool("Moving", true);
            character.Move(agent.desiredVelocity * stats.investigateSpeedFactor, false, false);
            return TaskStatus.Running;
        }
        else
        {
            aSource.PlayOneShot(stats.investigatedSound);
            animator.SetBool("Moving", false);
            character.Move(Vector3.zero, false, false);
            animator.SetTrigger("LookAround");
            stats.angerLevel++;
            GameManager.Instance.UpdateRage();
            return TaskStatus.Success;
        }
    }
}
