using UnityEngine;

//Encapsulates a Vector3 Position and Quaternion Rotation
public class OrientedPoint
{
    public Vector3 pos;
    public Quaternion rot;

    public static OrientedPoint ZERO = new OrientedPoint(Vector3.zero, Quaternion.identity);

    public OrientedPoint(Transform transform)
    {
        this.pos = transform.position;
        this.rot = transform.rotation;
    }
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

    override
    public string ToString()
    {
        return pos.ToString() + " " + rot.ToString();
    }

    public bool Equals(OrientedPoint o)
    {
        return this.pos.Equals(o.pos) && this.rot.Equals(o.rot);
    }
    public OrientedPoint MoveForward(float magnitude)
    {
        return new OrientedPoint(pos + (rot * Vector3.forward * magnitude), rot);
    }

    public OrientedPoint Rotate(Quaternion rotation)
    {
        return new OrientedPoint(pos, rot * rotation);
    }

    public OrientedPoint RollToZero()
    {
        Vector3 euler = rot.eulerAngles;
        euler.z = 0;
        Quaternion rotation = Quaternion.Euler(euler);
        return new OrientedPoint(pos, rotation);
    }

    public static OrientedPoint Lerp(OrientedPoint o1, OrientedPoint o2, float t)
    {
        return new OrientedPoint(Vector3.Lerp(o1.pos, o2.pos, t),
            Quaternion.Lerp(o1.rot, o2.rot, t));
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
