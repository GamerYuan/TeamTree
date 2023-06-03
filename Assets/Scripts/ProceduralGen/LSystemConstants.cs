using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * ScriptableObject for storing Geometric Constant values in an L-System.
 */
[CreateAssetMenu]
public class LSystemConstants : ScriptableObject
{
    //Length of a Segment 
    [SerializeField]
    [Range(0.1f, 5f)]
    private float SegmentLength;

    //Angle of Rotation
    [SerializeField]
    [Range(1f, 180f)]
    private float Rotation;

    //Alphabet Geometric Transformations for L-System
    public Transformation<TreeVert> GetTransformation(Unit u)
    {
        switch(u.name)
        {
            case "F":
                return x => x.moveForward(SegmentLength * u.GetParam(0));
            case "G":
                return x => x.moveForward(SegmentLength * u.GetParam(0));   
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

    //Alphabet Stack Modifications for L-System
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
}