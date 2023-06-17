using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManagerBehaviour : MonoBehaviour
{
    public static bool isPaused;
    
    [SerializeField] private GameObject minigameMenu, baseCanvas, flowerPot, treePrefab;
    private LoadingScreenTrigger loadScreenTrigger;
    private GameObject currTree;
    void Awake()
    {
        currTree = GameObject.FindGameObjectWithTag("Tree");
        StartCoroutine(WaterTree());
        isPaused = false;
        loadScreenTrigger= GetComponent<LoadingScreenTrigger>();
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
        Vector3 treePos = currTree.transform.localPosition;
        Vector3 treeRot = currTree.transform.localEulerAngles;
        Destroy(currTree);
        GameObject nextTree = Instantiate(treePrefab, flowerPot.transform);
        nextTree.transform.localPosition = treePos;
        nextTree.transform.localEulerAngles = treeRot;
        nextTree.GetComponent<Bonsai>().lsystem.InitAxiom();
        currTree = nextTree;
    }

    public void UpdateTree()
    {
        Bonsai bonsai = currTree.GetComponent<Bonsai>();
        bonsai.TreeUpdate();
    }

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
            UpdateTree();
        }
    }
}
