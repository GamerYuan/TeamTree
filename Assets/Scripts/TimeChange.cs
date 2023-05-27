using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeChange : MonoBehaviour
{
    void Update()
    {
        GetComponent<Light>().transform.Rotate(Time.deltaTime * 20f * ((transform.eulerAngles.x/180) + 3) / 3, 0, 0);
    }
}
