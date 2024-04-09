using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public AudioSource sfxAudioSource;
    public AudioSource musicAudioSource;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        } else
        {
            Destroy(instance);
            instance = this;
        }
    }

    public void PlayOneShot(AudioClip clip)
    {
        sfxAudioSource.PlayOneShot(clip);
    }
}
