using UnityEngine;

public class MediumCube : ObstacleBehaviour
{
    protected override void OnHit()
    {
        base.OnHit();
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        Color currColor = meshRenderer.material.color;
        Color newColor = currColor * 0.5f;
        meshRenderer.material.color = newColor;
    }
}
