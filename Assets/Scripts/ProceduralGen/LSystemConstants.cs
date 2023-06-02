using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

/*
 * ScriptableObject for storing Geometric Constant values in an L-System.
 */
[CreateAssetMenu]
public class LSystemConstants : ScriptableObject
{
    //Length of a Segment 
    [Range(0.1f, 5f)]
    public float SegmentLength;

    //Angle of Rotation
    [Range(1f, 180f)]
    public float Rotation;

    //Alphabet Geometric Transformations for L-System
    public Transformation<TreeVert> GetTransformation(Unit u)
    {
        float? param = u.GetParam(0);
        if(param == null)
        {
            param = 1;
        }

        float parameter = param.Value;

        switch(u.name)
        {
            case "F":
                return x => x.moveForward(SegmentLength * parameter);
            case "+":
                return x => x.rotate(Quaternion.Euler(0f, Rotation, 0f));
            case "-":
                return x => x.rotate(Quaternion.Euler(0f, -Rotation, 0f));
            case "&":
                return x => x.rotate(Quaternion.Euler(Rotation, 0f, 0f));
            case "^":
                return x => x.rotate(Quaternion.Euler(-Rotation, 0f, 0f));
            case "/":
                return x => x.rotate(Quaternion.Euler(0f, 0f, Rotation));
            case "\\":
                return x => x.rotate(Quaternion.Euler(0f, 0f, -Rotation));
            case "|":
                return x => x.rotate(Quaternion.Euler(0f, 180f, 0f));
            default:
                return x => x;
        }
    }

    
}
