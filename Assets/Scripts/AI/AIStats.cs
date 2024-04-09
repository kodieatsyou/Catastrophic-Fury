using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class AIStats : ScriptableObject
{

    [HideInInspector] public float currentSpeedFactor;
    [HideInInspector] public float currentFieldOfView;
    [HideInInspector] public float currentFieldOfViewDistance;
    [HideInInspector] public float currentHearingRadius;

    public float roamSpeedFactor;
    public float investigateSpeedFactor;
    public float chaseSpeedFactor;

    public float happy_fieldOfViewAngle;
    public float happy_fieldOfViewDistance;

    public float angy_fieldOfViewAngle;
    public float angy_fieldOfViewDistance;

    public float mad_fieldOfViewAngle;
    public float mad_fieldOfViewDistance;

    public float happy_hearingRadius;
    public float angy_hearingRadius;
    public float mad_hearingRadius;

    public float headYOffset;

    public float angerLevel;


    public LayerMask canHearWhileRoaming;
    public LayerMask canHearWhileInvestigating;
    public LayerMask playerSoundLayers;
    public LayerMask objectSoundLayers;
    public LayerMask visionBlockedLayers;

    public AudioClip startleSound;
    public AudioClip investigateSound;
    public AudioClip playerSawSound;
    public AudioClip lostPlayerSound;
    public AudioClip caughtPlayerSound;
    public AudioClip investigatedSound;
}
