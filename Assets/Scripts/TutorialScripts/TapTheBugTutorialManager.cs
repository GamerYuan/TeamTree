using System.Collections.Generic;
using UnityEngine;

public class TapTheBugTutorialManager : GlobalMinigameManager
{
    [SerializeField] private List<GameObject> bugs;
    [SerializeField] private int count;

    public void OnTutorialStart()
    {
        StartStage();
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
            RandomEventManager.instance.TutorialDone();
            StopStage();
        }
    }
}
