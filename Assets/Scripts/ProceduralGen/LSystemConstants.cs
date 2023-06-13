using System;
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
            default:
                return x => x;
        }
    }
    public StackMod<TreeVert> GetStackMod(Unit u)
    {
        switch(u.name)
        {
            case "[":
                return (x, stack) =>
                {
                    stack.Push(x);
                    return stack;
                };
            case "]":
                return (x, stack) =>
                {
                    stack.Pop();
                    return stack;
                };
            default:
                return (x, stack) =>
                {
                    stack.Pop();
                    stack.Push(x);
                    return stack;
                };

        }
    }
    public float[] GetDefaultParams(string name)
    {
        switch (name)
        {
            case "F":
                return new float[] { SegmentLength, 0.05f };
            case "G":
                return new float[] { SegmentLength, 0.05f };
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
