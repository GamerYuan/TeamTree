using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

//Encapsulates a vertex in the Tree
public class TreeVert
{
    public OrientedPoint point;

    //Parameters of the vertex.
    public float[] parameters;

    public static int LENGTH = 0;
    public static int THICKNESS = 1;


    public int id;

    public TreeVert(OrientedPoint point, int id, float[] parameters)
    {
        this.point = point;
        this.id = id;
        this.parameters = parameters;
    }

    public TreeVert MoveForward(float magnitude)
    {
        return new TreeVert(point.moveForward(magnitude), id, parameters);
    }

    public TreeVert Inflate(float magnitude)
    {
        float[] newparameters = new float[Math.Max(2,parameters.Length)];
        Array.Copy(parameters, newparameters, parameters.Length);
        newparameters[1] *= magnitude;
        return new TreeVert(point, id, newparameters);
    }

    public TreeVert SetParams(float[] parameters)
    {
        return new TreeVert(point, id, parameters);
    }

    public float GetParam(int i)
    {
        if (i < parameters.Length)
        {
            return parameters[i];
        }
        else
        {
            return 0.05f;
        }
    }

    public TreeVert Rotate(Quaternion rotation)
    {
        return new TreeVert(point.rotate(rotation), id, parameters);
    }

    override
    public string ToString()
    {
        return point.ToString() + "(" + string.Join(",", parameters) + ")";
    }

    public TreeVert Clone()
    {
        return new TreeVert(point, id, parameters);
    }
}
