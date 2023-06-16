using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingScreenTrigger : MonoBehaviour
{
    public void LoadLoadingScreen(string sceneToLoad)
    {
        SceneToLoad.sceneToLoad = sceneToLoad;
        SceneManager.LoadScene("LoadingScreen");
    }
}
