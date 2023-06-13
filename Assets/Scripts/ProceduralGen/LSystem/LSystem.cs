using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * ScriptableObject encapsulating an L-System.
 */
[CreateAssetMenu]
public class LSystem : ScriptableObject
{
    //Starting state of the L-System.
    public string axiomString;

    //Current state of the L-System.
    private Word word = Word.Of(new List<Unit>() { });

    public List<Unit> GetUnits() { return word.GetUnits(); }

    //RuleSet for L-System.
    public RuleSet[] ruleSets;

    public void InitAxiom()
    {
        word = Word.Parse(axiomString);
        Debug.Log(this.word);
    }

    //Updates the current List of Units by applying the Rules in the RuleSet
    public void ApplyRules()
    {
        Word nextWord = this.word;
        foreach (RuleSet ruleSet in ruleSets)
        {
            nextWord = nextWord.ApplyRules(ruleSet);
            if (nextWord.GetNumberOfUnits() > 50000)
            {
                Debug.Log("Size Exceeded");
            }
            else
            {
                Debug.Log(nextWord);
            }
        }
        this.word = nextWord;
    }
    // Stack Modifications for L-System
    public static StackMod<T> GetStackMod<T>(Unit u)
    {
        switch (u.name)
        {
            case "[":
                return (x, stack) =>
                {
                    stack.Push(x);
                    return stack;
                };
            case "]":
                return (x, stack) =>
                {
                    stack.Pop();
                    return stack;
                };
            default:
                return (x, stack) =>
                {
                    stack.Pop();
                    stack.Push(x);
                    return stack;
                };

        }
    }

    public void LoadString(string str)
    {
        word = Word.Parse(str);
    }

    override public string ToString()
    {
        return word.ToString();
    }
}
