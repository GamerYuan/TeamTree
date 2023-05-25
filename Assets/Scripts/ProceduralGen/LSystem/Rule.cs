using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using NCalc;

/*
* Encapsulates a Production Rule for an L-System.
*/
[CreateAssetMenu]
public class Rule : ScriptableObject
{
    // String of Unit to accept as input.
    private string inputString;
    /* contextual units to accept
    public Rule leftcontext;
    public Rule rightcontext;
    */

    // List of parameters
    private string[] paramnames = new string[] {};
    //Condition 
    private Expression condition;

    // Word to replace input with.
    public Word outputWord;
    

    // Basic Rule
    private void SetRule(string inputString, Word outputWord)
    {
        this.inputString = inputString;
        this.outputWord = outputWord;
    }

    // Parametric Rule
    private void SetRule(string inputString, string[] parameters, Expression condition, Word outputWord)
    {
        this.inputString = inputString;
        this.paramnames = parameters;
        this.condition = condition;
        this.outputWord = outputWord;
        
    }

    private bool SubstituteConditionParams(Dictionary<string, float> parameters)
    {
        if (parameters.Count == 0)
            return true;
        Expression substitutedCondition = condition;
        foreach(string var in paramnames)
        {
            substitutedCondition.Parameters[var] = parameters[var];
        }
        return Convert.ToBoolean(substitutedCondition.Evaluate());
    }


    // tests whether this rule applies to a given unit.
    public bool Accepts(Unit unit)
    {
        if(unit.GetParams().Length != paramnames.Length)
        {
            return false;
        }

        Dictionary<string,float> paramMap = new Dictionary<string,float>();
        for(int i = 0; i < paramnames.Length; i++)
        {
            paramMap[paramnames[i]] = unit.GetParam(i);
        }
        return this.inputString == unit.GetName() && SubstituteConditionParams(paramMap);
    }

    /* Parses a String into a Rule. 
     * 
     * Conventions:
     *  A(x, y) : y <= 3 : A(x ? 2, x + y)
     *                      ^Replacement String
     *            ^Condition of unit parameters
     *  ^ Unit to accept
     */
    private static readonly Regex sWhitespace = new Regex(@"\s+");
    private static string ReplaceWhitespace(string input, string replacement)
    {
        return sWhitespace.Replace(input, replacement);
    }

    public static Rule ParseRule(string ruleString)
    {
        ruleString = ReplaceWhitespace(ruleString, "");
        ruleString = ruleString.Replace((char)8743, '^');
        ruleString = ruleString.Replace((char)8594, ':');

        Rule rule = (Rule)CreateInstance("Rule");

        //Split into predecessor, condition and successor
        string[] ruleComponents = ruleString.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);

        //Parametric Rule
        if (ruleComponents.Length == 3)
        {
            string predecessor = ruleComponents[0];
            string condition = ruleComponents[1];
            string successor = ruleComponents[2];

            //Split predecessor into character and parameters
            string[] nameAndParams = predecessor.Split(new char[] { ',', '(', ')', ' '}, StringSplitOptions.RemoveEmptyEntries);
            string name = nameAndParams[0];
            string[] parameters = new string[nameAndParams.Length - 1];
            Array.Copy(nameAndParams, 1, parameters, 0, nameAndParams.Length - 1);
            rule.SetRule(name, parameters, new Expression(condition), Word.Parse(successor));
        }

        //Non-Parametric Rule
        else
        {
            string predecessor = ruleComponents[0];
            predecessor = ReplaceWhitespace(predecessor, "");
            string successor = ruleComponents[1];
            successor = ReplaceWhitespace(successor, "");

            rule.SetRule(predecessor, Word.Parse(successor));
        }
        return rule;
    }

    // returns new replacement List of Units.
    public Word GetOutput(Unit unit)
    {
        Dictionary<string, object> paramMap = new Dictionary<string, object>();
        for (int i = 0; i < paramnames.Length; i++)
        {
            paramMap.Add(paramnames[i], unit.GetParam(i));
        }
        outputWord.SetParameters(paramMap);
        return outputWord;
    }
}
