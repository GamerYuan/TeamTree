using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabberBehaviour : MonoBehaviour
{
    [SerializeField] private float rotation;
    private bool isSpin, isCooldown;
    private float rotTime;
    private LineRenderer lineRenderer;
    private bool isEnd;
    // Start is called before the first frame update
    void Start()
    {
        isSpin = true;
        isEnd = false;
        lineRenderer = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isSpin)
        {
            rotTime += Time.deltaTime;
            Spin();
        }
        if (Input.GetMouseButtonDown(0) && !isEnd)
        {
            DisableSpin();
            StartCoroutine(CooldownCount());
        }
        if (transform.GetChild(0).localPosition.y >= -0.4f && !isCooldown)
        {
            EnableSpin();
        }
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, transform.GetChild(0).position);
    }

    private void EnableSpin()
    {
        isSpin = true;
    }

    private void DisableSpin()
    {
        isSpin = false;
    }

    private void Spin()
    {
        float pingPongVal = Mathf.PingPong(rotTime * 30f, 2 * rotation);
        float rotationDeg = pingPongVal - rotation;
        transform.rotation = Quaternion.Euler(0, 0, rotationDeg);
    }
    
    private IEnumerator CooldownCount()
    {
        isCooldown = true;
        yield return new WaitForSeconds(0.05f);
        isCooldown = false;
    }
    public void EndStage()
    {
        isEnd = true;
    }
}
