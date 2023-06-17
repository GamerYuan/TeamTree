using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

public class FlowerPotBehaviour : MonoBehaviour
{
    [SerializeField] private float water;
    [SerializeField] private float maxWater;
    [SerializeField] private float minWater;
    [SerializeField] private TMP_Text text;

    public static FlowerPotBehaviour instance;

    private bool startWater;

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
    }
    void Start()
    {
        StartCoroutine(WaterCall());
        //StartCoroutine(WaterCountdown());
    }

    // Update is called once per frame
    void Update()
    {
        if (water > maxWater)
        {
            water = maxWater;
        }
        if (startWater)
        {
            AddWater();
        }
        //text.text = $"Water: {System.Math.Round(water, 2)}/{maxWater}";
    }

    void OnMouseDown()
    {
        startWater = true;
    }

    void OnMouseUp()
    {
        startWater = false;
    }

    private void AddWater()
    {
        if (water + 5 * Time.deltaTime <= maxWater)
        {
            water += 5 * Time.deltaTime;
            CoinManager.instance.RemoveCoins(1 * Time.deltaTime);
        }
    }

    public void DecreaseWater(float num)
    {
        if (water - num >= minWater)
        {
            water -= num;
        } 
        else
        {
            water = minWater;
        }
    }

    public float GetWater()
    {
        return water;
    }
    public void SetWater(float waterVal)
    {
        water = waterVal;
    }

    private IEnumerator WaterCall()
    {
        while (true)
        {
            Debug.Log($"Current Water Level is: {water}/{maxWater}");
            yield return new WaitForSeconds(2f);
        }
    }
    
    private IEnumerator WaterCountdown()
    {
        while (true)
        {
            yield return new WaitForSeconds(5f);
            DecreaseWater(1);
        }
    }
}
