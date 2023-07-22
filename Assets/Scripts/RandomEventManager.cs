using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RandomEventManager : TutorialManager
{
    public static RandomEventManager instance;

    private bool tutDoneCache;

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
        filePath = $"{Application.streamingAssetsPath}/tutData.json";
        firstLaunch = true;
        DontDestroyOnLoad(this);
        SceneManager.sceneLoaded += ChangedActiveScene;
        tutorialText = tutorialCanvas.transform.GetChild(0).GetChild(0).gameObject;
        tutorialButton = tutorialCanvas.transform.GetChild(0).GetChild(1).gameObject;
    }

    protected override void Start()
    {
        base.Start();
        StartCoroutine(SetTutDone(SaveData.tutDone));
    }

    protected override IEnumerator SetTutDone(bool[] tutSave)
    {
        yield return base.SetTutDone(tutSave);
        SaveData.SetTutDone(tutDone);
    }

    public override void CheckEvent(Component sender, object data)
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
                    if (tutDone == null || tutDone.Length == 0)
                    {
                        if (rechecking == null) rechecking = StartCoroutine(RecheckEvent((int) data));
                        break;
                    }
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
        SaveData.SetTutDone(tutDone);
    }

    private void ButtonClick(string sceneName)
    {
        DataSerializer.instance.SaveGameAsync();
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

    private void ChangedActiveScene(Scene scene, LoadSceneMode mode)
    {
        if (scene.name.Equals("SampleScene") && tutDoneCache)
        {
            CompleteTutorial();
        }
    }

    public override void ResetTutProgress()
    {
        tutDoneCache = false;
        base.ResetTutProgress();
        SaveData.SetTutDone(tutDone);
    }
}