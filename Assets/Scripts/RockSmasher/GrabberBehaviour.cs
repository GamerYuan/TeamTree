using System.Collections;
using UnityEngine;

public class GrabberBehaviour : MonoBehaviour
{
    [SerializeField] private float rotation;
    private bool isSpin, isCooldown;
    private float rotTime;
    private LineRenderer lineRenderer;
    private bool isEnd, stageStart;
    // Start is called before the first frame update
    void Awake()
    {
        stageStart = false;
        isSpin = false;
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
        if (transform.GetChild(0).localPosition.y >= -0.41f && !isCooldown && stageStart)
        {
            EnableSpin();
        }
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, transform.GetChild(0).position);
    }

    public void EnableSpin()
    {
        isSpin = true;
    }

    public void DisableSpin()
    {
        isSpin = false;
    }

    public void StartStage()
    {
        stageStart = true;
        EnableSpin();
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
        yield return new WaitForSeconds(0.01f);
        isCooldown = false;
    }
    public void EndStage()
    {
        isEnd = true;
    }
}
