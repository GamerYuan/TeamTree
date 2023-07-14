using System;
using UnityEngine;

//Encapsulates a vertex in the Tree
public class TreeVert
{
    public OrientedPoint point;

    //Parameters of the vertex.
    public float[] parameters;

    public static int LENGTH = 0;
    public static int THICKNESS = 1;
    public static int COLOR = 2;


    public int id;

    public TreeVert(OrientedPoint point, int id, float[] parameters)
    {
        this.point = point;
        this.id = id;
        this.parameters = parameters;
    }

    public TreeVert MoveForward(float magnitude)
    {
        return new TreeVert(point.MoveForward(magnitude), id, parameters);
    }

    public TreeVert Inflate(float magnitude)
    {
        float[] newparameters = new float[Math.Max(2, parameters.Length)];
        Array.Copy(parameters, newparameters, parameters.Length);
        newparameters[TreeVert.THICKNESS] *= magnitude;

        return new TreeVert(point, id, newparameters);
    }

    public TreeVert SetParams(float[] parameters)
    {
        return new TreeVert(point, id, parameters);
    }

    public TreeVert IncrementColorIndex()
    {
        float[] newparameters = new float[Math.Max(2, parameters.Length)];
        Array.Copy(parameters, newparameters, parameters.Length);
        newparameters[COLOR]++;
        return new TreeVert(point, id, newparameters);
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

    public TreeVert SetRotation(Quaternion rotation)
    {
        OrientedPoint newPoint = new OrientedPoint(point.pos, rotation);
        return new TreeVert(newPoint, id, parameters);
    }

    public TreeVert Rotate(Quaternion rotation)
    {
        return new TreeVert(point.Rotate(rotation), id, parameters);
    }

    public TreeVert RotateTo(Vector3 direction, float magnitude)
    {
        OrientedPoint newPoint;
        if (magnitude > 0)
            newPoint = new OrientedPoint(point.pos, Quaternion.RotateTowards(point.rot, Quaternion.Euler(direction), magnitude));
        else
            newPoint = new OrientedPoint(point.pos, Quaternion.RotateTowards(point.rot, Quaternion.Euler(direction), magnitude));
        return new TreeVert(newPoint, id, parameters);
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
