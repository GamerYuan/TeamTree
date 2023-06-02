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
    private List<StringFloatPair> Constants;
    // List of Rules as Strings, in the format (inputChar) ? (outputString)
    public List<string> ruleStrings = new List<string>();

    [SerializeField]
    public List<string> Ignore = new List<string>();

    // List of Rules parsed from Strings.
    List<Rule> rules => ruleStrings.ConvertAll<string>(x => replaceConstants(x))
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

    private float GetValue(string key)
    {
        for(int i = 0; i < Constants.Count; i++)
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
