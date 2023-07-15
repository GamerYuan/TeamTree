using System.Collections.Generic;
using UnityEngine;

public class RockSmasherTutManager : RockSmasherManager
{
    [SerializeField] private int count;
    [SerializeField] private List<GameObject> enableList;
    private void Start()
    {
        timerText.text = "";
        grabberBehaviour.DisableSpin();
    }

    public void OnTutorialStart()
    {
        grabberBehaviour.StartStage();
        hookBehaviour.StartStage();
        foreach (GameObject gameObject in enableList)
        {
            gameObject.SetActive(true);
        }
    }

    public void OnRockDestroyed(Component sender, object data)
    {
        if (sender is ObstacleBehaviour && sender is not GoodWorm)
        {
            count--;
            Debug.Log("Rock Destroyed");
        }
        if (count <= 0)
        {
            RandomEventManager.instance.TutorialDone();
            StopStage();
        }
    }

    protected override void Update()
    {
        // do nothing
    }

}
