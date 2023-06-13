using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DataSerializer : MonoBehaviour
{
    // Start is called before the first frame update
    private string currentString;
    private float waterVal;

    void Start()
    {
        Debug.Log(Application.persistentDataPath);
    }

    public void SaveData()
    {
        string filePath = Application.persistentDataPath + "/SaveData.json";
        DataProgress data = new DataProgress();
        data.currentString = currentString;
        data.waterVal = waterVal;
        string jsonString = JsonUtility.ToJson(data);
        File.WriteAllText(filePath, jsonString);
    }

    public void LoadData()
    {
        string filePath = Application.persistentDataPath + "/SaveData.json";
        if (File.Exists(filePath))
        {
            DataProgress data = JsonUtility.FromJson<DataProgress>(File.ReadAllText(filePath));
            currentString = data.currentString;
            waterVal = data.waterVal;
            Debug.Log($"Save File Loaded! {data}");
        } 
        else
        {
            Debug.Log("No save file is found! Loading default values");
            currentString = "F";
            waterVal = 0;
        }
    }

    public void SetData(string str, float val)
    {
        currentString = str;
        waterVal = val;
    }
}

[Serializable]
public struct DataProgress
{
    public string currentString;
    public float waterVal;

    public override string ToString()
    {
        return currentString + " " + waterVal.ToString();
    }
}
