using System.Collections;
using UnityEngine;

public class FlowerPotBehaviour : MonoBehaviour
{
    [SerializeField] private float water;
    [SerializeField] private float maxWater;
    [SerializeField] private float minWater;
    [SerializeField] private MeshRenderer soilMesh;

    public static FlowerPotBehaviour instance;

    private bool startWater, canWater;
    private Material soilMaterial;
    private Color baseColor;

    [Header("Events")]
    [SerializeField] private GameEvent onWaterChange;
    [SerializeField] private GameEvent onWaterStart;

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        canWater = true;
        soilMaterial = soilMesh.material;
        baseColor = soilMaterial.color;
        Debug.Log(baseColor);
    }
    void Start()
    {
        water = SaveData.waterVal;

        StartCoroutine(WaterCall());
        onWaterChange.Raise(this, water);
        if (water <= 50)
        {
            Color newColor = new Color(baseColor.r - (54f/50 * water / 255f), baseColor.g - (29f/50 * water / 255f), baseColor.b);
            soilMaterial.SetColor("_Color", newColor);
        } 
        else
        {
            Color newColor = new Color(61 / 255f, 33 / 255f, baseColor.b);
            soilMaterial.SetColor("_Color", newColor);
        }
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
        onWaterStart.Raise(this, true);
    }

    void OnMouseUp()
    {
        startWater = false;
        onWaterStart.Raise(this, false);
    }

    private void AddWater()
    {
        if (water + 5 * Time.deltaTime <= maxWater && canWater)
        {
            water += 5 * Time.deltaTime;
            CoinManager.instance.RemoveCoins(1 * Time.deltaTime);
            onWaterChange.Raise(this, water);
            if (water <= 50)
            {
                Color newColor = new Color(baseColor.r - (54f / 50 * water / 255f), baseColor.g - (29f / 50 * water / 255f), baseColor.b);
                soilMaterial.SetColor("_Color", newColor);
            }
        } 
        else
        {
            startWater = false;
            onWaterStart.Raise(this, false);
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
        if (water <= 50)
        {
            Color newColor = new Color(baseColor.r - (54f / 50 * water / 255f), baseColor.g - (29f / 50 * water / 255f), baseColor.b);
            soilMaterial.SetColor("_Color", newColor);
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
