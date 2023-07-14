using UnityEngine;

public class ObstacleBehaviour : MonoBehaviour
{
    [SerializeField] protected int score, hitCount;
    private void Death()
    {
        GlobalMinigameManager.AddScore(score);
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
    protected virtual void OnHit()
    {
        // do nothing
    }
}
