using System.Collections;
using UnityEngine;

public class HookBehaviour : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private float shootSpeed, pullBackSpeed, shootCooldown;
    //private Rigidbody rb;
    private bool isShoot, canShoot, isEnd;
    private Coroutine shootCountdown;
    void Awake()
    {
        //rb = GetComponent<Rigidbody>();
        isShoot = false;
        canShoot = false;
        isEnd = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && canShoot && !isEnd)
        {
            EnableShoot();
            shootCountdown = StartCoroutine(ShootTimer());
        }
        if (isShoot)
        {
            Shoot();
        }
        if (!isShoot && transform.localPosition.y < -0.4f)
        {
            PullBack();
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Grabber")
        {
            StartCoroutine(ShootCooldown());
        }
        if (shootCountdown != null)
        {
            StopCoroutine(shootCountdown);
        }
        DisableShoot();
    }

    private void Shoot()
    {
        transform.localPosition -= new Vector3(0, shootSpeed * Time.deltaTime, 0);
    }

    private void PullBack()
    {
        transform.localPosition += new Vector3(0, pullBackSpeed * Time.deltaTime, 0);
        if (transform.localPosition.y > -0.4)
        {
            transform.localPosition = new Vector3(0, -0.4f, 0);
        }
    }

    private void EnableShoot()
    {
        canShoot = false;
        isShoot = true;
    }

    private void DisableShoot()
    {
        isShoot = false;
    }

    private IEnumerator ShootTimer()
    {
        yield return new WaitForSeconds(5f);
        DisableShoot();
    }

    private IEnumerator ShootCooldown()
    {
        yield return new WaitForSeconds(shootCooldown);
        canShoot = true;
    }
    public void EndStage()
    {
        isEnd = true;
    }

    public void StartStage()
    {
        canShoot = true;
        isShoot = false;
        isEnd = false;
    }
}
