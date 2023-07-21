using System.Collections;
using UnityEngine;

public class FlowerPotBehaviour : MonoBehaviour
{
    [SerializeField] private float water;
    [SerializeField] private float maxWater;
    [SerializeField] private float minWater;

    public static FlowerPotBehaviour instance;

    private bool startWater, canWater;

    [Header("Events")]
    [SerializeField] private GameEvent onWaterChange;

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        canWater = true;
        
    }
    void Start()
    {
        water = SaveData.waterVal;

        StartCoroutine(WaterCall());
        onWaterChange.Raise(this, water);
        //StartCoroutine(WaterCountdown());
    }

    // Update is called once per frame
    void Update()
    {
        if (water > maxWater)
        {
            water = maxWater;
        }
        if (startWater && water < maxWater)
        {
            AddWater();
        }
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
        if (water + 5 * Time.deltaTime <= maxWater && canWater)
        {
            water += 5 * Time.deltaTime;
            CoinManager.instance.RemoveCoins(1 * Time.deltaTime);
            onWaterChange.Raise(this, water);
        }
        SaveData.SetWater(water);
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
        SaveData.SetWater(water);
        onWaterChange.Raise(this, water);
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

    private IEnumerator WaterCountdown()
    {
        while (true)
        {
            yield return new WaitForSeconds(5f);
            DecreaseWater(1);
        }
    }

    public void ChangeWaterState(Component sender, object data)
    {
        if (data is bool)
        {
            canWater = (bool)data;
        }
    }
}
