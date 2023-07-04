using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.IO;
using UnityEngine;

public class RandomEventManager : MonoBehaviour
{
    public static RandomEventManager instance;

    [SerializeField] private List<int> triggerCount = new List<int>();
    [SerializeField] private GameObject minigamePanel;
    
    private bool[] tutDone;
    private TutorialDataClass[] tutData;

    //[Header("Events")]
    //[SerializeField] private GameEvent onTutorialChange;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        } 
        else
        {
            instance = this;
        }
        Debug.Log(File.ReadAllText($"{Application.streamingAssetsPath}/tutData.json"));
        TutorialDataArray tutDataArray = JsonUtility.FromJson<TutorialDataArray>(File.ReadAllText($"{Application.streamingAssetsPath}/tutData.json"));
        tutData = tutDataArray.tutData;
    }

    public void SetTutDone(bool[] tutSave)
    {
        tutDone = new bool[triggerCount.Count];
        Debug.Log("test1");
        if (tutSave.Length != 0)
        {
            Debug.Log("inside");
            Array.Copy(tutSave, tutDone, tutSave.Length);
        }
    }
    
    public bool[] GetTutDone()
    {
        return tutDone;
    }


    public void CheckEvent(Component sender, object data)
    {
        if (data is int)
        {
            switch (data)
            {
                case int value when value < triggerCount[0]:
                    DisableGame();
                    break;
                default:
                    int minigameIndex = Check((int)data);
                    Debug.Log($"Trigger {minigameIndex} met");
                    for (int i = 0; i <= minigameIndex; i++)
                    {
                        if (tutDone[i])
                        {
                            EnableGame(i);
                        }
                        else
                        {
                            StartTutorial(i);
                        }
                    }
                    break;
            }
        }
    }

    private void StartTutorial(int index)
    {
        TutorialDataClass currTut = tutData[index];
        if (currTut != null) Debug.Log(currTut);
        //onTutorialChange.Raise(this, currTut);
    }

    private void EnableGame(int gameVal)
    {
        minigamePanel.transform.GetChild(gameVal).gameObject.SetActive(true);
        // enable the minigame
    }

    private void DisableGame()
    {
        for (int i = 0; i < minigamePanel.transform.childCount; i++)
        {
            minigamePanel.transform.GetChild(i).gameObject.SetActive(false);
        }
        // disable minigames
    }

    private int Check(int A)
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
}
