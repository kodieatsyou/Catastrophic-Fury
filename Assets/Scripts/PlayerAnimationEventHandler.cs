using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationEventHandler : MonoBehaviour
{
    public PlayerController controller;
    
    public void Meow()
    {
        controller.MakeMeowSoundQueue();
    }

    public void FootStep()
    {
        controller.MakeFootstepSoundQueue();
    }

    public void Land()
    {
        controller.MakeLandSoundQueue();
    }
}
