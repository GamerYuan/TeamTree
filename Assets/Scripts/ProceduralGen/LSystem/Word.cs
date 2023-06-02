using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using UnityEngine;

public class Word 
{
    //List of units in this word
    private List<Unit> units = new List<Unit>();

    public List<Unit> GetUnits() { return units; }
    public int GetNumberOfUnits() { return units.Count; }

    private Word (List<Unit> units)
    {
        this.units = units;
    }
    public Word ApplyRules(RuleSet rules)
    {
        Word newWord = Word.Of(new List<Unit>(){ });
        List<Unit> filteredList =  this.units.Where<Unit>(x => !rules.Ignore.Contains(x.name)).ToList();

        foreach (Unit unit in filteredList)
        {
            Word nextWord = unit.ApplyMatchingRule(rules);
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
        return new Word (units);
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
        outputWord.AssignNeighbours();
        return outputWord;
    }

    public void AssignNeighbours()
    {
        Unit currUnit = this.units[0];
        Unit prevUnit;
        Stack<Unit> unitStack = new Stack<Unit>();
        unitStack.Push(currUnit);
        for (int i = 1; i < this.units.Count; i++)
        {
            prevUnit = unitStack.Peek();
            currUnit = this.units[i];
            StackMod<Unit> stackMod = LSystem.GetStackMod<Unit>(currUnit);
            stackMod.Invoke(currUnit, unitStack);
        }
    }

    public void SetParameters(Dictionary<string, object> paramMap)
    {
        foreach(Unit unit in units)
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
