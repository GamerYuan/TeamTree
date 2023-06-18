using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CoinManager : MonoBehaviour 
{
    protected float currentCoins;
    private float cachedCoins;
    public static CoinManager instance;

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

    public void AddCoins(float val)
    {
        cachedCoins = currentCoins + val;
        Debug.Log($"Add Coins: {cachedCoins}");
        onCoinChanged.Raise(this, cachedCoins);
        if (currentCoins <= 0 && cachedCoins > 0)
        {
            onCoinCanUse.Raise(this, false);
        }
    }

    public void RemoveCoins(float val)
    {
        if (currentCoins - val <= 0)
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
        currentCoins = val;
        onCoinChanged.Raise(this, currentCoins);
        onCoinCanUse.Raise(this, currentCoins > 0);
    }

    private void ChangedActiveScene(Scene scene, LoadSceneMode mode)
    {
        if (scene.name.Equals("SampleScene"))
        {
            if (cachedCoins != 0)
            {
                currentCoins = cachedCoins;
            }
            if (currentCoins != 0)
            {
                onCoinChanged.Raise(this, currentCoins);
                onCoinCanUse.Raise(this, currentCoins > 0);
            }
        }
    }
}
