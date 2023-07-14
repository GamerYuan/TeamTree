using UnityEngine;

public class LargeCube : ObstacleBehaviour
{
    protected override void OnHit()
    {
        base.OnHit();
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        Color currColor = meshRenderer.material.color;
        Color newColor = currColor * 0.4f;
        meshRenderer.material.color = newColor;
    }
}
