using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class MinigameManagerBehaviour : MonoBehaviour
{
    [SerializeField] private List<GameObject> obstacleList;
    [SerializeField] private int obstacleCount;
    [SerializeField] private LayerMask obstacleLayer;
    [SerializeField] private TMP_Text scoreTextHelper;
    private static TMP_Text scoreText;
    private static int score;
    void Awake()
    {
        scoreText = scoreTextHelper;
        scoreText.text = $"Score: {score}";
        int i = 0;
        while (i < obstacleCount)
        {
            int weightedRand = Mathf.FloorToInt(Mathf.Sqrt(Random.Range(0f, Mathf.Pow(obstacleList.Count, 2) - 1)));
            int obstacleType = 3 - weightedRand;
            bool isOverlap = SpawnObstacle(obstacleType);
            if (!isOverlap)
            {
                ++i;
            }
        }
    }

    public static void AddScore(int add)
    {
        score += add;
        scoreText.text = $"Score: {score}";
    }

    private bool SpawnObstacle(int obstacleType)
    {
        float randY = Random.Range(-6f, 5.5f);
        float randX = Random.Range(-4f, 4f);
        float randRot = Random.Range(-180f, 180f);
        float randScale = Random.Range(0.95f, 1.05f);
        GameObject obstacle = Instantiate(obstacleList[obstacleType], new Vector3(randX, randY, 0), Quaternion.Euler(0, 0, randRot));
        obstacle.transform.localScale *= randScale;
        switch (obstacleType)
        {
            case 0:
                obstacle.transform.Translate(0, 0, -0.11f * randScale);
                break;
            case 1:
                obstacle.transform.Translate(0, 0, -0.24f * randScale);
                break;
            case 2:
                obstacle.transform.Translate(0, 0, -0.4f * randScale);
                break;
            case 3:
                obstacle.transform.Translate(0, 0, -0.15f * randScale);
                break;
        }

        if (Physics.OverlapBox(obstacle.transform.position, obstacle.transform.localScale / 2, obstacle.transform.rotation, obstacleLayer).Length > 1)
        {
            Destroy(obstacle);
            return true;
        }
        return false;
    }

}
