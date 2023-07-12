using System;
using System.Collections;
using System.IO;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;

public class DataSerializer : MonoBehaviour
{
    private static string fileName = "/SaveData.dat";
    public static DataSerializer instance;

    private string currentString;
    private float waterVal;
    private float coinVal;
    private int updateCount;
    private bool[] tutDone = new bool[0];
    private long lastLoginEpoch;
    private Bonsai bonsai;

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
        //Debug.Log(Application.persistentDataPath);
        bonsai = GameObject.FindGameObjectWithTag("Tree").GetComponent<Bonsai>();
    }

    public void SaveData()
    {
        string filePath = Application.persistentDataPath + fileName;
        DataProgress data = new DataProgress();
        if (bonsai == null)
        {
            bonsai = GameObject.FindGameObjectWithTag("Tree").GetComponent<Bonsai>();
        }
        currentString = bonsai.GetTreeString();
        waterVal = FlowerPotBehaviour.instance.GetWater();
        coinVal = CoinManager.instance.GetCoins();
        updateCount = StageManagerBehaviour.instance.GetUpdateCount();
        tutDone = RandomEventManager.instance.GetTutDone();
        lastLoginEpoch = DateTimeOffset.Now.ToUnixTimeSeconds();
        Debug.Log("File Saved");
        data.currentString = currentString;
        data.waterVal = waterVal;
        data.coinVal = coinVal;
        data.updateCount = updateCount;
        data.tutDone = tutDone;
        data.lastLoginEpoch = lastLoginEpoch;
        string jsonString = JsonUtility.ToJson(data);
        byte[] soup = DataEncrypter.Encrypt(jsonString);
        File.WriteAllBytes(filePath, soup);
    }

    private void LoadData()
    {
        string filePath = Application.persistentDataPath + fileName;
        if (File.Exists(filePath))
        {
            byte[] soup = File.ReadAllBytes(filePath);
            string jsonString = DataEncrypter.Decrypt(soup);
            DataProgress data = JsonUtility.FromJson<DataProgress>(jsonString);
            if (bonsai == null)
            {
                bonsai = GameObject.FindGameObjectWithTag("Tree").GetComponent<Bonsai>();
            }
            currentString = data.currentString;
            waterVal = data.waterVal;
            coinVal = data.coinVal;
            updateCount = data.updateCount;
            tutDone = data.tutDone;
            lastLoginEpoch = data.lastLoginEpoch;
            bonsai.LoadString(currentString);
            FlowerPotBehaviour.instance.SetWater(waterVal);
            CoinManager.instance.SetCoins(coinVal);
            RandomEventManager.instance.SetTutDone(tutDone);
            StageManagerBehaviour.instance.SetUpdateCount(updateCount);
            StageManagerBehaviour.instance.SetUpdateIteration(lastLoginEpoch);
            Debug.Log($"Save File Loaded!");
        } 
        else
        {
            Debug.Log("No save file is found! Loading default values");
            if (bonsai == null)
            {
                bonsai = GameObject.FindGameObjectWithTag("Tree").GetComponent<Bonsai>();
            }
            bonsai.InitTree();
            currentString = bonsai.GetTreeString();
            waterVal = 5f;
            coinVal = 5f;
            updateCount = 0;
            tutDone = new bool[0];
            FlowerPotBehaviour.instance.SetWater(waterVal);
            CoinManager.instance.SetCoins(coinVal);
            StageManagerBehaviour.instance.SetUpdateCount(updateCount);
            RandomEventManager.instance.SetTutDone(tutDone);
        }
    }

    public void SetData(string str, float val)
    {
        currentString = str;
        waterVal = val;
    }

    void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            SaveData();
        } 
        else
        {
            LoadData();
        }
    }

    void OnApplicationQuit()
    {
        SaveData();
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
            SaveData();
        }
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
        return currentString + " " + waterVal.ToString() + coinVal.ToString() + updateCount.ToString() + tutDone.ToString() + lastLoginEpoch;
    }
}

