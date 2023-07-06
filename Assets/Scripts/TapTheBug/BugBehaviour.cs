using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BugBehaviour : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private float moveSpeed, moveCooldown;
    [SerializeField] private int wallCollideCount, wallLayer, score;
    [SerializeField] private LayerMask groundLayer;
    private bool isMoving = true;
    private float hitWalliFrame = 0.5f;
    private bool isHitWall = false;
    protected Coroutine moveCoroutine;

    private float velX, velZ;

    protected Rigidbody rb;

    [Header("Events")]
    [SerializeField] private GameEvent gameEvent;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        velX = 0;
        velZ = 0;
    }

    private void OnMouseDown()
    {
        if (!isMoving)
        {
            GlobalMinigameManager.AddScore(score);
            Death();
        }
    }

    protected virtual void Update()
    {
        rb.velocity = new Vector3(velX, rb.velocity.y, velZ);
        if (isHitWall)
        {
            hitWalliFrame -= Time.deltaTime;
        }
        if (hitWalliFrame <= 0f)
        {
            isHitWall = false;
            hitWalliFrame = 0.5f;
        }
    }

    protected virtual void Death()
    {
        gameEvent.Raise(this, score);
    }

    protected IEnumerator Move()
    {
        while(true)
        {
            float randNum = Random.Range(0f, 1f);
            int randXDirection = Random.Range(0, 2);
            int randZDirection = Random.Range(0, 2);
            if (randXDirection == 0) randXDirection = -1;
            if (randZDirection == 0) randZDirection = -1;
            float angle = Mathf.Asin(randNum);
            velX = moveSpeed * Mathf.Cos(angle) * randXDirection;
            velZ = moveSpeed * randNum * randZDirection;
            rb.velocity = new Vector3(velX, rb.velocity.y, velZ);
            Rotate();
            yield return new WaitForSeconds(moveCooldown);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        LayerMask layerMask = collision.gameObject.layer;
        if (isMoving && Physics.OverlapSphere(transform.position, 0.2f, groundLayer) != null)
        {
            moveCoroutine = StartCoroutine(Move());
            isMoving = false;
            return;
        }
        if (!isHitWall && layerMask == wallLayer)
        {
            isHitWall = true;
            if (wallCollideCount == 0)
            {
                Death();
                return;
            }
            MoveBackwards();
            wallCollideCount--;
            return;
        }
        MoveBackwards();
    }

    private void Rotate()
    {
        transform.rotation = Quaternion.LookRotation(new Vector3(rb.velocity.x, 0, rb.velocity.z), Vector3.up);
    }

    private void MoveBackwards()
    {
        velX = Mathf.Clamp(rb.velocity.x * -1.2f, -1.5f, 1.5f);
        velZ = Mathf.Clamp(rb.velocity.z * -1.2f, -1.5f, 1.5f);
        rb.velocity = new Vector3(velX, 0, velZ);
        Rotate();
    }
}
