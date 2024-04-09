using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{

    public UnityEvent interactLeft;
    public UnityEvent interactRight;
    public UnityEvent hitFloor;

    public void InteractLeft() 
    {
        interactLeft.Invoke();
    }

    public void InteractRight()
    {
        interactRight.Invoke();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.tag == "Floor")
        {
            hitFloor.Invoke();
        }
    }
}
