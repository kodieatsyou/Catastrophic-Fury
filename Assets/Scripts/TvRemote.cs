using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Video;

public class TvRemote : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera tvCam;
    [SerializeField] private GameObject tvScreen;
    private VideoPlayer tvVideoPlayer;
    private bool isOn = false;

    private void OnEnable()
    {
        CameraSwitcher.RegisterVirtualCamera(tvCam);
    }

    private void OnDisable()
    {
        CameraSwitcher.UnRegisterVirtualCamera(tvCam);
    }

    private void Start()
    {
        tvVideoPlayer = tvScreen.GetComponent<VideoPlayer>();
        tvScreen.SetActive(false);
    }

    public void ToggleTV()
    {
        if(isOn)
        {
            tvScreen.SetActive(false);
            tvVideoPlayer.Stop();
            isOn = false;
        } else
        {
            CameraSwitcher.SwitchToVirtualCamera(tvCam);
            tvScreen.SetActive(true);
            tvVideoPlayer.Play();
            isOn = true;
        }
    }

}
