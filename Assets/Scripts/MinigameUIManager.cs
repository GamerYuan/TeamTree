using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class MinigameUIManager : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreText, timerText, returnText, returnTimer;
    [SerializeField] private GameObject returnPanel;

    public static MinigameUIManager instance;

    void Awake()
    {
        instance = this;
        scoreText.text = $"Score: 0";
        timerText.text = "";
    }

    public void ScoreChange(Component sender, object data)
    {
        if (scoreText != null && data is int)
        {
            scoreText.text = $"Score: {(int)data}";
        }
    }

    public void TimerChange(Component sender, object data)
    {
        if (timerText != null && data is float)
        {
            timerText.text = $"Time: {Mathf.RoundToInt((float)data)}";
        }
    }

    public void ReturnTimerChange(Component sender, object data)
    {
        if (returnText != null && data is int)
        {
            returnTimer.text = $"Returning in {(int)data}...";
        }
    }

    public void StopStage(Component sender, object data)
    {
        if (sender is GlobalMinigameManager && data is int)
        {
            returnPanel.SetActive(true);
            timerText.text = "";
            scoreText.text = "";
            returnText.text = $"Time's Up!\nFinal Score: {(int)data}";
        }
    }
}
