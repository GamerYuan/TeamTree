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
        coinText.text = $"<mark=#4d4d4d88 padding=\"20, 20, 0, 0\">Coin: {coinAmount.ToString("n2")}</mark>";
    }

    public void UpdateCoin(Component sender, object data)
    {
        if (data is float) { 
            SetCoin((float)data);
        }
    }
}
