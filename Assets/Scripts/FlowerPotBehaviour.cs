using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FlowerPotBehaviour : MonoBehaviour
{
    [SerializeField] private float water;
    [SerializeField] private float maxWater;
    [SerializeField] private float minWater;
    [SerializeField] private TMP_Text text;

    private bool startWater;
    
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(WaterCall());
        StartCoroutine(WaterCountdown());
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
            Debug.Log("Adding Water");
        }
        text.text = $"Water: {System.Math.Round(water, 2)}/{maxWater}";
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
        }
    }

    private void DecreaseWater(float num)
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
        yield return new WaitForSeconds(5f);
        while (true)
        {
            DecreaseWater(1);
            yield return new WaitForSeconds(5f);
        }
    }
}
