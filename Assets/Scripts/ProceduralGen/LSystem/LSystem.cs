using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LSystem
{   
    private List<Rule> rules;

    private List<Unit> units;

    public LSystem(List<Rule> rules, string axiom)
    {
        this.rules = rules;
        units = Unit.ListFromString(axiom);
    }

    public void setUnits(List<Unit> units)
    {
        this.units = units;
    }

    public void ApplyRules()
    {
        List<Unit> newUnits = new List<Unit>();
        foreach (Unit unit in units)
        {
            newUnits.AddRange(unit.ApplyMatchingRule(rules));
        }
        this.units = newUnits;
    }

    public List<TreeVert> generateTreeVerts(OrientedPoint origin)
    {
        List<TreeVert> newTreeVerts = new List<TreeVert>();
        newTreeVerts.Add(new TreeVert(origin));
        OrientedPoint turtlePos = origin;
        foreach (Unit unit in units)
        {
            turtlePos = unit.ApplyTransformation(turtlePos);
            newTreeVerts.Add(new TreeVert(turtlePos));
        }
        return newTreeVerts;
    }

    public List<int> generateTreeEdges()
    {
        List<int> newTreeEdges = new List<int>();
        for(int i = 0; i < units.Count; i++)
        {
            newTreeEdges.Add(i);
            newTreeEdges.Add(i + 1);
        }
        return newTreeEdges;
    }
}
