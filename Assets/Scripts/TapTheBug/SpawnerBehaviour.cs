using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerBehaviour : MonoBehaviour
{
    [SerializeField] private float spawnCooldown;
    [SerializeField]
    [Range(0f, 1f)] private float cooldownMultiplier;
    [SerializeField] private List<GameObject> bugList;

    void Start()
    {
        StartCoroutine(SpawnBugs());
    }

    private IEnumerator SpawnBugs()
    {
        while(true)
        {
            int randBug = Random.Range(0, bugList.Count);
            Vector3 pos = transform.position;
            Instantiate(bugList[randBug], pos, Quaternion.identity);
            float randCd = Random.Range(0.5f, spawnCooldown);
            spawnCooldown = Mathf.Clamp(spawnCooldown * cooldownMultiplier, 1.0f, spawnCooldown);
            Debug.Log($"New bug in {randCd.ToString("F2")}");
            yield return new WaitForSeconds(randCd);
        }
    }

}
