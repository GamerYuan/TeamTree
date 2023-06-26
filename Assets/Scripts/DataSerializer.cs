using System;
using System.Collections;
using System.IO;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;

public class DataSerializer : MonoBehaviour
{
    private static string fileName = "/SaveData.dat";

    private string currentString;
    private float waterVal;
    private float coinVal;
    private Bonsai bonsai;

    private static string key = "N7OnL3lf8YasErkERkQAE7+u5R6fspD6QkQZhWhCv/4=";
    private static string iv = "dt9espR+qOm3M5jlfo5uqQ==";

    void Awake()
    {
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
        Debug.Log("File Saved");
        data.currentString = currentString;
        data.waterVal = waterVal;
        data.coinVal = coinVal;
        string jsonString = JsonUtility.ToJson(data);
        byte[] soup = Encrypt(jsonString);
        File.WriteAllBytes(filePath, soup);
    }

    public void LoadData()
    {
        string filePath = Application.persistentDataPath + fileName;
        if (File.Exists(filePath))
        {
            byte[] soup = File.ReadAllBytes(filePath);
            string jsonString = Decrypt(soup);
            DataProgress data = JsonUtility.FromJson<DataProgress>(jsonString);
            if (bonsai == null)
            {
                bonsai = GameObject.FindGameObjectWithTag("Tree").GetComponent<Bonsai>();
            }
            currentString = data.currentString;
            waterVal = data.waterVal;
            coinVal = data.coinVal;
            bonsai.LoadString(currentString);
            FlowerPotBehaviour.instance.SetWater(waterVal);
            CoinManager.instance.SetCoins(coinVal);
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
            FlowerPotBehaviour.instance.SetWater(waterVal);
            CoinManager.instance.SetCoins(coinVal);
        }
    }

    public void SetData(string str, float val)
    {
        currentString = str;
        waterVal = val;
    }

    private byte[] Encrypt(string original)
    {
        byte[] encrypted;
        using (Aes aes = Aes.Create())
        {
            aes.Key = Convert.FromBase64String(key);
            aes.IV = Convert.FromBase64String(iv);

            ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                {
                    using (StreamWriter streamWriter = new StreamWriter(cryptoStream))
                    {
                        streamWriter.Write(original);
                    }
                    encrypted = memoryStream.ToArray();
                }
            }
        }
        return encrypted;
    }

    private string Decrypt(byte[] encrypted)
    {
        string decrypted;

        using (Aes aes = Aes.Create())
        {
            aes.Key = Convert.FromBase64String(key);
            aes.IV = Convert.FromBase64String(iv);

            ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

            using (MemoryStream memoryStream = new MemoryStream(encrypted))
            {
                using (CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                {
                    using (StreamReader streamReader = new StreamReader(cryptoStream))
                    {
                        decrypted = streamReader.ReadToEnd();
                    }
                }
            }
        }

        return decrypted;
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

    public override string ToString()
    {
        return currentString + " " + waterVal.ToString() + coinVal.ToString();
    }
}

