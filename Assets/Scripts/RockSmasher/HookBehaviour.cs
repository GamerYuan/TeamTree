using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookBehaviour : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private float shootSpeed, pullBackSpeed;
    //private Rigidbody rb;
    private bool isShoot = false;
    void Start()
    {
        //rb = GetComponent<Rigidbody>();
        isShoot = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            EnableShoot();
            StartCoroutine(ShootTimer());
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

    private void Shoot()
    {
        transform.localPosition -= new Vector3(0, shootSpeed * Time.deltaTime, 0);
    }
    
    private void PullBack()
    {
        Debug.Log("Pulling back");
        transform.localPosition += new Vector3(0, pullBackSpeed * Time.deltaTime, 0);
        if (transform.localPosition.y > -0.4)
        {
            transform.localPosition = new Vector3(0, -0.4f, 0);
        }
    }

    private void EnableShoot()
    {
        isShoot = true;
    }

    private void DisableShoot()
    {
        Debug.Log("Disabling Shoot");
        isShoot = false;
    }

    private IEnumerator ShootTimer()
    {
        yield return new WaitForSeconds(5f);
        DisableShoot();
    }
}
