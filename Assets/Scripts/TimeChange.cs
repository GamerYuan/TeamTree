using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeChange : MonoBehaviour
{
    void Update()
    {
        GetComponent<Light>().transform.Rotate(Time.deltaTime * 20f, 0, 0);
    }
}
