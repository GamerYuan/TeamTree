using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BugBehaviour : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private float moveSpeed, moveCooldown;
    [SerializeField] private int wallCollideCount;

    private Rigidbody rb;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        StartCoroutine(Move());
    }

    private void OnMouseDown()
    {
        Death();
    }

    private void Death()
    {
        Destroy(gameObject);
    }

    private IEnumerator Move()
    {
        while(true)
        {
            float randNum = Random.Range(0.5f, 1f);
            int randXDirection = Random.Range(-1, 2);
            int randYDirection = Random.Range(-1, 2);
            float angle = Mathf.Asin(randNum);
            rb.velocity = new Vector3(moveSpeed * Mathf.Cos(angle) * randXDirection, moveSpeed * randNum * randYDirection, 0);
            Rotate();
            yield return new WaitForSeconds(moveCooldown);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.layer == 6)
        {
            if (wallCollideCount == 0)
            {
                Death();
                return;
            }
            MoveBackwards();
            wallCollideCount--;
        }
    }

    private void Rotate()
    {
        transform.rotation = Quaternion.LookRotation(new Vector3(0, 0, 1), rb.velocity);
    }

    private void MoveBackwards()
    {
        rb.velocity *= -1.5f;
        Rotate();
    }
}
