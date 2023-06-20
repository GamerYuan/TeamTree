using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrunkNode : Node
{
    private float thickness;
    private Mesh2d crossSection;

    public TrunkNode(OrientedPoint op) : base(op)
    {

    }
}
