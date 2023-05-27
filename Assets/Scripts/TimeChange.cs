using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeChange : MonoBehaviour
{
    void Update()
    {
        GetComponent<Light>().transform.Rotate(Time.deltaTime * 15f * (Mathf.Round(transform.eulerAngles.x/180) + 1), 0, 0);
    }
}
