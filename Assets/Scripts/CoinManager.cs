using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CoinManager : MonoBehaviour
{
    protected static float currentCoins;
    private int coinsToAdd;
    public static CoinManager instance;
    private GameObject tutorialCanvas;

    [Header("Events")]
    [SerializeField] private GameEvent onCoinChanged;
    [SerializeField] private GameEvent onCoinCanUse;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
        SceneManager.sceneLoaded += ChangedActiveScene;
        DontDestroyOnLoad(gameObject);
    }
    void Start()
    {
        currentCoins = SaveData.coinVal;
        onCoinChanged.Raise(this, currentCoins);
        onCoinCanUse.Raise(this, currentCoins > 0);
    }

    public void AddCoins(int val)
    {
        currentCoins += val;
        coinsToAdd = val;
        Debug.Log($"Add Coins: {coinsToAdd}");
        onCoinChanged.Raise(this, currentCoins);
        if (currentCoins <= 0.02)
        {
            onCoinCanUse.Raise(this, false);
        }
        SaveData.SetCoin(currentCoins);
    }

    public void RemoveCoins(float val)
    {
        if (currentCoins - val <= 0.02f)
        {
            onCoinCanUse.Raise(this, false);
        }
        else
        {
            currentCoins -= val;
            onCoinChanged.Raise(this, currentCoins);
        }
        SaveData.SetCoin(currentCoins);
    }
    public int CalculateCoins(int score, int multiplier)
    {
        int coinCount = Mathf.RoundToInt(score / multiplier);
        return coinCount;
    }
    public float GetCoins()
    {
        return currentCoins;
    }

    private void ChangedActiveScene(Scene scene, LoadSceneMode mode)
    {
        if (scene.name.Equals("SampleScene"))
        {
            onCoinChanged.Raise(this, currentCoins);
            onCoinCanUse.Raise(this, currentCoins > 0);
            if (coinsToAdd > 0)
            {
                ShowCoins();
            }
        }
    }

    private void ShowCoins()
    {
        tutorialCanvas = Resources.FindObjectsOfTypeAll<GameObject>().First(x => x.name == "TutorialCanvas");
        GameObject tutorialText = tutorialCanvas.transform.GetChild(0).GetChild(0).gameObject;
        GameObject tutorialButton = tutorialCanvas.transform.GetChild(0).GetChild(1).gameObject;
        tutorialText.GetComponent<TMP_Text>().text = $"Congratulations! You have earned {coinsToAdd} coins!";
        tutorialCanvas.SetActive(true);
        tutorialButton.GetComponent<Button>().onClick.RemoveAllListeners();
        tutorialButton.GetComponent<Button>().onClick.AddListener(ButtonClick);
        tutorialButton.transform.GetChild(0).GetComponent<TMP_Text>().text = "Hooray!";
    }

    private void ButtonClick()
    {
        tutorialCanvas.SetActive(false);
    }
}
