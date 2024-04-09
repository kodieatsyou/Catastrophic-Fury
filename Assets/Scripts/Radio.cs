using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Radio : MonoBehaviour
{

    public AudioClip[] songs;

    public GameObject musicNoteParticles;

    public AudioClip recordScratch;
    public AudioClip turnOn;
    public AudioClip turnOff;

    private AudioSource source;
    private int currentSongIndex;


    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource>();
        currentSongIndex = Random.Range(0, songs.Length);
        source.clip = songs[currentSongIndex];
        source.Play();
    }

    public void NextSong()
    {
        StartCoroutine(ChangeToNextSong());
    }

    public void PreviousSong()
    {
        StartCoroutine(ChangeToPreviousSong());
    }

    public void TurnOff()
    {
        StopAllCoroutines();
        source.Stop();
        musicNoteParticles.GetComponent<ParticleSystem>().Stop();
    }

    IEnumerator ChangeToNextSong()
    {
        if (currentSongIndex == songs.Length - 1)
        {
            source.clip = turnOff;
            source.loop = false;
            source.Play();
            musicNoteParticles.SetActive(false);
            source.loop = true;
            yield return new WaitForSeconds(1);
            currentSongIndex = -1;
            source.Stop();
        }
        else if (currentSongIndex == -1)
        {
            source.clip = turnOn;
            source.loop = false;
            source.Play();
            yield return new WaitForSeconds(1);
            source.loop = true;
            musicNoteParticles.SetActive(true);
            currentSongIndex = 0;
            source.clip = songs[currentSongIndex];
            source.Play();
        }
        else
        {
            currentSongIndex++;
            source.clip = recordScratch;
            source.loop = false;
            source.Play();
            yield return new WaitForSeconds(1);
            source.loop = true;
            source.clip = songs[currentSongIndex];
            source.Play();
        }
    }

    IEnumerator ChangeToPreviousSong()
    {
        if (currentSongIndex == 0)
        {
            source.clip = turnOff;
            source.loop = false;
            source.Play();
            musicNoteParticles.SetActive(false);
            yield return new WaitForSeconds(1);
            source.loop = true;
            currentSongIndex = -1;
            source.Stop();

        }
        else if (currentSongIndex == -1)
        {
            source.clip = turnOn;
            source.loop = false;
            source.Play();
            yield return new WaitForSeconds(1);
            source.loop = true;
            musicNoteParticles.SetActive(true);
            currentSongIndex = songs.Length - 1;
            source.clip = songs[currentSongIndex];
            source.Play();
        }
        else
        {
            currentSongIndex--;
            source.clip = recordScratch;
            source.loop = false;
            source.Play();
            yield return new WaitForSeconds(1);
            source.loop = true;
            source.clip = songs[currentSongIndex];
            source.Play();
        }
    }
}
