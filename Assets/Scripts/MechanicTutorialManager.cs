using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MechanicTutorialManager : TutorialManager
{
    public static MechanicTutorialManager instance;
    private int currUpdate;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Debug.Log(instance);
            Destroy(this);
        }
        else
        {
            instance = this;
        }
        filePath = $"{Application.streamingAssetsPath}/mechanicsTutData.json";
        firstLaunch = true;
        tutorialTriggered = false;
        DontDestroyOnLoad(this);
        tutorialText = tutorialCanvas.transform.GetChild(0).GetChild(0).gameObject;
        tutorialButton = tutorialCanvas.transform.GetChild(0).GetChild(1).gameObject;
    }

    protected override void Start()
    {
        base.Start();
        StartCoroutine(SetTutDone(SaveData.mechanicTutDone));
    }

    protected override IEnumerator SetTutDone(bool[] tutSave)
    {
        yield return base.SetTutDone(tutSave);
        SaveData.SetMechanicTutDone(tutDone);
    }

    public override void CheckEvent(Component sender, object data)
    {
        if (data is int)
        {
            currUpdate = (int)data;
            int mechanicCheck = Check((int)data);
            Debug.Log($"Mechanic Check Tutorial {mechanicCheck} trigger met");
            if (tutDone == null || tutDone.Length == 0)
            {
                if (rechecking == null) rechecking = StartCoroutine(RecheckEvent((int)data));
                return;
            }
            for (int i = 0; i <= mechanicCheck; i++)
            {
                Debug.Log($"Checking Mechanic Tutorial {i}");
                if (tutDone[i])
                {
                    if (tutData[i].tutorialStageName == "Trimming")
                    {
                        Resources.FindObjectsOfTypeAll<GameObject>().First(x => x.name == "TrimButton").SetActive(true);
                    }
                    continue;
                } 
                else if (!tutorialTriggered) 
                {
                    StartTutorial(i);
                }
            }
        }
    }
    private void StartTutorial(int index)
    {
        Debug.Log("Start Tutorial");
        tutorialTriggered = true;
        TutorialDataClass currTut = tutData[index];
        if (currTut != null)
        {
            if (tutorialCanvas == null)
            {
                tutorialCanvas = Resources.FindObjectsOfTypeAll<GameObject>().First(x => x.name == "TutorialCanvas");
                tutorialText = tutorialCanvas.transform.GetChild(0).GetChild(0).gameObject;
                tutorialButton = tutorialCanvas.transform.GetChild(0).GetChild(1).gameObject;
            }
            tutIndex = index;
            tutorialText.GetComponent<TMP_Text>().text = currTut.tutorialText;
            tutorialCanvas.SetActive(true);
            tutorialButton.GetComponent<Button>().onClick.RemoveAllListeners();
            if (currTut.tutorialStageName == "Trimming")
            {
                tutorialButton.GetComponent<Button>().onClick.AddListener(() => TrimClick());
            } 
            else
            {
                tutorialButton.GetComponent<Button>().onClick.AddListener(() => ButtonClick());
            }
            tutorialButton.transform.GetChild(0).GetComponent<TMP_Text>().text = "Got it!";
        }
    }

    private void ButtonClick()
    {
        DataSerializer.instance.SaveGameAsync();
        tutorialCanvas.SetActive(false);
        tutorialTriggered = false;
        tutDone[tutIndex] = true;
        SaveData.SetMechanicTutDone(tutDone);
        CheckEvent(this, currUpdate);
    }
    private void TrimClick()
    {
        ButtonClick();
    }

    public override void ResetTutProgress()
    {
        base.ResetTutProgress();
        SaveData.SetMechanicTutDone(tutDone);
    }
}