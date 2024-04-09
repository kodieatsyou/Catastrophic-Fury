using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;

public class IsMad : Action
{
    public AIStats stats;
    public override TaskStatus OnUpdate()
    {
        if (stats.angerLevel >= 6)
        {
            return TaskStatus.Success;
        }
        else
        {
            return TaskStatus.Failure;
        }
    }
}
