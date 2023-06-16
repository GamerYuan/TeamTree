using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerBehaviour : MonoBehaviour
{
    [SerializeField] private float spawnCooldown, startCooldown;
    [SerializeField]
    [Range(0f, 1f)] private float cooldownMultiplier;
    [SerializeField] private List<GameObject> bugList;
    private int counter = 5;
    private List<GameObject> spawnList;

    void Start()
    {
        StartCoroutine(StartDelay());
        spawnList = new List<GameObject>()
        {
            bugList[0],
        };
    }

    private IEnumerator StartDelay()
    {
        yield return new WaitForSeconds(startCooldown);
        StartCoroutine(SpawnBugs());
    }

    private IEnumerator SpawnBugs()
    {
        
        while(true)
        {
            int randBug;
            float randNum = Random.Range(0f, 1f);
            if (randNum < 0.7)
            {
                randBug = 0;
            } 
            else
            {
                randBug = 1;
            }
            Vector3 pos = transform.position;
            GameObject currBug = Instantiate(bugList[randBug], pos, Quaternion.identity);
            currBug.transform.localScale = Vector3.one * Random.Range(0.9f, 1.3f);
            float randCd = Random.Range(0.5f, spawnCooldown);
            spawnCooldown = Mathf.Clamp(spawnCooldown * cooldownMultiplier, 1.0f, spawnCooldown);
            counter--;
            if (counter == 0) spawnList.Add(bugList[1]);
            yield return new WaitForSeconds(randCd);
        }
    }

}
