using System;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using UnityEditor;
using UnityEngine;

/*
 * ScriptableObject for storing Geometric Constant values in an L-System.
 */
[CreateAssetMenu]
public class LSystemConstants : ScriptableObject
{
    //Length of a Segment 
    [Range(0.1f, 5f)]
    public float SegmentLength = 1f;

    //Angle of Rotation
    [Range(1f, 180f)]
    public float Rotation = 60f;
    //Alphabet Geometric Transformations for L-System
    public Transformation<TreeVert> GetTransformation(Unit u)
    {
        switch(u.name)
        {
            case "B":
                return x => x.SetParams(u.GetParams())
                             .Rotate(Quaternion.Euler(0f, u.GetParamOrDefault(2), 0f))
                             .MoveForward(u.GetParamOrDefault(0));
            case "F":
                return x => x.MoveForward(SegmentLength * u.GetParamOrDefault(0));
            case "f":
                return x => x.MoveForward(SegmentLength * u.GetParamOrDefault(0));
            case "G":
                return x => x.MoveForward(SegmentLength * u.GetParamOrDefault(0)).Inflate(u.GetParamOrDefault(1));
            case "+":
                return x => x.Rotate(Quaternion.Euler(0f, Rotation, 0f));
            case "-":
                return x => x.Rotate(Quaternion.Euler(0f, -Rotation, 0f));
            case "&":
                return x => x.Rotate(Quaternion.Euler(Rotation, 0f, 0f));
            case "^":
                return x => x.Rotate(Quaternion.Euler(-Rotation, 0f, 0f));
            case "/":
                return x => x.Rotate(Quaternion.Euler(0f, 0f, Rotation));
            case "\\":
                return x => x.Rotate(Quaternion.Euler(0f, 0f, -Rotation));
            case "|":
                return x => x.Rotate(Quaternion.Euler(0f, 180f, 0f));
            case "!":
                return x => x.Inflate(0.707f);
            default:
                return x => x;
        }
    }

    public bool AddsNode(Unit u)
    {
        switch (u.name)
        {
            case "F":
                return true;
            case "f":
                return true;
            default:
                return false;
        }
    }

    public float[] GetDefaultParams(string name)
    {
        switch (name)
        {
            case "f":
                return new float[] { SegmentLength };
            case "F":
                return new float[] { SegmentLength, 0.05f };
            case "G":
                return new float[] { SegmentLength, 0.02f };
            case "+":
                return new float[] { Rotation };
            case "-":
                return new float[] { Rotation };
            case "&":
                return new float[] { Rotation };
            case "^":
                return new float[] { Rotation };
            case "/":
                return new float[] { Rotation };
            case "\\":
                return new float[] { Rotation };
            case "|":
                return new float[] { 180f };
            default:
                return new float[] { 0f, 0f };
        }
    } 
}
