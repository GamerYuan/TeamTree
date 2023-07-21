using UnityEngine;

public class GoodWorm : ObstacleBehaviour
{
    [Header("Subclass")]
    [SerializeField] private float punishmentRatio;

    [Header("Events")]
    [SerializeField] private GameEvent onPunishment;
    protected override void OnHit()
    {
        base.OnHit();
        onPunishment.Raise(this, punishmentRatio);
        transform.localScale *= 0.9f;
        float randX = Random.Range(-0.2f, 0.2f);
        float randY = Random.Range(-0.2f, 0.2f);
        float randRotate = Random.Range(-30, 30);
        transform.Translate(randX, randY, 0);
        transform.Rotate(0, 0, randRotate);
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        Color currColor = meshRenderer.material.color;
        Color newColor = currColor * 0.85f;
        meshRenderer.material.color = newColor;
    }
}
