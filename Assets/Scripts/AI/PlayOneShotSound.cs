using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;

public class PlayOneShotSound : Action
{
    public AudioClip clip;
    AudioSource source;

    public override void OnStart()
    {
        source = GetComponent<AudioSource>();
        source.PlayOneShot(clip);
    }

    public override TaskStatus OnUpdate()
    {
        return TaskStatus.Success;
    }
}
