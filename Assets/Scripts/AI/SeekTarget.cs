using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;
using UnityStandardAssets.Characters.ThirdPerson;
using UnityEngine.AI;

public class SeekTarget : Action
{
   
    Animator animator;
    public SharedVector3 target;
    public AIStats stats;
    ThirdPersonCharacter character;
    NavMeshAgent agent;

    public override void OnStart()
    {
        animator = GetComponent<Animator>();
        character = GetComponent<ThirdPersonCharacter>();
        agent = GetComponent<NavMeshAgent>();
        agent.isStopped = false;
        agent.SetDestination(target.Value);
    }

    public override TaskStatus OnUpdate()
    {
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
