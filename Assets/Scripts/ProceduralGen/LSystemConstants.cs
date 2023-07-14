using System;
using System.Collections.Generic;
using UnityEngine;

/*
 * ScriptableObject for storing Geometric Constant values in an L-System.
 */
[CreateAssetMenu]
public class LSystemConstants : ScriptableObject
{

    public Dictionary<string, bool> ISGEOMETRY = new Dictionary<string, bool>()
    {
            { "B", true },
            { "f", true },
            { "F", true },
            { "+", true },
            { "-", true },
            { "&", true },
            { "^", true },
            { "/", true },
            { "\\", true },
            { "|", true },
            { "!", true },
            { "'", true },
            { "$", true },
            { "[", true },
            { "]", true },
            { "{", true },
            { "}", true }
    };

    //Length of a Segment 
    [Range(0.1f, 5f)]
    public float SegmentLength = 1f;

    //Angle of Rotation
    [Range(1f, 180f)]
    public float Rotation = 60f;

    [Range(0.001f, 0.05f)]
    public float Thickness = 0.001f;

    [SerializeField]
    List<Color> colors = new List<Color>();

    public Color GetColor(int i)
    {
        return colors[i];
    }

    //Alphabet Geometric Transformations for L-System
    public Transformation<TreeVert> GetTransformation(Unit u)
    {
        switch (u.name)
        {
            case "B":
                return x => x.MoveForward(SegmentLength * u.GetParamOrDefault(0));
            case "F":
                return x => x.MoveForward(SegmentLength * u.GetParamOrDefault(0));
            case "f":
                return x => x.MoveForward(SegmentLength * u.GetParamOrDefault(0));
            case "+":
                return x => x.Rotate(Quaternion.Euler(0f, u.GetParamOrDefault(0), 0f));
            case "-":
                return x => x.Rotate(Quaternion.Euler(0f, -u.GetParamOrDefault(0), 0f));
            case "&":
                return x => x.Rotate(Quaternion.Euler(u.GetParamOrDefault(0), 0f, 0f));
            case "^":
                return x => x.Rotate(Quaternion.Euler(-u.GetParamOrDefault(0), 0f, 0f));
            case "/":
                return x => x.Rotate(Quaternion.Euler(0f, 0f, u.GetParamOrDefault(0)));
            case "\\":
                return x => x.Rotate(Quaternion.Euler(0f, 0f, -u.GetParamOrDefault(0)));
            case "|":
                return x => x.Rotate(Quaternion.Euler(0f, 180f, 0f));
            case "!":
                return x => x.Inflate(u.GetParamOrDefault(0));
            case "$":
                return x => x.RotateTo(Vector3.up, u.GetParamOrDefault(0));
            case "'":
                return x => x.IncrementColorIndex();
            default:
                return x => x;
        }
    }


    public bool AddsNode(Unit u)
    {
        switch (u.name)
        {
            case "B":
                return true;
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
            case "B":
                return new float[] { SegmentLength };
            case "f":
                return new float[] { SegmentLength };
            case "F":
                return new float[] { SegmentLength };
            case "G":
                return new float[] { SegmentLength };
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
            case "!":
                return new float[] { 0.707f };
            default:
                return new float[] { 0f, 0f };
        }
    }
}
