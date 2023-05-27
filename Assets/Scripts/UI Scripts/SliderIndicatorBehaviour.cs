using MathNet.Numerics;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SliderIndicatorBehaviour : MonoBehaviour
{
    private TMP_Text tmpText;

    void Awake()
    {
        tmpText = GetComponent<TMP_Text>();
    }
    public void ChangeIndicator(Slider slider)
    {
        decimal dec = (decimal) slider.value;
        tmpText.text = dec.Round(2).ToString();
    }
}
