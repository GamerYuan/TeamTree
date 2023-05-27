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
    public List<string> ruleStrings = new List<string>();

    // List of Rules parsed from Strings.
    List<Rule> rules => ruleStrings.ConvertAll<Rule>(x => Rule.ParseRule(x));

    public List<Rule> GetRules()
    {
        return rules; 
    }
}
