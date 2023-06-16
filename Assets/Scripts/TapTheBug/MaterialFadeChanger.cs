using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialFadeChanger : MonoBehaviour
{
    // Start is called before the first frame update
    private Renderer meshRenderer;
    private Color endColor, startColor;
    void Start()
    {
        meshRenderer = GetComponent<Renderer>();
        startColor = new Color(1f, 1f, 1f, 0.1f);
        endColor = new Color(1f, 1f, 1f, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        meshRenderer.material.color = Color.Lerp(startColor, endColor, (10 - transform.parent.position.y)/10);
    }
}
