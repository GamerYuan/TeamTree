using UnityEngine;

public class ObstacleBehaviour : MonoBehaviour
{
    [SerializeField] protected int score, hitCount;

    [Header("Events")]
    [SerializeField] private GameEvent gameEvent;
    private void Death()
    {
        GlobalMinigameManager.AddScore(score);
        Destroy(gameObject);
        gameEvent.Raise(this, score);
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
