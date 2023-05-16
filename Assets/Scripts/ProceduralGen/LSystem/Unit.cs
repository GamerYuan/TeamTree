using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit
{
    public char character;
    private Transformation transformation = x => x;

    public Unit (char character)
    {
        this.character = character;
        transformation = LSystemConstants.GetTransformationFromUnit(this);
    }

    public static List<Unit> ListFromString(string sentence)
    {
        List<Unit> list = new List<Unit>();
        foreach(char c in sentence)
        {
            list.Add(new Unit(c));
        }
        return list;
    }

    public bool Equals(Unit u)
    {
        return u.character == character;
    }

    public List<Unit> ApplyMatchingRule(List<Rule> rules)
    {
        foreach (Rule rule in rules)
        {
            if (rule.Accepts(this))
            {
                return rule.getOutput();
            }
        }
        return new List<Unit> { this };
    }

    public OrientedPoint ApplyTransformation(OrientedPoint start)
    {
        return transformation.Invoke(start);
    }
}
