using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManagerBehaviour : MonoBehaviour
{
    public static bool isPaused;
    
    [SerializeField] private GameObject minigameMenu, baseCanvas;
    void Awake()
    {
        isPaused = false;
    }

    public static void StopTime()
    {
        isPaused = true;
        Time.timeScale = 0f;
    }
    public static void StartTime()
    {
        Time.timeScale = 1f;
        isPaused = false;
    }
    public void CallMinigameMenu()
    {
        minigameMenu.SetActive(true);
        baseCanvas.SetActive(false);
        StopTime();
    }
    public void LeaveMinigameMenu()
    {
        minigameMenu.SetActive(false);
        baseCanvas.SetActive(true);
        StartTime();
    }
}
