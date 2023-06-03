using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreTextHelper;
    private static TMP_Text scoreText;
    private static int score = 0;

    public static ScoreManager instance;

    private void Awake()
    {
        scoreText = scoreTextHelper;
        instance = this; 
    }

    public static void AddScore(int num)
    {
        score += num;
        scoreText.text = $"Score: {score}";
    }
}
