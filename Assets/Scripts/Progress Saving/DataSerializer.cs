using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class DataSerializer : MonoBehaviour
{
    private static string fileName = "/SaveData.dat";
    private static string filePath;
    public static DataSerializer instance;

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
        filePath = Application.persistentDataPath + fileName;
    }

    public void SaveGameAsync()
    {
        Task.Run(() => SaveGame());
    }

    private async void SaveGame()
    {
        Debug.Log("Saving File...");
        DataProgress data = SaveData.GetDataProgress();
        string jsonString = JsonUtility.ToJson(data);
        byte[] soup = await DataEncrypter.Encrypt(jsonString);
        await File.WriteAllBytesAsync(filePath, soup);
    }

    private void LoadData()
    {
        if (File.Exists(filePath))
        {
            byte[] soup = File.ReadAllBytes(filePath);
            string jsonString = DataEncrypter.Decrypt(soup);
            DataProgress data = JsonUtility.FromJson<DataProgress>(jsonString);
            SaveData.LoadSavedValue(data);
            Debug.Log($"Save File Loaded!");
        }
        else
        {
            Debug.Log("No save file is found! Loading default values");
            SaveData.LoadDefaultValue();
        }
    }

    void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            Task.Run(() => SaveGame());
        }
        else
        {
            LoadData();
        }
    }

    void OnApplicationQuit()
    {
        SaveGame();
    }
    void OnEnable()
    {
        LoadData();
        StartCoroutine(SaveDataRoutine());
    }

    private IEnumerator SaveDataRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(10f);
            Debug.Log("10s Autosave!");
            Task.Run(() => SaveGame());
        }
    }

    private void LoadDataTimer()
    {
        while (RandomEventManager.instance == null || StageManagerBehaviour.instance == null || FlowerPotBehaviour.instance == null)
        {
            Debug.Log("Scripts not loaded, retrying...");
            Task.Delay(TimeSpan.FromSeconds(0.2f));
        }
        LoadData();
    }
}