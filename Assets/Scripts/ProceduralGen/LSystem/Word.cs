using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Word
{
    //List of units in this word
    private readonly List<Unit> units = new List<Unit>();


    public List<Unit> GetUnits() { return units; }
    public int GetNumberOfUnits() { return units.Count; }

    private Word(List<Unit> units)
    {
        this.units = units;
    }

    public Word ApplyRules(RuleSet rules)
    {
        List<Unit> newWord = new List<Unit>();
            foreach (Unit unit in units)
        {
            newWord.AddRange(rules.ApplyMatchingRule(unit, this).units);
        }
        return Word.Of(newWord);
    }

    public Word Clone()
    {
        return Word.Of(units.ConvertAll<Unit>(x => x.Clone()));
    }

    public void RemoveUnit(Unit unit)
    {
        units.Remove(unit);
    }

    public static Word Of(List<Unit> units)
    {
        return new Word(units);
    }

    public Unit GetLeftContext(int index, Unit unit, char[] ignore)
    {
        int bracketDepth = 0;
        for (int unitIndex = index - 1; unitIndex > -1; unitIndex--)
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

    public int FindUnit(Unit unit)
    {
        for(int i = 0; i < units.Count; i++)
        {
            if (units[i].Equals(unit))
                return i;
        }
        return -1;
    }

    public Unit[] GetRightContext(int index, Unit unit, char[] ignore)
    {
        List<Unit> rightContexts = new List<Unit>();
        if (index < units.Count - 1)
        {
            Unit nextUnit = units[index + 1];
            if (!ignore.Contains(nextUnit.GetName()) && !nextUnit.IsLeftBracket() && !nextUnit.IsRightBracket())
            {
                rightContexts.Add(nextUnit);
                return rightContexts.ToArray();
            }
        }
        int bracketDepth = 0;
        Stack<int> distance = new Stack<int>();
        distance.Push(0);
        for (int unitIndex = index + 1; unitIndex < units.Count; unitIndex++)
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
                if (bracketDepth == 0)
                    break;
                distance.Push(1);
            }
        }
        return rightContexts.ToArray();
    }

    public void CleanEmptyBrackets()
    {
        for (int i = 1; i < units.Count; i++)
        {
            Unit currunit = units[i - 1];
            Unit nextUnit = units[i];
            if (currunit.IsLeftBracket() && nextUnit.IsRightBracket())
            {
                units.Remove(currunit);
                units.Remove(nextUnit);
                i = Mathf.Max(1, i - 1);
            }
        }
    }

    public static Word Parse(string word)
    {
        List<Unit> units = new List<Unit>();
        string currUnitString = word[0].ToString();
        bool nextFlag = true;
        int bracketDepth = 0;
        for (int i = 1; i < word.Length; i++)
        {
            if (word[i] == '(')
            {
                bracketDepth++;
                nextFlag = false;
                currUnitString += word[i].ToString();
            }
            else if (word[i] == ')')
            {
                bracketDepth--;
                if (bracketDepth == 0)
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
