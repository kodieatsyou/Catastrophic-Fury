using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableObject : MonoBehaviour
{
    [SerializeField] private GameObject[] pieces;
    [SerializeField] private int soundLayer;
    [SerializeField] private float breakNoiseSize;
    [SerializeField] private AudioClip breakSound;
    [SerializeField] private float despawnTime;
    [SerializeField] private int value;
    [SerializeField] private string objectName;

    public void BreakObject()
    {
        GetComponent<MeshCollider>().enabled = false;
        GetComponent<MeshRenderer>().enabled = false;
        foreach (GameObject piece in pieces)
        {
            piece.SetActive(true);
        }
        GameObject soundQueue = Instantiate(GameAssets.i.soundQueue);
        Debug.Log(soundQueue.name);
        soundQueue.transform.position = transform.position;
        soundQueue.layer = soundLayer;
        soundQueue.GetComponent<SoundQueue>().PlaySoundQueue(breakSound, breakNoiseSize);
        GameManager.Instance.AddDestroyedObject(objectName, value);
        UIManager.Instance.UpdateBrokenMoneyAmount();
    }
}
