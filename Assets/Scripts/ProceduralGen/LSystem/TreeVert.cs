using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

//Encapsulates a vertex in the Tree
public class TreeVert
{
    public OrientedPoint point;
    public int id;

    public TreeVert(OrientedPoint point, int id)
    {
        this.point = point;
        this.id = id;
    }
    
    public TreeVert moveForward(float magnitude)
    {
        return new TreeVert(point.moveForward(magnitude), id);
    }

    public TreeVert rotate(Quaternion rotation)
    {
        return new TreeVert(point.rotate(rotation), id);
    }

    override
    public string ToString()
    {
        return point.ToString(); 
    }
    
    public TreeVert clone()
    {
        return new TreeVert(point, id);
    }

    

}
