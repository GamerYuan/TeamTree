using System;
using UnityEngine;

public class TimeChange : MonoBehaviour, ITimeChanger
{
    void Awake()
    {
        ChangeTime(DateTime.Now);
    }

    public void ChangeTime(DateTime currTime)
    {
        int currHour = currTime.Hour;
        int currMinute = currTime.Minute;
        int currTimeOffset = currHour * 60 + currMinute;
        GetComponent<Light>().transform.rotation = Quaternion.Euler((currTimeOffset - 360) * 360f / 1440f, 50, 0);
    }
}
