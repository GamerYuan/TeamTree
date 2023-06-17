using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CoinText : MonoBehaviour
{
    [SerializeField] private TMP_Text coinText;
    private float coinAmount;

    void Awake()
    {
        SetCoin(5);
    }

    private void SetCoin(float coin)
    {
        coinAmount = coin;
    }

    public void UpdateCoin(Component sender, object data)
    {
        if (data is float) { 
        
            SetCoin((float)data);
            coinText.text = $"Coin: {coinAmount.ToString("n2")}";
        }
    }
}
