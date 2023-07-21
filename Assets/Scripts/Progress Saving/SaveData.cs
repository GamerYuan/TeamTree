using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public static class SaveData
{
    private static DataProgress currData;
    private static Bonsai bonsai;

    public static string currString { get; private set; }
    public static float waterVal { get; private set; }
    public static float coinVal { get; private set; }
    public static int updateCount { get; private set; }
    public static bool[] tutDone { get; private set; }
    public static long loginEpochTime { get; private set; }
    public static void LoadDefaultValue()
    {
        bonsai = GameObject.FindGameObjectWithTag("Tree").GetComponent<Bonsai>();
        bonsai.InitTree();
        currString = bonsai.GetTreeString();
        waterVal = 5f;
        coinVal = 5f;
        updateCount = 0;
        tutDone = new bool[0];
        loginEpochTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        currData = new DataProgress
        {
            currentString = currString,
            waterVal = waterVal,
            coinVal = coinVal,
            updateCount = updateCount,
            tutDone = tutDone,
            lastLoginEpoch = loginEpochTime,
        };
        Debug.Log(currData);
    }

    public static void LoadSavedValue(DataProgress dataProgress)
    {
        currData = dataProgress;
        currString = currData.currentString;
        waterVal = currData.waterVal; 
        coinVal = currData.coinVal;
        updateCount = currData.updateCount;
        tutDone = currData.tutDone;
        loginEpochTime = currData.lastLoginEpoch;
        Debug.Log(currData);
    }

    public static DataProgress GetDataProgress()
    {
        currData = new DataProgress
        {
            currentString = currString,
            waterVal = waterVal,
            coinVal = coinVal,
            updateCount = updateCount,
            tutDone = tutDone,
            lastLoginEpoch = loginEpochTime,
        };
        Debug.Log(currData);
        return currData;
    }

    public static void SetString(string str)
    {
        currString = str;
    }

    public static void SetWater(float val)
    {
        waterVal = val;
    }

    public static void SetCoin(float val)
    {
        coinVal = val;
    }

    public static void SetUpdate(int val)
    {
        updateCount = val;
    }

    public static void SetTutDone(bool[] tutSave)
    {
        tutDone = tutSave;
    }

    public static void SetLoginEpoch(long lastLogin)
    {
        loginEpochTime = lastLogin;
    }

}

[Serializable]
public struct DataProgress
{
    public string currentString;
    public float waterVal;
    public float coinVal;
    public int updateCount;
    public bool[] tutDone;
    public long lastLoginEpoch;
    public override string ToString()
    {
        return $"{currentString}, {waterVal.ToString()}, {coinVal.ToString()}, {updateCount.ToString()}, {tutDone.ToString()}, {lastLoginEpoch}";
    }
}