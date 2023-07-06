using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Bug1 : BugBehaviour
{
    // Start is called before the first frame update
    private GameObject body, head;
    protected override void Death()
    {
        base.Death();
        body = gameObject.transform.GetChild(0).gameObject;
        head = gameObject.transform.GetChild(1).gameObject;
        gameObject.layer = 2;
        body.layer = 2;
        head.layer = 2;
        GenerateSplatter();
        body.transform.SetParent(null);
        head.transform.SetParent(null);
        body.GetComponent<MaterialFadeChanger>().enabled = false;
        head.GetComponent<MaterialFadeChanger>().enabled = false;
        Rigidbody rb = GetComponent<Rigidbody>();
        Vector3 velocity = rb.velocity;
        rb.velocity = Vector3.zero;
        head.AddComponent<Rigidbody>();
        head.AddComponent<BoxCollider>();
        body.AddComponent<Rigidbody>();
        body.AddComponent<BoxCollider>();
        GetComponent<BoxCollider>().enabled = false;
        body.GetComponent<Rigidbody>().velocity = Vector3.zero;
        float xDir = velocity.x < 0 ? -1 : 1;
        float zDir = velocity.z < 0 ? -1 : 1;
        head.GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(0.5f, 1f) * xDir, 0, Random.Range(0.5f, 1f) * zDir), ForceMode.Impulse);
        Destroy(body, 3);
        Destroy(head, 3);
    }

    private void GenerateSplatter()
    {
        GameObject splatterPlane = GameObject.CreatePrimitive(PrimitiveType.Plane);
        splatterPlane.GetComponent<MeshCollider>().enabled = false;
        MeshRenderer meshRenderer = splatterPlane.GetComponent<MeshRenderer>();
        MeshFilter meshFilter = splatterPlane.GetComponent<MeshFilter>();
        meshRenderer.material = Resources.Load<Material>("Sprites/Materials/splat");
        meshRenderer.material.SetFloat("_Mode", 2);
        meshRenderer.material.SetColor("_Color", new Color(Random.Range(0f, 1f), Random.Range(0.7f, 1f), Random.Range(0.6f, 1f)));
        float randScale = Random.Range(0.09f, 0.12f);
        splatterPlane.transform.localScale = Vector3.one * randScale;
        splatterPlane.transform.eulerAngles = new Vector3(0, Random.Range(-180f, 180f), 0);
        splatterPlane.transform.position = new Vector3(transform.position.x, 0.005f, transform.position.z);
        Destroy(splatterPlane, Random.Range(5f, 20f));
    }
}
