using System;
using System.Collections.Generic;
using System.Linq;
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
    public List<char> Ignore = new List<char>();

    // List of Rules as Strings, in the format (inputChar) ? (outputString)
    [SerializeField]
    private List<StringFloatPair> ruleStrings = new List<StringFloatPair>();

    // List of Rules parsed from Strings.
    public List<Rule> rules = new List<Rule>();

    public void InitRules()
    {
       rules = ruleStrings.ConvertAll<Rule>(x => Rule.ParseRule(replaceConstants(x.String), x.Value)).ToList();
    }

    private string replaceConstants(string str)
    {
        string output = str;
        foreach (StringFloatPair pair in Constants)
        {
            output = output.Replace(pair.String, pair.Value.ToString());
        }
        return output;
    }

    public Word ApplyMatchingRule(Unit unit, Word word, int index)
    {
        List<Rule> matchingRules = new List<Rule>();
        foreach (Rule rule in rules)
        {
            if (rule.Type == Rule.RuleType.BasicRule || rule.Type == Rule.RuleType.ParametricRule)
            {
                if (rule.Accepts(unit))
                    matchingRules.Add(rule);
            }
            else
            {
                if (rule.Accepts(unit, word.GetLeftContext(index, unit, Ignore.ToArray()), word.GetRightContext(index, unit, Ignore.ToArray())))
                {
                    matchingRules.Add(rule);
                }
            }
        }
        if (matchingRules.Count == 1)
        {
            if (matchingRules[0].Type == Rule.RuleType.BasicRule || matchingRules[0].Type == Rule.RuleType.ParametricRule)
            {
                Word output = matchingRules[0].GetOutput(unit);
                return output;
            }
            else
            {
                return matchingRules[0].GetOutput(unit, word.GetLeftContext(index, unit, Ignore.ToArray()), word.GetRightContext(index, unit, Ignore.ToArray()));
            }
        }
        else if (matchingRules.Count > 1)
        {
            float random = Random.Range(0f, 1f);
            for (int i = 0; i < matchingRules.Count; i++)
            {
                random -= matchingRules[i].Weight;
                if (random <= 0)
                    if (matchingRules[i].Type == Rule.RuleType.BasicRule || matchingRules[i].Type == Rule.RuleType.ParametricRule)
                    {
                        return matchingRules[i].GetOutput(unit);
                    }
                    else
                    {
                        return matchingRules[i].GetOutput(unit, word.GetLeftContext(index, unit, Ignore.ToArray()), word.GetRightContext(index, unit, Ignore.ToArray()));
                    }
            }
            return Word.Of(new List<Unit>() { unit });
        }
        else
        {
            return Word.Of(new List<Unit>() { unit });
        }
    }

    public List<Rule> GetRules()
    {
        return rules;
    }
}
