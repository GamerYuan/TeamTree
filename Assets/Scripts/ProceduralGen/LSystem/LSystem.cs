using NCalc;
using System.Collections.Generic;
using System.Linq;
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

    public List<Unit> GetUnits()
    {
        Word nextWord = this.word;
        foreach (RuleSet ruleSet in geometricRules)
        {
            nextWord = nextWord.ApplyRules(ruleSet);
        }
        return nextWord.GetUnits();
    }

    //RuleSet for L-System.
    public RuleSet[] ruleSets;

    //Rules to interpret higher-level symbols as turtle instructions.
    public RuleSet[] geometricRules;

    public void InitAxiom()
    {
        word = Word.Parse(axiomString);
        Debug.Log(this.word);
    }

    public void InitRules()
    {
        foreach (RuleSet ruleSet in ruleSets)
        {
            ruleSet.InitRules();
        }
        foreach (RuleSet ruleSet in geometricRules)
        {
            ruleSet.InitRules();
        }
    }

    public void InitSystem()
    {
        InitAxiom();
        InitRules();
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
                //Debug.Log(ruleSet.name + " : " + nextWord);
            }
        }
        this.word = nextWord;
    }
    // Stack Modifications for L-System
    public static StackMod<T> GetStackMod<T>(Unit u)
    {
        switch (u.name)
        {
            case '[':
                return (x, stack) =>
                {
                    stack.Push(x);
                    return stack;
                };
            case ']':
                return (x, stack) =>
                {
                    stack.Pop();
                    return stack;
                };
            case '{':
                return (x, stack) =>
                {
                    stack.Push(x);
                    return stack;
                };
            case '}':
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

    public void RemoveUnitSubtree(Unit unit)
    {
        foreach (Unit rightunit in word.GetRightContext(word.FindUnit(unit), unit, new char[] { }))
        {
            RemoveUnitSubtree(rightunit);
        }
        word.RemoveUnit(unit);
        CleanEmptyBrackets();
    }

    private void CleanEmptyBrackets()
    {
        word.CleanEmptyBrackets();
    }

    public void ModifyUnit(char name, int paramIndex, float newValue)
    {
        foreach (Unit unit in GetUnits())
        {
            if (unit.name == name)
            {
                unit.unitParameters[paramIndex] = new Expression((unit.GetParamOrDefault(paramIndex) + newValue).ToString());
                unit.MemoizeParam(paramIndex);
                break;
            }
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
