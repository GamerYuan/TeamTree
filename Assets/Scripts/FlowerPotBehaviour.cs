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
    private GameObject currTree;
    
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(WaterCall());
        currTree = GameObject.FindGameObjectWithTag("Tree");
        StartCoroutine(WaterTree());
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

    private IEnumerator WaterCall()
    {
        while (true)
        {
            Debug.Log($"Current Water Level is: {water}/{maxWater}");
            yield return new WaitForSeconds(2f);
        }
    }

    // Let tree suck water every 5s
    private IEnumerator WaterTree()
    {
        while (true)
        {
            yield return new WaitForSeconds(5f);
            if (water >= 0.5f)
            {
                Debug.Log("Watering Tree!");
                float decAmount = water * 0.7f;
                DecreaseWater(decAmount);
                if (currTree == null)
                {
                    currTree = GameObject.FindGameObjectWithTag("Tree");
                }
                Bonsai bonsai = currTree.GetComponent<Bonsai>();
                bonsai.WaterTree(decAmount * 0.95f);
            }
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
