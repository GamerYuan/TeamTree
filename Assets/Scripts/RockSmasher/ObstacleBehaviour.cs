using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleBehaviour : MonoBehaviour
{
    [SerializeField] private int score, hitCount;
    private void Death()
    {
        Destroy(gameObject);
    }
    void OnTriggerEnter(Collider other)
    {
        OnHit();
        --hitCount;
        if (hitCount == 0)
        {
            Death();
        }
    }
    public virtual void OnHit()
    {
        MinigameManagerBehaviour.AddScore(score);
    }
}
