using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    DefaultControls controls;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        } else
        {
            Destroy(Instance);
            Instance = this;
        }
        
        controls = new DefaultControls();

    }

    public AIStats[] aiStats;

    public int moneyBroken;
    public float humanRage;
    public bool isPlaying;
    public List<string> objectsDestroyedNames;
    public List<int> objectsDestroyedCost;
    public CinemachineVirtualCamera mainMenuCam;
    public CinemachineVirtualCamera startCam;
    public CinemachineDollyCart mainCamDolly;
    public Animator playerAnim;
    public GameObject cameraColliderHolder;

    private void OnEnable()
    {
        controls.Enable();
        controls.Player.Pause.performed += OnPause;
        CameraSwitcher.RegisterVirtualCamera(mainMenuCam);
        CameraSwitcher.RegisterVirtualCamera(startCam);
    }

    private void OnPause(InputAction.CallbackContext context)
    {
        UIManager.Instance.TogglePauseScreen();
    }

    private void OnDisable()
    {
        controls.Player.Pause.performed -= OnPause;
        CameraSwitcher.UnRegisterVirtualCamera(mainMenuCam);
        CameraSwitcher.UnRegisterVirtualCamera(startCam);
    }

    // Start is called before the first frame update
    void Start()
    {
        moneyBroken = 0;
        humanRage = 0;
        isPlaying = false;
        PlayerController.instance.enabled = false;
        cameraColliderHolder.SetActive(false);
        UIManager.Instance.ShowMainMenu();
    }

    private void Update()
    {
        if (!isPlaying)
        {
            if(mainCamDolly.m_Position >= 14.96342f)
            {
                mainCamDolly.m_Position = 0;
            }
        }
    }

    public void StartGame()
    {
        PlayerController.instance.enabled = true;
        cameraColliderHolder.SetActive(true);
        playerAnim.SetTrigger("WakeUp");
        CameraSwitcher.SwitchToVirtualCamera(startCam);
        foreach (AIStats stats in aiStats)
        {
            stats.angerLevel = 0;
        }
        UpdateRage();
    }

    public void ResetGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void GameOver()
    {
        UIManager.Instance.ShowGameOver();
        isPlaying = false;
        PlayerController.instance.enabled = false;
        cameraColliderHolder.SetActive(false);
    }

    public void UpdateRage()
    {
        float total = 0f;
        foreach(AIStats stats in aiStats)
        {
            total += stats.angerLevel;
        }
        Debug.Log($"Total rage before division{total}");
        humanRage = Mathf.Floor(total / (float)aiStats.Length);
        Debug.Log($"Total ais{(float)aiStats.Length}");
        UIManager.Instance.SetAngerMeterAmount(humanRage);
    }

    public void AddDestroyedObject(string name, int value)
    {
        objectsDestroyedNames.Add(name);
        objectsDestroyedCost.Add(value);
        moneyBroken += value;
    }

    public void Test()
    {
        GameOver();
    }

    public void OnQuit()
    {
        Application.Quit();
    }
}
