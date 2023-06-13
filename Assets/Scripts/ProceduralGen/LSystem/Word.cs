using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.Rendering;

public class Word
{
    //List of units in this word
    private List<Unit> units = new List<Unit>();

    public List<Unit> GetUnits() { return units; }
    public int GetNumberOfUnits() { return units.Count; }

    private Word(List<Unit> units)
    {
        this.units = units;
    }

    public Word ApplyRules(RuleSet rules)
    {
        Word newWord = Word.Of(new List<Unit>(){ });
        foreach (Unit unit in units)
        {
            Word nextWord = rules.ApplyMatchingRule(unit, this);
            newWord.AddWord(nextWord);
        }
        return newWord;
    }

    private void AddWord(Word word)
    {
        this.units.AddRange(word.units);
    }

    public static Word Of(List<Unit> units)
    {
        return new Word(units);
    }

    public Unit GetLeftContext(Unit unit, string[] ignore)
    {
        int bracketDepth = 0;
        for (int unitIndex = units.IndexOf(unit) - 1; unitIndex > -1; unitIndex--)
        {
            if (units[unitIndex].IsRightBracket())
            {
                bracketDepth++;
            }
            else if (units[unitIndex].IsLeftBracket())
            {
                if (bracketDepth > 0)
                    bracketDepth--;
            }
            else if (bracketDepth <= 0 && !ignore.Contains(units[unitIndex].GetName()))
            {
                return units[unitIndex];
            }
        }
        return Unit.EMPTY_UNIT;
    }

    public Unit[] GetRightContext(Unit unit, string[] ignore)
    {
        List<Unit> rightContexts = new List<Unit>();
        int bracketDepth = 0;
        Stack<int> distance = new Stack<int>();
        distance.Push(0);
        for (int unitIndex = units.IndexOf(unit) + 1; unitIndex < units.Count; unitIndex++)
        {
            Unit nextUnit = units[unitIndex];
            if (bracketDepth < 0)
                break;
            else if (nextUnit.IsLeftBracket())
            {
                distance.Push(distance.Peek());
                bracketDepth++;
            }
            else if (nextUnit.IsRightBracket())
            {
                distance.Pop();
                bracketDepth--;
            }
            else if (distance.Peek() == 0 && !ignore.Contains(nextUnit.GetName()))
            {
                rightContexts.Add(nextUnit);
                distance.Push(distance.Pop() + 1);
            }
        }
        return rightContexts.ToArray();
    }

    public static Word Parse(string word)
    {
        List<Unit> units = new List<Unit>();
        string currUnitString = word[0].ToString();
        bool nextFlag = true;
        for (int i = 1; i < word.Length; i++)
        {
            if (word[i] == '(')
            {
                nextFlag = false;
                currUnitString += word[i].ToString();
            }
            else if (word[i] == ')')
            {

                nextFlag = true;
                currUnitString += word[i].ToString();
            }
            else if (nextFlag)
            {
                units.Add(Unit.Parse(currUnitString));
                currUnitString = "";
                currUnitString += word[i].ToString();
            }
            else
            {
                currUnitString += word[i].ToString();
            }
        }
        Unit currUnit = Unit.Parse(currUnitString);
        units.Add(currUnit);
        Word outputWord = new Word(units);
        return outputWord;
    }

    public void SetParameters(Dictionary<string, object> paramMap)
    {
        foreach (Unit unit in units)
        {
            unit.SetParameters(paramMap);
        }
    }

    override
    public string ToString()
    {
        return string.Join("", units);
    }
}
