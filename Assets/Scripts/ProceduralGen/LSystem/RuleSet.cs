using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Encapsulates a List of Production Rules for use in an L-System.
 *
 */
[CreateAssetMenu]
public class RuleSet : ScriptableObject
{
    // List of Rules as Strings, in the format (inputChar) ? (outputString)
    [SerializeField]
    List<string> ruleStrings = new List<string>();

    // List of Rules parsed from Strings.
<<<<<<< Updated upstream
    List<Rule> rules => ruleStrings.ConvertAll<Rule>(x => Rule.ParseRule(x));
=======
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
>>>>>>> Stashed changes

    public List<Rule> GetRules()
    {
        return rules;
    }
}
