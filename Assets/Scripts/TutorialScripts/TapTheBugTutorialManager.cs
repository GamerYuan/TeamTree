using System.Collections.Generic;
using UnityEngine;

public class TapTheBugTutorialManager : GlobalMinigameManager
{
    [SerializeField] private List<GameObject> bugs;
    [SerializeField] private int count;

    private void Start()
    {
        timerText.text = "";
    }

    public void OnTutorialStart()
    {
        for (int i = 0; i < count - 1; i++)
        {
            Instantiate(bugs[0], new Vector3(Random.Range(-2f, 2f), 10, Random.Range(-2f, 2f)), Quaternion.identity);
        }
        Instantiate(bugs[1], new Vector3(Random.Range(-2f, 2f), 10, Random.Range(-2f, 2f)), Quaternion.identity);
    }

    public void OnBugDie(Component sender, object data)
    {
        if (sender is BugBehaviour)
        {
            count--;
        }
        if (count <= 0)
        {
            StopStage();
            RandomEventManager.instance.TutorialDone();
        }
    }

    protected override void Update()
    {
        // do nothing
    }
}
