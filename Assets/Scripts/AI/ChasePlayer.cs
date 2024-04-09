using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;
using UnityEngine.AI;
using UnityStandardAssets.Characters.ThirdPerson;

public class ChasePlayer : Action
{
    Animator animator;
    ThirdPersonCharacter character;
    NavMeshAgent agent;
    AudioSource aSource;
    public SharedBool seesPlayer;
    public Transform player;
    public AIStats stats;


    public override void OnStart()
    {
        aSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        character = GetComponent<ThirdPersonCharacter>();
        agent = GetComponent<NavMeshAgent>();
        agent.isStopped = false;
        aSource.PlayOneShot(stats.playerSawSound);
    }
    public override TaskStatus OnUpdate()
    {
        if (agent.remainingDistance > 0.2f)
        {
            agent.SetDestination(player.transform.position);
            animator.SetBool("Moving", true);
            character.Move(agent.desiredVelocity * stats.chaseSpeedFactor, false, false);
            return TaskStatus.Running;
        }
        else
        {

            if(seesPlayer.Value == true)
            {
                animator.SetTrigger("CatchPlayer");
                player.gameObject.GetComponent<PlayerController>().enabled = false;
                aSource.PlayOneShot(stats.caughtPlayerSound);
                StartCoroutine(WaitForGOScreen());
                return TaskStatus.Success;
            } else
            {
                return TaskStatus.Failure;
            }

        }
    }

    IEnumerator WaitForGOScreen()
    {
        yield return new WaitForSeconds(1);
        GameManager.Instance.GameOver();
    }
}
