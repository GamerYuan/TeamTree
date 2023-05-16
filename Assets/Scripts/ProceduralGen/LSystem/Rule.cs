using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu]
public class Rule : ScriptableObject
{
    public char inputChar;
    public string outputString;

    public Unit unit => new Unit(inputChar);
    public List<Unit> output => Unit.ListFromString(outputString);

    public bool Accepts(Unit unit)
    {
        return this.unit.Equals(unit); 
    }
    public List<Unit> getOutput()
    {
        return output;
    }
}
