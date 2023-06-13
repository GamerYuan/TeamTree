using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        public float Value;
    }

    [SerializeField]
    private List<StringFloatPair> Constants = new List<StringFloatPair>();

    [SerializeField]
    public List<string> Ignore = new List<string>();

    // List of Rules as Strings, in the format (inputChar) ? (outputString)
    [SerializeField]
    public List<string> ruleStrings = new List<string>();

    // List of Rules parsed from Strings.
    public List<Rule> rules => ruleStrings.ConvertAll<string>(x => replaceConstants(x))
                                   .ConvertAll<Rule>(x => Rule.ParseRule(x));
    //Split predecessor into character and parameters


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
        foreach (Rule rule in rules)
        {
            if (rule.Accepts(unit, word.GetLeftContext(unit, Ignore.ToArray()), word.GetRightContext(unit, Ignore.ToArray())))
            {
                return rule.GetOutput(unit, word.GetLeftContext(unit, Ignore.ToArray()), word.GetRightContext(unit, Ignore.ToArray()));
            }
        }

        //Otherwise, return unchanged 
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
