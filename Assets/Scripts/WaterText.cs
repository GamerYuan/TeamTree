using TMPro;
using UnityEngine;

public class WaterText : MonoBehaviour
{
    [SerializeField] private TMP_Text waterText;
    public void ChangeText(Component sender, object value)
    {
        if (value is float)
        {
            float waterVal = (float)value;
            waterText.text = $"<mark=#4d4d4d88 padding=\"20, 20, 0, 0\">Water: {waterVal:#.##}</mark>";
        }
    }
}
