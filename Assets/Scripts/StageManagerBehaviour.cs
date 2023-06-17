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
        currTree = nextTree;
    }

    public void UpdateTree()
    {
        Bonsai bonsai = currTree.GetComponent<Bonsai>();
        bonsai.TreeUpdate();
    }

}
