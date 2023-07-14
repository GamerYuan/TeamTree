using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

/* Encapsulates a List of Production Rules for use in an L-System.
 *
 */
[CreateAssetMenu]
public class RuleSet : ScriptableObject
{
    [Serializable]
    private class StringFloatPair
    {
        public string String;
        public float Value = 1f;
    }

    [SerializeField]
    private List<StringFloatPair> Constants = new List<StringFloatPair>();

    [SerializeField]
    public List<string> Ignore = new List<string>();

    // List of Rules as Strings, in the format (inputChar) ? (outputString)
    [SerializeField]
    private List<StringFloatPair> ruleStrings = new List<StringFloatPair>();

    // List of Rules parsed from Strings.
    public List<Rule> rules => ruleStrings.ConvertAll<Rule>(x => Rule.ParseRule(replaceConstants(x.String), x.Value));

    private string replaceConstants(string str)
    {
        string output = str;
        foreach (StringFloatPair pair in Constants)
        {
            output = output.Replace(pair.String, pair.Value.ToString());
        }
        return output;
    }

    public Word ApplyMatchingRule(Unit unit, Word word)
    {
        List<Rule> matchingRules = new List<Rule>();

        foreach (Rule rule in rules)
        {
            if (rule.type == Rule.RuleType.BasicRule || rule.type == Rule.RuleType.ParametricRule)
            {
                if (rule.Accepts(unit))
                    matchingRules.Add(rule);
            }
            else if (rule.Accepts(unit, word.GetLeftContext(unit, Ignore.ToArray()), word.GetRightContext(unit, Ignore.ToArray())))
            {
                matchingRules.Add(rule);
            }
        }
        if (matchingRules.Count == 1)
            if (matchingRules[0].type == Rule.RuleType.BasicRule || matchingRules[0].type == Rule.RuleType.ParametricRule)
                return matchingRules[0].GetOutput(unit);
            else
            {
                return matchingRules[0].GetOutput(unit, word.GetLeftContext(unit, Ignore.ToArray()), word.GetRightContext(unit, Ignore.ToArray()));
            }
        else if (matchingRules.Count > 0)
        {
            float random = Random.Range(0f, 1f);
            for (int i = 0; i < matchingRules.Count; i++)
            {
                random -= matchingRules[i].Weight;
                if (random <= 0)
                    if (matchingRules[i].type == Rule.RuleType.BasicRule || matchingRules[i].type == Rule.RuleType.ParametricRule)
                        return matchingRules[i].GetOutput(unit);
                    else
                    {
                        return matchingRules[i].GetOutput(unit, word.GetLeftContext(unit, Ignore.ToArray()), word.GetRightContext(unit, Ignore.ToArray()));
                    }
            }
            return Word.Of(new List<Unit>() { unit });
        }
        else
            return Word.Of(new List<Unit>() { unit });
    }

    private float GetValue(string key)
    {
        for (int i = 0; i < Constants.Count; i++)
        {
            if (Constants[i].String == key)
                return Constants[i].Value;
        }
        throw new System.Exception("Value not defined");
    }

    public List<Rule> GetRules()
    {
        return rules;
    }
}
