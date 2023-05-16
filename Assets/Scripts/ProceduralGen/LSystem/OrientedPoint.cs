using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrientedPoint
{
    public Vector3 pos;
    public Quaternion rot;

    public OrientedPoint(Vector3 pos, Quaternion rot)
    {
        this.pos = pos;
        this.rot = rot;
    }

    public OrientedPoint(Vector3 pos, Vector3 normal)
    {
        this.pos = pos;
        this.rot = Quaternion.LookRotation(normal);
    }

    public OrientedPoint moveForward(float magnitude)
    {
        return new OrientedPoint(pos + (rot * Vector3.forward * magnitude), rot);
    }

    public OrientedPoint rotate(Quaternion rotation)
    {
        return new OrientedPoint(pos, rot * rotation);
    }

    public Vector3 LocalToWorldPos(Vector3 localSpacePos)
    {
        return pos + rot * localSpacePos;
    }

    public Vector3 LocalToWorldVec(Vector3 localSpacePos)
    {
        return rot * localSpacePos;
    }
}
