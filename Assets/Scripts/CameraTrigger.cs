using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraTrigger : MonoBehaviour
{

    [SerializeField] private CinemachineVirtualCamera vCamera;

    private void OnEnable()
    {
        CameraSwitcher.RegisterVirtualCamera(vCamera);
    }

    private void OnDisable()
    {
        CameraSwitcher.UnRegisterVirtualCamera(vCamera);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            if(CameraSwitcher.activeCamera != vCamera)
            {
                CameraSwitcher.SwitchToVirtualCamera(vCamera);
            }
        }
    }
}
