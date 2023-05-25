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
    [SerializeField]
    private string axiomString;

    //Current state of the L-System.
    private Word word = Word.Of(new List<Unit>(){});

    public List<Unit> GetUnits() { return word.GetUnits(); }

    //RuleSet for L-System.
    [SerializeField]
    private RuleSet ruleSet;

    public void InitAxiom()
    {
        word = Word.Parse(axiomString);
        Debug.Log(this.word);
    }

    //Updates the current List of Units by applying the Rules in the RuleSet
    public void ApplyRules()
    {
        Word nextWord = this.word.ApplyRules(ruleSet);
        if (nextWord.GetNumberOfUnits() > 20000)
            Debug.Log("Size Exceeded");
        else
            this.word = nextWord;
    }
}
