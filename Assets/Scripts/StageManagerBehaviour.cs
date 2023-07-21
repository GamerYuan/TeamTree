using System;
using System.Collections;
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
    [SerializeField] private GameEvent onTrimStart;
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
        
        
    }
    void Start()
    {
        updateCount = SaveData.updateCount;
        RaiseUpdateChange();
        SetUpdateIteration(SaveData.loginEpochTime);
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
        RaiseUpdateChange();
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
        RaiseUpdateChange();
        Debug.Log("Update Count = " + updateCount);
    }
    public void SetUpdateIteration(long lastLoginEpoch)
    {
        DateTime currTime = DateTime.UtcNow;
        double timeDiff = currTime.Subtract(DateTimeOffset.FromUnixTimeSeconds(lastLoginEpoch).UtcDateTime).TotalMinutes;
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

    public void StartTrim()
    {
        if (currTree == null)
        {
            currTree = GameObject.FindGameObjectWithTag("Tree");
        }
        currTree.GetComponent<Bonsai>().ToggleScissors();
        StopTime();
        onTrimStart.Raise(this, false);
    }

    public void EndTrim()
    {
        if (currTree == null)
        {
            currTree = GameObject.FindGameObjectWithTag("Tree");
        }
        currTree.GetComponent<Bonsai>().ToggleScissors();
        StartTime();
        onTrimStart.Raise(this, true);
    }

    private void RaiseUpdateChange()
    {
        SaveData.SetUpdate(updateCount);
        onUpdateChanged.Raise(this, updateCount);
    }

    public void ChangeLoadState(Component sender, object data)
    {
        if (data is bool)
        {
            Debug.Log("Outside update raise");
            if ((bool) data)
            {
                Debug.Log("Update Raise called");
                RaiseUpdateChange();
            }
        }
    }
}