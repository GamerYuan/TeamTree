using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RandomEventManager : MonoBehaviour
{
    public static RandomEventManager instance;

    [SerializeField] private List<int> triggerCount = new List<int>();
    [SerializeField] private GameObject minigamePanel, tutorialCanvas;

    private bool[] tutDone;
    private TutorialDataClass[] tutData;
    private GameObject tutorialText, tutorialButton;
    private bool tutorialTriggered, firstLaunch, tutDoneCache;
    private int tutIndex;

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
        firstLaunch = true;
        DontDestroyOnLoad(this);
        TutorialDataArray tutDataArray = JsonUtility.FromJson<TutorialDataArray>(File.ReadAllText($"{Application.streamingAssetsPath}/tutData.json"));
        tutData = tutDataArray.tutData;
        tutorialText = tutorialCanvas.transform.GetChild(0).GetChild(0).gameObject;
        tutorialButton = tutorialCanvas.transform.GetChild(0).GetChild(1).gameObject;
        SceneManager.sceneLoaded += ChangedActiveScene;
    }

    public void SetTutDone(bool[] tutSave)
    {
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
                    Debug.Log(tutorialTriggered);
                    for (int i = 0; i < minigameIndex + 1; i++)
                    {
                        Debug.Log($"Check tutorial {i}");
                        if (tutDone[i])
                        {
                            EnableGame(i);
                        }
                        else if (!tutorialTriggered)
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
            tutorialButton.GetComponent<Button>().onClick.AddListener(() => ButtonClick(currTut.tutorialStageName));
            tutorialButton.transform.GetChild(0).GetComponent<TMP_Text>().text = "Go to stage";
        }
    }

    public void TutorialDone()
    {
        Debug.Log("Tut Done");
        tutDone[tutIndex] = true;
        tutDoneCache = true;
        tutorialTriggered = false;
    }

    private void CompleteTutorial()
    {
        if (tutIndex == 0)
        {
            MinigameTutorial();
        }
        tutDoneCache = false;
    }

    private void ButtonClick(string sceneName)
    {
        DataSerializer.instance.SaveData();
        StageManagerBehaviour.instance.LoadStage(sceneName);
        tutorialCanvas.SetActive(false);
    }

    private void EnableGame(int gameVal)
    {
        Debug.Log($"Enable {gameVal}");
        if (minigamePanel == null)
        {
            minigamePanel = Resources.FindObjectsOfTypeAll<GameObject>().First(x => x.name == "Minigame Panel");
        }
        minigamePanel.transform.GetChild(gameVal).gameObject.SetActive(true);
        if (gameVal == 0)
        {
            GameObject minigameButton = Resources.FindObjectsOfTypeAll<GameObject>().First(x => x.name == "Minigame");
            minigameButton.SetActive(true);
        }
        // enable the minigame
    }

    private void DisableGame()
    {
        if (minigamePanel == null)
        {
            minigamePanel = Resources.FindObjectsOfTypeAll<GameObject>().First(x => x.name == "Minigame Panel");
        }
        for (int i = 0; i < minigamePanel.transform.childCount; i++)
        {
            minigamePanel.transform.GetChild(i).gameObject.SetActive(false);
        }
        GameObject minigameButton = Resources.FindObjectsOfTypeAll<GameObject>().First(x => x.name == "Minigame");
        minigameButton.SetActive(false);
        // disable minigames
    }

    private void MinigameTutorial()
    {

        if (tutorialCanvas == null)
        {
            tutorialCanvas = Resources.FindObjectsOfTypeAll<GameObject>().First(x => x.name == "TutorialCanvas");
            tutorialText = tutorialCanvas.transform.GetChild(0).GetChild(0).gameObject;
            tutorialButton = tutorialCanvas.transform.GetChild(0).GetChild(1).gameObject;
        }
        tutorialText.GetComponent<TMP_Text>().text = "Minigames has been unlocked! You can play Minigames to earn coins that can be used to water your bonsai! More minigames can be unlocked as you encounter them in the future.";
        tutorialCanvas.SetActive(true);
        tutorialButton.GetComponent<Button>().onClick.RemoveAllListeners();
        tutorialButton.GetComponent<Button>().onClick.AddListener(MinigameTutorialButtonClick);
        tutorialButton.transform.GetChild(0).GetComponent<TMP_Text>().text = "Got It!";
    }

    private void MinigameTutorialButtonClick()
    {
        tutorialCanvas.SetActive(false);
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

    private void ChangedActiveScene(Scene scene, LoadSceneMode mode)
    {
        if (scene.name.Equals("SampleScene") && tutDoneCache)
        {
            CompleteTutorial();
        }
    }

    public void ResetTutProgress()
    {
        tutorialTriggered = false;
        tutDone = new bool[triggerCount.Count];
        tutDoneCache = false;
        Debug.Log("Tutorial Progress Reset!");
    }
}
