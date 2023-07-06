using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManagerBehaviour : MonoBehaviour
{
    public static StageManagerBehaviour instance;
    
    public static bool isPaused;
    private int updateCount;
    
    [SerializeField] private GameObject minigameMenu, baseCanvas, flowerPot, treePrefab;
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
        StartCoroutine(WaterTree());
        isPaused = false;
        loadScreenTrigger= GetComponent<LoadingScreenTrigger>();
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
        onUpdateChanged.Raise(this, updateCount);
        Debug.Log("Update Count = " + updateCount);
    }

    public void UpdateTree()
    {
        if (currTree == null)
        {
            currTree = GameObject.FindGameObjectWithTag("Tree");
        }
        Bonsai bonsai = currTree.GetComponent<Bonsai>();
        bonsai.TreeUpdate();
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

    // Let tree suck water and update every 5s
    private IEnumerator WaterTree()
    {
        while (true)
        {
            yield return new WaitForSeconds(5f);
            float water = FlowerPotBehaviour.instance.GetWater();
            if (water >= 0.5f)
            {
                Debug.Log("Watering Tree!");
                float decAmount = water * 0.7f;
                FlowerPotBehaviour.instance.DecreaseWater(decAmount);
                if (currTree == null)
                {
                    currTree = GameObject.FindGameObjectWithTag("Tree");
                }
                Bonsai bonsai = currTree.GetComponent<Bonsai>();
                bonsai.WaterTree(decAmount * 0.95f);
            }
            //UpdateTree();
        }
    }
}
