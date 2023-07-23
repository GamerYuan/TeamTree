using UnityEngine;

public class Bug1 : BugBehaviour
{
    // Start is called before the first frame update
    private GameObject body, head;
    protected override void Death()
    {
        body = gameObject.transform.GetChild(0).gameObject;
        head = gameObject.transform.GetChild(1).gameObject;
        gameObject.layer = 2;
        body.layer = 2;
        head.layer = 2;
        GenerateSplatter(RandomBlue());
        body.transform.SetParent(null);
        head.transform.SetParent(null);
        body.GetComponent<MaterialFadeChanger>().enabled = false;
        head.GetComponent<MaterialFadeChanger>().enabled = false;
        Rigidbody rb = GetComponent<Rigidbody>();
        Vector3 velocity = rb.velocity;
        rb.velocity = Vector3.zero;
        base.Death();
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
}
