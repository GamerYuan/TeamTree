using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Word 
{
    //List of units in this word
    private List<Unit> units = new List<Unit>();

    public List<Unit> GetUnits() {return units;}

    private Word (List<Unit> units)
    {
        this.units = units;
    }

    public Word ApplyRules(RuleSet rules)
    {
        Word newWord = Word.Of(new List<Unit>(){ });
        foreach (Unit unit in units)
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
        units.Add(Unit.Parse(currUnitString));
        return new Word(units);
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
