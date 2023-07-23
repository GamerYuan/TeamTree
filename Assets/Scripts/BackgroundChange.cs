using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundChange : MonoBehaviour, ITimeChanger
{
    private float baseColorVal = 200f/255;
    private Camera cam;
    void Awake()
    {
        cam = GetComponent<Camera>();
        ChangeTime(DateTime.Now);
    }

    public void ChangeTime(DateTime currTime)
    {
        int currHour = currTime.Hour;
        int currMinute = currTime.Minute;
        int currTimeOffset = currHour * 60 + currMinute;
        int currTimeVal = Mathf.Abs(currTimeOffset - 720);
        float newColorVal = baseColorVal - 150f / 720 * currTimeVal / 255f;
        cam.backgroundColor = new Color(newColorVal, newColorVal, newColorVal);
    }
}
