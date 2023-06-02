using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using NCalc;
using System.Runtime.CompilerServices;
using Unity.Profiling.LowLevel.Unsafe;

/*
* Encapsulates a Production Rule for an L-System.
*/
[CreateAssetMenu]
public class Rule : ScriptableObject
{
    private enum RuleType
    {
        BasicRule,
        ParametricRule,
        LeftContextRule,
        RightContextRule,
        FullContextRule
    }
    // String of Unit to accept as input.
    private string predString = null;

    // contextual Unit strings to accept
    private string leftString = null;

    private string rightString = null;
    // List of parameters
    private string[] leftParamNames = new string[] { };

    private string[] predParamNames = new string[] { };

    private string[] rightParamNames = new string[] { };
    //Condition 
    private Expression condition;
    public string Condition
    {
        set
        {
            condition = new Expression(value);
        }
    }

    // Word to replace input with.
    private Word outputWord;
    public Word OutputWord
    {
        get { return outputWord; }
    }

    private bool SubstituteConditionParams(Dictionary<string, float> parameters)
    {
        if (parameters.Count == 0)
            return true;
        Expression substitutedCondition = condition;

        foreach(string var in predParamNames)
        {
            if (parameters.ContainsKey(var))
                substitutedCondition.Parameters[var] = parameters[var];
            else
                return false;
        }
        foreach (string var in leftParamNames)
        {
            if (parameters.ContainsKey(var))
                substitutedCondition.Parameters[var] = parameters[var];
            else
                return false;
        }
        foreach (string var in rightParamNames)
        {
            if (parameters.ContainsKey(var))
                substitutedCondition.Parameters[var] = parameters[var];
            else
                return false;
        }
        return Convert.ToBoolean(substitutedCondition.Evaluate());
    }

    // tests whether this rule applies to a given unit.
    public bool Accepts(Unit unit)
    {
        // If units mismatch
        if (predString != unit.GetName())
            return false;
        if (leftString != null)
            if (unit.leftUnit == Unit.EMPTY_UNIT || unit.leftUnit.GetName() != leftString)
                return false;
        if (rightString != null)
            if (unit.rightUnit == Unit.EMPTY_UNIT || unit.rightUnit.GetName() != rightString)
                return false;

        // If no. of parameters in units mismatch
        if (predParamNames.Length != unit.GetParams().Length)
            return false;
        if (leftParamNames.Length != unit.leftUnit.GetParams().Length)
            return false;
        if (rightParamNames.Length != unit.rightUnit.GetParams().Length)
            return false;

        // Construct parameter mappings
        Dictionary<string, float> paramMap = new Dictionary<string, float>();
        for (int i = 0; i < predParamNames.Length; i++)
            paramMap.Add(predParamNames[i], (float)unit.GetParam(i));
        for (int i = 0; i < leftParamNames.Length; i++)
                paramMap.Add(leftParamNames[i], (float)unit.leftUnit.GetParam(i));
        for (int i = 0; i < rightParamNames.Length; i++)
                paramMap.Add(rightParamNames[i], (float)unit.rightUnit.GetParam(i));

       return SubstituteConditionParams(paramMap);
    }

    // Parses a String into a Rule. 
    private static readonly Regex sWhitespace = new Regex(@"\s+");
    private static string ReplaceWhitespace(string input, string replacement)
    {
        return sWhitespace.Replace(input, replacement);
    }

    public static Rule ParseRule(string ruleString)
    {
        ruleString = ReplaceWhitespace(ruleString, "");
        ruleString = ruleString.Replace((char)8743, '^');
        ruleString = ruleString.Replace((char)8594, '=');

        Rule rule = (Rule)CreateInstance("Rule");

        RuleType ruleType;

        //Split into predecessor, condition and successor

        string successor = null;
        string predecessor = null;
        string[] predparams = null;
        string predecessorleft = null;
        string[] leftparams = null;
        string predecessorright = null;
        string[] rightparams = null;
        string stringCondition = null;
        Expression condition = null;

        int idx = ruleString.LastIndexOf('=');
        string a = ruleString.Substring(0, idx);
        successor = ruleString.Substring(idx + 1);


        string[] b = a.Split(':', 2, StringSplitOptions.RemoveEmptyEntries);
        if (b.Length == 1)
        {
            predecessor = b[0];
            ruleType = RuleType.BasicRule;
        }
        else
        {
            stringCondition = b[1];
            string[] c = b[0].Split('<', 2, StringSplitOptions.RemoveEmptyEntries);
            if (c.Length == 2)
            {
                predecessorleft = c[0];
                string[] d = c[1].Split('>', 2, StringSplitOptions.RemoveEmptyEntries);
                if (d.Length == 2)
                {
                    predecessor = d[0];
                    predecessorright = d[1];
                    ruleType = RuleType.FullContextRule;
                }
                else
                {
                    predecessor = d[0];
                    ruleType = RuleType.LeftContextRule;
                }
            }
            else
            {

                c = b[0].Split('>', 2, StringSplitOptions.RemoveEmptyEntries);
                if (c.Length == 2)
                {
                    predecessorright = c[1];
                    predecessor = c[0];
                    ruleType = RuleType.RightContextRule;
                }
                else
                {
                    predecessor = c[0];
                    ruleType = RuleType.ParametricRule;
                }
            }
        }

        successor = ReplaceWhitespace(successor, "");
        if(stringCondition != null)
            condition = new Expression(stringCondition);

        switch (ruleType)
        {
            case RuleType.BasicRule:
                {
                    predecessor = ReplaceWhitespace(predecessor, "");
                    successor = ReplaceWhitespace(successor, "");

                    rule.predString = predecessor;
                    rule.outputWord = Word.Parse(successor);
                    return rule;
                }

            case RuleType.ParametricRule:
                {
                    predparams = predecessor.Split(new char[] { ',', '(', ')', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    predecessor = predparams[0];

                    string[] predparameters = new string[predparams.Length - 1];
                    Array.Copy(predparams, 1, predparameters, 0, predparams.Length - 1);

                    rule.predString = predecessor;
                    rule.condition = condition;
                    rule.outputWord = Word.Parse(successor);
                    rule.predParamNames = predparameters;
                    return rule;
                }
            case RuleType.LeftContextRule:
                {
                    leftparams = predecessorleft.Split(new char[] { ',', '(', ')', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    predparams = predecessor.Split(new char[] { ',', '(', ')', ' ' }, StringSplitOptions.RemoveEmptyEntries);

                    predecessor = predparams[0];
                    predecessorleft = leftparams[0];

                    string[] predparameters = new string[predparams.Length - 1];
                    string[] leftparameters = new string[leftparams.Length - 1];
                    Array.Copy(predparams, 1, predparameters, 0, predparams.Length - 1);
                    Array.Copy(leftparams, 1, leftparameters, 0, leftparams.Length - 1);

                    rule.predString = predecessor;
                    rule.leftString = predecessorleft;
                    rule.condition = condition;
                    rule.outputWord = Word.Parse(successor);
                    rule.predParamNames = predparameters;
                    rule.leftParamNames = leftparameters;
                    return rule;
                }
            case RuleType.RightContextRule:
                {
                    rightparams = predecessorright.Split(new char[] { ',', '(', ')', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    predparams = predecessor.Split(new char[] { ',', '(', ')', ' ' }, StringSplitOptions.RemoveEmptyEntries);

                    predecessor = predparams[0];
                    predecessorright = rightparams[0];

                    string[] predparameters = new string[predparams.Length - 1];
                    string[] rightparameters = new string[rightparams.Length - 1];
                    Array.Copy(predparams, 1, predparameters, 0, predparams.Length - 1);
                    Array.Copy(rightparams, 1, rightparameters, 0, rightparams.Length - 1);

                    rule.predString = predecessor;
                    rule.rightString = predecessorright;
                    rule.condition = condition;
                    rule.outputWord = Word.Parse(successor);
                    rule.predParamNames = predparameters;
                    rule.rightParamNames = rightparameters;
                    return rule;
                }

            case RuleType.FullContextRule:
                {
                    leftparams = predecessorleft.Split(new char[] { ',', '(', ')', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    predparams = predecessor.Split(new char[] { ',', '(', ')', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    rightparams = predecessorright.Split(new char[] { ',', '(', ')', ' ' }, StringSplitOptions.RemoveEmptyEntries);

                    predecessor = predparams[0];
                    predecessorleft = leftparams[0];
                    predecessorright = rightparams[0];

                    string[] predparameters = new string[predparams.Length - 1];
                    string[] leftparameters = new string[leftparams.Length - 1];
                    string[] rightparameters = new string[rightparams.Length - 1];
                    Array.Copy(predparams, 1, predparameters, 0, predparams.Length - 1);
                    Array.Copy(leftparams, 1, leftparameters, 0, leftparams.Length - 1);
                    Array.Copy(rightparams, 1, rightparameters, 0, rightparams.Length - 1);

                    rule.predString = predecessor;
                    rule.leftString = predecessorleft;
                    rule.rightString = predecessorright;
                    rule.condition = condition;
                    rule.outputWord = Word.Parse(successor);
                    rule.predParamNames = predparameters;
                    rule.leftParamNames = leftparameters;
                    rule.rightParamNames = rightparameters;

                    return rule;
                }
            default:
                throw new Exception("Parsing Error");
        }
    }



    // returns new replacement List of Units.
    public Word GetOutput(Unit unit)
    {
        Dictionary<string, object> paramMap = new Dictionary<string, object>();
        for (int i = 0; i < predParamNames.Length; i++)
            paramMap.Add(predParamNames[i], unit.GetParam(i));
        for (int i = 0; i < leftParamNames.Length; i++)
            paramMap.Add(leftParamNames[i], unit.leftUnit.GetParam(i));
        for (int i = 0; i < rightParamNames.Length; i++)
            paramMap.Add(rightParamNames[i], unit.rightUnit.GetParam(i));
        outputWord.SetParameters(paramMap);
        return outputWord;
    }
}
