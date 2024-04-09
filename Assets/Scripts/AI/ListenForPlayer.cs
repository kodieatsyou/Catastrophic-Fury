using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;

public class ListenForPlayer : Action
{

    public AIStats stats;

    public override void OnStart()
    {
        base.OnStart();
    }

    public override TaskStatus OnUpdate()
    {
        Collider[] heardObjects = Physics.OverlapSphere(new Vector3(transform.position.x, transform.position.y + stats.headYOffset, transform.position.z), stats.currentHearingRadius, stats.playerSoundLayers);
        if(heardObjects.Length > 0 )
        {
            return TaskStatus.Success;
        } else
        {
            return TaskStatus.Running;
        }
    }

    public override void OnEnd()
    {
        base.OnEnd();
    }
}
