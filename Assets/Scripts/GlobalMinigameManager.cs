using MathNet.Numerics;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GlobalMinigameManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private float timer;
    [SerializeField] private List<GameObject> disableList;
    [SerializeField] private int scoreMultiplier;    private static int score;
    private static bool endStage;
    private LoadingScreenTrigger loadingScreenTrigger;
    private Coroutine timerCoroutine;

    [Header("Events")]
    [SerializeField] private GameEvent onScoreChange;
    [SerializeField] private GameEvent onTimerChange, onStageEnd, onReturnTimerChange;

    protected virtual void Awake()
    {
        score = 0;
        loadingScreenTrigger = GetComponent<LoadingScreenTrigger>();
        endStage = false;
        StartStage();
    }

    // Update is called once per frame

    protected void StartStage()
    {
        if (timer > 0)
        {
            timerCoroutine = StartCoroutine(Timer());
        }
    }

    protected void StopStage()
    {
        endStage = true;
        foreach (GameObject go in disableList)
        {
            if (go != null)
            {
                go.SetActive(false);
            }
        }
        onStageEnd.Raise(this, score);
        if (timerCoroutine != null)
        {
            StopCoroutine(timerCoroutine);
        }
        CoinManager.instance.AddCoins(CoinManager.instance.CalculateCoins(score, scoreMultiplier));
        StartCoroutine(ReturnTimer());
    }
    public void AddScore(Component sender, object data)
    {
        if (!endStage && data is int)
        {
            score += (int) data;
            onScoreChange.Raise(this, score);
        }
    }

    public void Punishment(Component sender, object data)
    {
        Debug.Log("Punishment");
        if (data is float)
        {
            score = Mathf.RoundToInt(score * (float)data);
            onScoreChange.Raise(this, score);
        }
    }

    private IEnumerator ReturnTimer()
    {
        int i = 5;
        while (i > 0)
        {
            onReturnTimerChange.Raise(this, i);
            yield return new WaitForSeconds(1f);
            --i;
        }
        loadingScreenTrigger.LoadLoadingScreen("SampleScene");
    }

    private IEnumerator Timer()
    {
        while(!endStage)
        {
            onTimerChange.Raise(this, timer);
            yield return new WaitForSeconds(1f);
            --timer;
            if (timer == 0)
            {
                StopStage();
            }
        }
    }
}
