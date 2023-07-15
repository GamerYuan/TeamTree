using NCalc;
using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using Random = UnityEngine.Random;

/*
 * Encapsulates a single alphabet in an L-System sentence.
 */
public class Unit
{
    public static Unit EMPTY_UNIT = new Unit(char.MinValue, new Expression[] { });

    // a character that represents this unit.
    public char name;

    // Parameters of this unit.
    public Expression[] unitParameters = { };
    public float[] defaultParameters;
    private float[] evaluatedParameters = { };

    public Unit(char name)
    {
        this.name = name;
    }

    public Unit(char name, Expression[] parameters)
    {
        this.name = name;
        this.unitParameters = parameters;
        this.evaluatedParameters = new float[parameters.Length];
        for(int i = 0; i < evaluatedParameters.Length; i++)
        {
            this.evaluatedParameters[i] = float.MinValue;
        }
    }

    public Unit Clone()
    {
        Unit clone = new Unit(name, (Expression[])unitParameters.Clone());
        return clone;
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
        else if (evaluatedParameters[index] == float.MinValue)
        {
            evaluatedParameters[index] = Convert.ToSingle(unitParameters[index].Evaluate());
        }
        return evaluatedParameters[index];
    }

    //Convert a string of form "A(1,2,3,..)" to a Unit
    public static Unit Parse(string unitString)
    {
        string[] unitComponents = new string[2];
        unitComponents[0] = unitString.Substring(0, Math.Max(unitString.IndexOf("("), 1));
        if (unitString.Contains("("))
            unitComponents[1] = unitString.Substring(unitString.IndexOf("(") + 1, unitString.LastIndexOf(")") - 2);
        else
            unitComponents[1] = "";

        char name = unitComponents[0][0];
        int bracketdepth = 0;
        string parameter = "";
        List<string> parameters = new List<string>();
        for (int i = 0; i < unitComponents[1].Length; i++)
        {
            if (unitComponents[1][i] == '(')
            {
                parameter += unitComponents[1][i];
                bracketdepth++;
            }
            else if (unitComponents[1][i] == ')')
            {
                parameter += unitComponents[1][i];
                bracketdepth--;
            }
            else if (bracketdepth == 0 && unitComponents[1][i] == ',')
            {
                parameters.Add(parameter);
                parameter = "";
            }
            else
                parameter += unitComponents[1][i];
        }
        if (parameter != "")
            parameters.Add(parameter);
        Expression[] expressions = new Expression[parameters.Count];
        for (int i = 0; i < parameters.Count; i++)
        {
            expressions[i] = new Expression(parameters[i]);
            expressions[i].EvaluateFunction += NCalcExtensionFunctions;
        }
        return new Unit(name, expressions);
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
        for (int i = 0; i < unitParameters.Length; i++)
        {
            unitParameters[i].Parameters = paramMap;
            float value = Convert.ToSingle(unitParameters[i].Evaluate());
            unitParameters[i] = new Expression(value.ToString());
            evaluatedParameters[i] = value;
        }
    }

    // Add Random function to Expression
    private static void NCalcExtensionFunctions(string name, FunctionArgs functionArgs)
    {
        if (name == "Random")
        {
            if (functionArgs.Parameters.Count() != 2)
            {
                throw new Exception("Bad random range");
            }
            else
            {
                functionArgs.Result = Random.Range(
                    Convert.ToSingle(functionArgs.Parameters[0].Evaluate()),
                    Convert.ToSingle(functionArgs.Parameters[1].Evaluate()));
            }
        }
    }

    public bool IsLeftBracket()
    {
        return name == '['  || name == '{';
    }

    public bool IsRightBracket()
    {
        return name == ']' || name == '}';
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

    internal char GetName()
    {
        return name;
    }
}
