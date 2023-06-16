using MathNet.Numerics;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MinigameManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private float timer;
    [SerializeField] private TMP_Text timerText, scoreTextHelper, finalText, returnText;
    [SerializeField] private List<GameObject> disableList;
    [SerializeField] private GameObject panel;
    private static TMP_Text scoreText;
    private static int score;
    private static bool endStage;
    private LoadingScreenTrigger loadingScreenTrigger;


    void Start()
    {
        loadingScreenTrigger = GetComponent<LoadingScreenTrigger>();
        scoreText = scoreTextHelper;
        timerText.text = $"Time: {timer}";
        scoreText.text = $"Score: {score}";
        endStage = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!endStage)
        {
            timer -= Time.deltaTime;
            timerText.text = $"Time: {timer.Round(0)}";
            if (timer <= 0)
            {
                timer = 0;
                StopStage();
            }
        }
    }

    private void StopStage()
    {
        endStage = true;
        foreach (GameObject go in disableList)
        {
            go.SetActive(false);
        }
        timerText.text = "";
        scoreText.text = "";
        panel.SetActive(true);
        finalText.text = $"Time's Up!\nFinal Score: {score}";
        StartCoroutine(ReturnTimer());
    }
    public static void AddScore(int num)
    {
        if (!endStage)
        {
            score += num;
            scoreText.text = $"Score: {score}";
        }
    }

    private IEnumerator ReturnTimer()
    {
        int i = 5;
        while (i > 0)
        {
            yield return new WaitForSeconds(1f);
            i--;
            returnText.text = $"Returning in {i}...";
        }
        loadingScreenTrigger.LoadLoadingScreen("SampleScene");
    }
}
