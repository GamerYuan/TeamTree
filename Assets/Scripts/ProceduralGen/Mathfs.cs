using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mathfs : MonoBehaviour
{
    public static Vector2 UnitVectorFromAngle(float angRad)
    {
        return new Vector2(
                Mathf.Cos(angRad),
                Mathf.Sin(angRad)
                );
    }
}
