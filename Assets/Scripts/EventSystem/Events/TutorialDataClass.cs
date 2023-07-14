using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class TutorialDataClass
{
    public string tutorialText;
    public string tutorialStageName;

    public override string ToString()
    {
        return tutorialText + " " + tutorialStageName;
    }
}

[Serializable]
public class TutorialDataArray
{
    public TutorialDataClass[] tutData;
}
