using System;
using UnityEngine;
public class StageManagerBehaviour : MonoBehaviour
{
    public static StageManagerBehaviour instance;

    public static bool isPaused;
    private int updateCount;

    [SerializeField] private GameObject minigameMenu, baseCanvas, flowerPot, treePrefab;
    [SerializeField] private float updatePeriod;
    private LoadingScreenTrigger loadScreenTrigger;
    private GameObject currTree;
    [Header("Events")]
    [SerializeField] private GameEvent onUpdateChanged;
    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
        //currTree = GameObject.FindGameObjectWithTag("Tree");
        isPaused = false;
        loadScreenTrigger = GetComponent<LoadingScreenTrigger>();
        updateCount = 0;
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
    public void LoadStage(string stageToLoad)
    {
        loadScreenTrigger.LoadLoadingScreen(stageToLoad);
    }
    public void ResetTree()
    {
        if (currTree == null)
        {
            currTree = GameObject.FindGameObjectWithTag("Tree");
        }
        currTree.GetComponent<Bonsai>().InitTree();
        updateCount = 0;
        RandomEventManager.instance.ResetTutProgress();
        onUpdateChanged.Raise(this, updateCount);
        Debug.Log("Update Count = " + updateCount);
    }
    public void UpdateTree()
    {
        if (currTree == null)
        {
            currTree = GameObject.FindGameObjectWithTag("Tree");
        }
        WaterTree();
        currTree.GetComponent<Bonsai>().TreeUpdate();
        updateCount += 1;
        onUpdateChanged.Raise(this, updateCount);
        Debug.Log("Update Count = " + updateCount);
    }
    public void SetUpdateCount(int updateCount)
    {
        this.updateCount = updateCount;
        onUpdateChanged.Raise(this, updateCount);
    }
    public int GetUpdateCount() { return updateCount; }
    public void SetUpdateIteration(long lastLoginEpoch)
    {
        DateTime currTime = DateTime.Now;
        double timeDiff = currTime.Subtract(DateTimeOffset.FromUnixTimeSeconds(lastLoginEpoch).LocalDateTime).TotalMinutes;
        int updateIteration = (int)Math.Floor(timeDiff / updatePeriod);
        Debug.Log($"Time diff from last login: {timeDiff}, update {updateIteration} times");
        for (int i = 0; i < updateIteration; i++)
        {
            UpdateTree();
        }
    }
    private void WaterTree()
    {
        float water = FlowerPotBehaviour.instance.GetWater();
        if (water >= 0.5f)
        {
            Debug.Log("Watering Tree!");
            float decAmount = water * 0.5f;
            FlowerPotBehaviour.instance.DecreaseWater(decAmount);
            if (currTree == null)
            {
                currTree = GameObject.FindGameObjectWithTag("Tree");
            }
            currTree.GetComponent<Bonsai>().WaterTree(decAmount * 0.90f);
        }
    }
}