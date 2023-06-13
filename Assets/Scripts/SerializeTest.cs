using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;

public class SerializeTest : MonoBehaviour
{
    [SerializeField] private DataSerializer dataSerializer;
    public void Test1()
    {
        dataSerializer.SetData("FF", 10f);
        dataSerializer.SaveData();
    }
    public void Test2()
    {
        dataSerializer.SetData("F[+F]F", 50.345f);
        dataSerializer.SaveData();
    }
    public void Test3()
    {
        int randNum = Random.Range(0, 1000);
        string randStr = RandomString(randNum);
        float randFloat = Random.Range(0f, 100f);
        dataSerializer.SetData(randStr, randFloat);
        Debug.Log(randStr+ " " + randFloat);
        dataSerializer.SaveData();
    }

    public void LoadData()
    {
        dataSerializer.LoadData();
    }

    private string RandomString(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        StringBuilder ret = new StringBuilder();
        for (int i = 0; i < length; i++)
        {
            ret.Append(chars[Random.Range(0, chars.Length)]);
        }
        return ret.ToString();
    }
}
