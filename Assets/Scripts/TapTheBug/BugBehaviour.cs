using System.Collections;
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
    [SerializeField] private GameEvent onBugDeath;
    [SerializeField] private GameEvent onBugKill;

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
            Kill();
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

    protected virtual void Kill()
    {
        onBugKill.Raise(this, score);
        Death();
    }

    protected virtual void Death()
    {
        Destroy(gameObject);
        onBugDeath.Raise(this, score);
    }

    protected IEnumerator Move()
    {
        while (true)
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

    protected void GenerateSplatter(Color splatterColor)
    {
        GameObject splatterPlane = GameObject.CreatePrimitive(PrimitiveType.Plane);
        splatterPlane.GetComponent<MeshCollider>().enabled = false;
        MeshRenderer meshRenderer = splatterPlane.GetComponent<MeshRenderer>();
        MeshFilter meshFilter = splatterPlane.GetComponent<MeshFilter>();
        meshRenderer.material = Resources.Load<Material>("Sprites/Materials/splat");
        meshRenderer.material.SetFloat("_Mode", 2);
        meshRenderer.material.SetColor("_Color", splatterColor);
        float randScale = Random.Range(0.09f, 0.12f);
        splatterPlane.transform.localScale = Vector3.one * randScale;
        splatterPlane.transform.eulerAngles = new Vector3(0, Random.Range(-180f, 180f), 0);
        splatterPlane.transform.position = new Vector3(transform.position.x, 0.005f, transform.position.z);
        Destroy(splatterPlane, Random.Range(5f, 20f));
    }

    protected Color RandomBlue()
    {
        return new Color(Random.Range(0f, 0.4f), Random.Range(0.8f, 1f), Random.Range(0.3f, 0.75f));
    }

    protected Color RandomRed()
    {
        return new Color(Random.Range(0.8f, 1f), Random.Range(0.3f, 0.75f), Random.Range(0f, 0.4f));
    }
}
