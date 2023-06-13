using NCalc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Threading;
using UnityEditor.Search;
using UnityEngine;
using Debug = UnityEngine.Debug;

/*
 * Encapsulates a single alphabet in an L-System sentence.
 */
public class Unit
{
    public static Unit EMPTY_UNIT = new Unit("EMPTY_UNIT", new Expression[] { });

    // a character that represents this unit.
    public string name;

    // Parameters of this unit.
    public Expression[] unitParameters = { };
    public float[] defaultParameters;

    public Unit(string name)
    {
        this.name = name;
    }

    public Unit(string name, Expression[] parameters)
    {
        this.name = name;
        this.unitParameters = parameters;
    }

    //Assign Default values to each parameter
    public void SetDefaults(float[] defaults)
    {
        this.defaultParameters = defaults;
    }

    //Get the full list of Parameters
    public float[] GetParams()
    {
        float[] paramValues = new float[unitParameters.Length];
        for (int i = 0; i < unitParameters.Length; i++)
            paramValues[i] = GetParamOrDefault(i);
        return paramValues;
    }

    //Get a specific parameter
    public float GetParamOrDefault(int index)
    {
        if (index > unitParameters.Length - 1)
        {
            return defaultParameters[index];
        }
        return Convert.ToSingle(unitParameters[index].Evaluate());
    }

    //Convert a string of form "A(1,2,3,..)" to a Unit
    public static Unit Parse(string unitString)
    {
        string[] unitComponents = unitString.Split(new char[] { '(', ')', ',' }, StringSplitOptions.RemoveEmptyEntries);
        string name = unitComponents[0];
        Expression[] parameters = new Expression[unitComponents.Length - 1];
        for (int i = 0; i < parameters.Length; i++)
        {
            parameters[i] = new Expression(unitComponents[i + 1]);
        }
        return new Unit(name, parameters);
    }

    public Dictionary<string, float> ConstructParamMap(string[] paramNames)
    {
        Dictionary<string, float> paramMap = new Dictionary<string, float>();
        for (int i = 0; i < paramNames.Length; i++)
        {
            paramMap.Add(paramNames[i], GetParamOrDefault(i));
        }
        return paramMap;
    }

    // Assign numeric values to expression parameters in this unit
    public void SetParameters(Dictionary<string, object> paramMap)
    {
        foreach (Expression unitParameter in unitParameters)
        {
            unitParameter.Parameters = paramMap;
        }
    }

    public bool IsLeftBracket()
    {
        return name == "[" || name == "{";
    }

    public bool IsRightBracket()
    {
        return name == "]" || name == "}";
    }

    override
    public string ToString()
    {
        float[] parameters = new float[unitParameters.Length];
        for (int i = 0; i < parameters.Length; i++)
            parameters[i] = Convert.ToSingle(unitParameters[i].Evaluate());
        if (unitParameters.Length > 0)
            return this.name.ToString() + "(" + string.Join(",", parameters) + ")";
        else
            return this.name.ToString();
    }

    internal string GetName()
    {
        return name;
    }
}
