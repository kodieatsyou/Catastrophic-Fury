using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Characters.ThirdPerson;

public class Human : MonoBehaviour
{
    [Header("Sounds")]
    [SerializeField] private AudioClip huhSound;
    [SerializeField] private AudioClip hmmSound;

    [Header("Hearing")]
    [SerializeField] private float hearingRadius;

    private NavMeshAgent agent;
    private Animator animator;
    private AudioSource aSource;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        aSource = GetComponent<AudioSource>();
        agent.updateRotation = false;
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetBool("isMoving", !agent.isStopped);
    }
}
