using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundQueue : MonoBehaviour
{

    public void PlaySoundQueue(AudioClip soundToPlay, float soundSize)
    {
        StartCoroutine(MakeSoundQueue(soundToPlay, soundSize));
    }
    private IEnumerator MakeSoundQueue(AudioClip soundToPlay, float soundSize)
    {
        SphereCollider sc = GetComponent<SphereCollider>();
        sc.enabled = true;
        sc.radius = soundSize;
        AudioSource source = GetComponent<AudioSource>();
        source.clip = soundToPlay;
        source.Play();
        yield return new WaitForSeconds(soundToPlay.length);
        Destroy(gameObject);
    }
}
