using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bug2 : BugBehaviour
{
    private bool isDeath;
    private float flySpeed;
    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        if (isDeath)
        {
            rb.velocity = new Vector3(rb.velocity.x, flySpeed, rb.velocity.z);
        }
    }

    protected override void Death()
    {
        GetComponent<CapsuleCollider>().enabled = false;
        gameObject.layer = 2;
        rb = GetComponent<Rigidbody>();
        isDeath = true;
        flySpeed = Random.Range(0.5f, 2f);
        Destroy(gameObject, Random.Range(2f, 5f));
        StopCoroutine(moveCoroutine);
        base.Death();
    }
}
