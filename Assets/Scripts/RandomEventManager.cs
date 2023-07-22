using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Networking;

public class RandomEventManager : MonoBehaviour
{
    public static RandomEventManager instance;

    [SerializeField] private List<int> triggerCount = new List<int>();
    [SerializeField] private GameObject minigamePanel, tutorialCanvas;

    private bool[] tutDone;
    private TutorialDataClass[] tutData;
    private GameObject tutorialText, tutorialButton;
    private bool tutorialTriggered, firstLaunch, tutDoneCache, tutLoaded;
    private int tutIndex;
    protected string filePath = $"{Application.streamingAssetsPath}/tutData.json";
    private string jsonString;
    private Coroutine rechecking;

    //[Header("Events")]
    //[SerializeField] private GameEvent onTutorialLoaded;

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
        Debug.Log("Random Event Manager Awoken");
        DontDestroyOnLoad(this);
        SceneManager.sceneLoaded += ChangedActiveScene;
        tutorialText = tutorialCanvas.transform.GetChild(0).GetChild(0).gameObject;
        tutorialButton = tutorialCanvas.transform.GetChild(0).GetChild(1).gameObject;
    }

    void Start()
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
            //onTutorialLoaded.Raise(this, true);
        }
        StartCoroutine(SetTutDone(SaveData.tutDone));
    }

    public IEnumerator SetTutDone(bool[] tutSave)
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
        SaveData.SetTutDone(tutDone);
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
        SaveData.SetTutDone(tutDone);
    }

    private IEnumerator GetTutData()
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
                    //onTutorialLoaded.Raise(this, true);
                    break;
            }
        }
    }

    private IEnumerator RecheckEvent(int data)
    {
        yield return new WaitForSeconds(0.2f);
        CheckEvent(this, data);
        rechecking = null;
    }
}