using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;

public class SetStats : Action
{
    public AIStats stats;

    public override void OnStart()
    {
        if(stats.angerLevel >= 4 && stats.angerLevel < 6)
        {
            stats.currentFieldOfView = stats.angy_fieldOfViewAngle;
            stats.currentFieldOfViewDistance = stats.angy_fieldOfViewDistance;
            stats.currentHearingRadius = stats.angy_hearingRadius;
        } else if(stats.angerLevel < 4)
        {
            stats.currentFieldOfView = stats.happy_fieldOfViewAngle;
            stats.currentFieldOfViewDistance = stats.happy_fieldOfViewDistance;
            stats.currentHearingRadius = stats.happy_hearingRadius;
        } else
        {
            stats.currentFieldOfView = stats.mad_fieldOfViewAngle;
            stats.currentFieldOfViewDistance = stats.mad_fieldOfViewDistance;
            stats.currentHearingRadius = stats.mad_hearingRadius;
        }
    }

    public override TaskStatus OnUpdate()
    {
        return TaskStatus.Success;
    }
}
