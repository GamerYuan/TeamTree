using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomEventManager : MonoBehaviour
{

    [SerializeField] private List<int> triggerCount = new List<int>();
    [SerializeField] private GameObject minigamePanel;

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
                        EnableGame(i);
                    }
                    break;
            }
        }
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
