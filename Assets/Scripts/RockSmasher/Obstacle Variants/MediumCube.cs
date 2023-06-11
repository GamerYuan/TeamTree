using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MediumCube : ObstacleBehaviour
{
    public override void OnHit()
    {
        base.OnHit();
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        Color currColor = meshRenderer.material.color;
        Color newColor = currColor * 0.5f;
        meshRenderer.material.color = newColor;
    }
}
