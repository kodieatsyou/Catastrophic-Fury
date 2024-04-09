using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using UnityStandardAssets.Characters.ThirdPerson;
using UnityEngine.AI;

public class StopMoving : Action
{
    ThirdPersonCharacter character;
    NavMeshAgent agent;
    Animator animator;

    public override void OnStart()
    {
        character = GetComponent<ThirdPersonCharacter>();
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        character.Move(Vector3.zero, false, false);
        animator.SetBool("Moving", false);
    }
}
