using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using NCalc;
<<<<<<< Updated upstream
=======
using System.Runtime.CompilerServices;
using Unity.Profiling.LowLevel.Unsafe;
using UnityEditor;
using System.Linq;
using System.Threading;
>>>>>>> Stashed changes

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
<<<<<<< Updated upstream
        foreach(string var in paramnames)
=======

        foreach (string var in predParamNames)
>>>>>>> Stashed changes
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
<<<<<<< Updated upstream
        }

        Dictionary<string,float> paramMap = new Dictionary<string,float>();
        for(int i = 0; i < paramnames.Length; i++)
        {
            paramMap[paramnames[i]] = unit.GetParam(i);
        }
        return this.inputString == unit.GetName() && SubstituteConditionParams(paramMap);
=======

        // If no. of parameters in units mismatch
        if (predParamNames.Length != unit.GetParams().Length)
            return false;
        // Construct parameter mappings
        Dictionary<string, float> paramMap = new Dictionary<string, float>();
        for (int i = 0; i < predParamNames.Length; i++)
            paramMap.Add(predParamNames[i], unit.GetParamOrDefault(i));
        return SubstituteConditionParams(paramMap);
    }

    public bool Accepts(Unit unit, Unit leftContext, Unit[] rightContext)
    {
        if (unit.GetName() != predString)
            return false;

        if (leftString != null && leftContext.GetName() != leftString)
            return false;

        if (predParamNames.Length != unit.GetParams().Length)
            return false;

        if (leftString != null && leftParamNames.Length != leftContext.GetParams().Length)
            return false;

        Dictionary<string, float> paramMap = new Dictionary<string, float>();
        for (int i = 0; i < predParamNames.Length; i++)
            paramMap.Add(predParamNames[i], unit.GetParamOrDefault(i));
        for (int i = 0; i < leftParamNames.Length; i++)
            paramMap.Add(leftParamNames[i], leftContext.GetParamOrDefault(i));

        if (rightString != null)
        {
            foreach (Unit right in rightContext)
            {
                if (right.GetName() == rightString)
                {
                    Dictionary<string, float> rightParams = new Dictionary<string, float>();
                    for (int i = 0; i < rightParamNames.Length; i++)
                        rightParams.Add(rightParamNames[i], right.GetParamOrDefault(i));
                    if (SubstituteConditionParams(paramMap.Concat(rightParams).ToDictionary(x => x.Key, x => x.Value)))
                        return true;
                }
         
            }
            return false;
        }
        return SubstituteConditionParams(paramMap);
>>>>>>> Stashed changes
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

<<<<<<< Updated upstream
            rule.SetRule(predecessor, Word.Parse(successor));
=======
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
        if (stringCondition != null)
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
>>>>>>> Stashed changes
        }
        return rule;
    }

    // returns new replacement List of Units.
    public Word GetOutput(Unit unit, Unit leftContext, Unit[] rightContexts)
    {
<<<<<<< Updated upstream
        Dictionary<string, object> paramMap = new Dictionary<string, object>();
        for (int i = 0; i < paramnames.Length; i++)
        {
            paramMap.Add(paramnames[i], unit.GetParam(i));
        }
        outputWord.SetParameters(paramMap);
=======
        Dictionary<string, float> paramMap = new Dictionary<string, float>();
        for (int i = 0; i < predParamNames.Length; i++)
            paramMap.Add(predParamNames[i], unit.GetParamOrDefault(i));
        for (int i = 0; i < leftParamNames.Length; i++)
            paramMap.Add(leftParamNames[i], leftContext.GetParamOrDefault(i));

        foreach (Unit rightContext in rightContexts)
        {
            Dictionary<string, float> rightParamMap = new Dictionary<string, float>();
            if (rightContext.GetName() == rightString)
            {
                for (int i = 0; i < rightParamNames.Length; i++)
                {
                    rightParamMap.Add(rightParamNames[i], rightContext.GetParamOrDefault(i));
                }
                if (SubstituteConditionParams(paramMap.Concat(rightParamMap).ToDictionary(x => x.Key, x => x.Value)))
                {
                    foreach (KeyValuePair<string, float> rightparam in rightParamMap)
                    {
                        if (paramMap.ContainsKey(rightparam.Key))
                            paramMap[rightparam.Key] = (float)rightparam.Value + (float)paramMap[rightparam.Key];
                        else
                            paramMap.Add(rightparam.Key, rightparam.Value);
                    }
                }
            }
        }

        Dictionary<string, object> objectParamMap = new Dictionary<string, object>();
        foreach (KeyValuePair<string, float> kvp in paramMap)
        {
            objectParamMap.Add(kvp.Key, kvp.Value);
        }
        outputWord.SetParameters(objectParamMap);
>>>>>>> Stashed changes
        return outputWord;
    }
}
