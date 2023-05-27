using NCalc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Threading;
using UnityEngine;
using Debug = UnityEngine.Debug;

/*
 * Encapsulates a single alphabet in an L-System sentence.
 */
public class Unit
{
    // a character that represents this unit.
    public string name;

    /* Parameters of this unit.
     */
    public Expression[] unitParameters = { };
    public float[] defaultParameters = new float[10];

    public Unit (string name)
    {
        this.name = name;
    }

    public Unit(string name, Expression[] parameters) {
        this.name = name;
        this.unitParameters = parameters;
    }

    public float?[] GetParams()
    {
        float?[] paramValues = new float?[unitParameters.Length];
        for (int i = 0; i < unitParameters.Length; i++)
            paramValues[i] = GetParam(i);
        return paramValues;
    }

    public float? GetParam(int index)
    {
        if (unitParameters.Length - 1 < index)
        {
            return null; 
        }
        return Convert.ToSingle(unitParameters[index].Evaluate());
    }

    public static Unit Parse(string unitString)
    {
        string[] unitComponents = unitString.Split(new char[] { '(', ')' , ','}, StringSplitOptions.RemoveEmptyEntries);
        string name = unitComponents[0];
        Expression[] parameters = new Expression[unitComponents.Length - 1];
        for (int i = 0; i < parameters.Length; i++)
        {
            parameters[i] = new Expression(unitComponents[i + 1]);
        }
        return new Unit(name, parameters);
    }

    // finds first valid rule from a list of rules and returns output List<Unit> from applying rule
    public Word ApplyMatchingRule(RuleSet rules)
    {
        foreach (Rule rule in rules.GetRules())
        {
            if (rule.Accepts(this))
            {
                return rule.GetOutput(this);
            }
        }
        return Word.Of(new List<Unit>() {this});
    }

    public void SetParameters(Dictionary<string, object> paramMap)
    {
        foreach(Expression unitParameter in unitParameters)
        {
            unitParameter.Parameters = paramMap; 
        }
    }

    override 
    public string ToString()
    {
        float[] parameters = new float[unitParameters.Length];
        for (int i = 0; i < parameters.Length; i++)
            parameters[i] = Convert.ToSingle(unitParameters[i].Evaluate());
        if (unitParameters.Length > 0)
            return this.name.ToString() + "(" + string.Join(",",parameters) + ")";
        else
            return this.name.ToString();
    }

    internal string GetName()
    {
        return name;
    }
}
