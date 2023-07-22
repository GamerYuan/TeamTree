using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public abstract class TutorialManager : MonoBehaviour
{
    [SerializeField] protected List<int> triggerCount = new List<int>();
    [SerializeField] protected GameObject minigamePanel, tutorialCanvas;

    protected bool[] tutDone;
    protected TutorialDataClass[] tutData;
    protected GameObject tutorialText, tutorialButton;
    protected bool tutorialTriggered, firstLaunch, tutLoaded;
    protected int tutIndex;
    protected string filePath;
    protected string jsonString;
    protected Coroutine rechecking;

    public abstract void CheckEvent(Component sender, object data);
    protected virtual IEnumerator SetTutDone(bool[] tutSave)
    {
        while (!tutLoaded)
        {
            Debug.Log("Tutorial not loaded, retrying...");
            yield return new WaitForSeconds(0.1f);
        }

        if (firstLaunch)
        {
            Debug.Log("Set Tut Done");
            tutDone = new bool[triggerCount.Count];
            if (tutSave.Length != 0)
            {
                Array.Copy(tutSave, tutDone, tutSave.Length);
            }
            firstLaunch = false;
        }
    }

    protected virtual void Start()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            Debug.Log("Starting Loading Coroutine");
            StartCoroutine(GetTutData());
        }
        else
        {
            jsonString = File.ReadAllText(filePath);
            TutorialDataArray tutDataArray = JsonUtility.FromJson<TutorialDataArray>(jsonString);
            tutData = tutDataArray.tutData;
            tutLoaded = true;
        }
    }

    protected int Check(int A)
    {
        int index = triggerCount.BinarySearch(A);

        if (index >= 0)
        {
            // A is found in the list, return its index
            return index;
        }
        else
        {
            // A is not found, binarySearch returns the bitwise complement
            // of the index of the next larger element, or the length of the list
            index = ~index;

            if (index == 0)
            {
                // A is smaller than all elements in the list
                return -1; // or throw an exception, depending on your requirement
            }
            else if (index == triggerCount.Count)
            {
                // A is larger than all elements in the list
                return index - 1;
            }
            else
            {
                // A falls between two elements in the list
                // return the index of the largest smallest element to A
                return index - 1;
            }
        }
    }
    public virtual void ResetTutProgress()
    {
        tutorialTriggered = false;
        tutDone = new bool[triggerCount.Count];
        Debug.Log("Tutorial Progress Reset!");
    }

    protected IEnumerator RecheckEvent(int data)
    {
        yield return new WaitForSeconds(0.2f);
        CheckEvent(this, data);
        rechecking = null;
    }

    protected IEnumerator GetTutData()
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(filePath))
        {

            yield return webRequest.SendWebRequest();

            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                    Debug.LogError("Connection Error, can't find");
                    break;
                case UnityWebRequest.Result.Success:
                    Debug.Log("Connection Established");
                    jsonString = webRequest.downloadHandler.text;
                    Debug.Log(jsonString);
                    TutorialDataArray tutDataArray = JsonUtility.FromJson<TutorialDataArray>(jsonString);
                    tutData = tutDataArray.tutData;
                    tutLoaded = true;
                    break;
            }
        }
    }
}
