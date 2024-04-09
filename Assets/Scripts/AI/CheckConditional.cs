using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;

public class CheckConditional : Action
{

    public SharedBool sawPlayer;

    public override TaskStatus OnUpdate()
    {
        if(sawPlayer.Value) 
        {
            return TaskStatus.Failure;
        } else
        {
            return TaskStatus.Success;
        }
    }
}
