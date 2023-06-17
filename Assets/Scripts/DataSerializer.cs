using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using UnityEngine;

public class DataSerializer : MonoBehaviour
{
    private static string fileName = "/SaveData.dat";

    private string currentString;
    private float waterVal;

    private static string key = "N7OnL3lf8YasErkERkQAE7+u5R6fspD6QkQZhWhCv/4=";
    private static string iv = "dt9espR+qOm3M5jlfo5uqQ==";

    void Start()
    {
        Debug.Log(Application.persistentDataPath);
        StartCoroutine(SaveDataRoutine());
    }

    public void SaveData()
    {
        string filePath = Application.persistentDataPath + fileName;
        DataProgress data = new DataProgress();
        data.currentString = currentString;
        data.waterVal = waterVal;
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

    public override string ToString()
    {
        return currentString + " " + waterVal.ToString();
    }
}

