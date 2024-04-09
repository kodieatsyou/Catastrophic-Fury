using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;

public class ListenForSoundQueues : Action
{

    public AIStats stats;
    public SharedVector3 target;

    public override void OnStart()
    {
        base.OnStart();
    }

    public override TaskStatus OnUpdate()
    {
        Collider[] heardObjects = Physics.OverlapSphere(new Vector3(transform.position.x, transform.position.y + stats.headYOffset, transform.position.z), stats.currentHearingRadius, stats.objectSoundLayers);
        if (heardObjects.Length > 0)
        {
            PrintParents(heardObjects[0].transform);
            if (heardObjects[0].transform.position != target.Value)
            {
                Debug.Log($"{heardObjects[0].name} at {heardObjects[0].transform.position}");
                target.Value = heardObjects[0].transform.position;
                return TaskStatus.Success;
            } 
        }
        return TaskStatus.Running;
    }

    public override void OnEnd()
    {
        base.OnEnd();
    }

    void PrintParents(Transform obj)
    {
        Transform parent = obj.parent;
        while (parent != null)
        {
            Debug.Log(parent.name);
            parent = parent.parent;
        }
    }
}
