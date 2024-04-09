using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public static class CameraSwitcher
{
    static List<CinemachineVirtualCamera> virtualCameras = new List<CinemachineVirtualCamera>();
    public static CinemachineVirtualCamera activeCamera;

    public static bool IsActiveCamera(CinemachineVirtualCamera virtualCamera)
    {
        return virtualCamera == activeCamera;
    }

    public static void SwitchToVirtualCamera(CinemachineVirtualCamera virtualCam)
    {
        virtualCam.Priority = 10;
        activeCamera = virtualCam;

        foreach(CinemachineVirtualCamera cam in virtualCameras)
        {
            if(cam != activeCamera && cam.Priority != 0)
            {
                cam.Priority = 0;
            }
        }
    }

    public static void RegisterVirtualCamera(CinemachineVirtualCamera virtualCamera)
    {
        virtualCameras.Add(virtualCamera);
    }

    public static void UnRegisterVirtualCamera(CinemachineVirtualCamera virtualCamera)
    {
        virtualCameras.Remove(virtualCamera);
    }
}
