using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public static TimeManager instance;
    public long loginEpochTime { get; private set; }

    // Start is called before the first frame 
    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        } 
        else
        {
            instance = this;
        }
        DontDestroyOnLoad(this);
        Application.targetFrameRate = 60;
    }

    public void SetUpdateTime(long time)
    {
        loginEpochTime = time;
    }

    public void ChangeUpdateTime(Component sender, object data)
    {
        if (sender is StageManagerBehaviour)
        {
            loginEpochTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        }
    }
}
