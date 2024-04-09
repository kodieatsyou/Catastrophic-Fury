using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class FramedObject : MonoBehaviour
{
    public CinemachineVirtualCamera framedCamera;

    private void OnEnable()
    {
        CameraSwitcher.RegisterVirtualCamera(framedCamera);
    }

    private void OnDisable()
    {
        CameraSwitcher.UnRegisterVirtualCamera(framedCamera);
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "Player")
        {
            Debug.Log("Player enterd");
            CameraSwitcher.SwitchToVirtualCamera(framedCamera);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            Debug.Log("Player exit");
            PlayerController.instance.SwitchToMainCam();
        }
    }
}
