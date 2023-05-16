using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LSystemConstants : MonoBehaviour
{
    private static float LENGTH_OF_SEGMENT = 1f;

    private static Quaternion POSITIVE_ROTATION = Quaternion.Euler(120f, 0f, 0f);

    private static Quaternion NEGATIVE_ROTATION = Quaternion.Euler(-120f, 0f, 0f);

    public static Transformation GetTransformationFromUnit(Unit u)
    {
        switch(u.character)
        {
            case 'F':
                return x => x.moveForward(LENGTH_OF_SEGMENT);
            case 'G':
                return x => x.moveForward(LENGTH_OF_SEGMENT);   
            case '+':
                return x => x.rotate(POSITIVE_ROTATION);
            case '-':
                return x => x.rotate(NEGATIVE_ROTATION);
            default:
                return x => x;
        }
    }
}
