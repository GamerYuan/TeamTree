using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundChange : MonoBehaviour, ITimeChanger
{
    private Color baseColor = new(230f/255f, 130f/255f, 1f);
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
        cam.backgroundColor = new Color(baseColor.r - (170f/720) * currTimeVal/255f, baseColor.g - (130f / 720) * currTimeVal / 255f, baseColor.b - (185f / 720) * currTimeVal / 255f);
    }

}
