using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CoinManager : MonoBehaviour 
{
    protected static float currentCoins;
    private float coinsToAdd;
    public static CoinManager instance;
    private bool firstLaunch;

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
        firstLaunch = true;
        SceneManager.sceneLoaded += ChangedActiveScene;
        DontDestroyOnLoad(gameObject);
    }

    public void AddCoins(float val)
    {
        currentCoins += val;
        coinsToAdd = val;
        Debug.Log($"Add Coins: {currentCoins}");
        onCoinChanged.Raise(this, currentCoins);
        if (currentCoins <= 0.02)
        {
            onCoinCanUse.Raise(this, false);
        }
    }

    public void RemoveCoins(float val)
    {
        if (currentCoins - val <= 0.02f)
        {
            onCoinCanUse.Raise(this, false);
        } else
        {
            currentCoins -= val;
            onCoinChanged.Raise(this, currentCoins);
        }
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
    public void SetCoins(float val)
    {
        if (firstLaunch)
        {
            currentCoins = val;
            onCoinChanged.Raise(this, currentCoins);
            onCoinCanUse.Raise(this, currentCoins > 0);
            firstLaunch = false;
        }
    }

    private void ChangedActiveScene(Scene scene, LoadSceneMode mode)
    {
        if (scene.name.Equals("SampleScene"))
        {
            onCoinChanged.Raise(this, currentCoins);
            onCoinCanUse.Raise(this, currentCoins > 0);
        }
    }
}
