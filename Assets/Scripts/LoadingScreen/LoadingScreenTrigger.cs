using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingScreenTrigger : MonoBehaviour
{
    public void LoadLoadingScreen(string sceneToLoad)
    {
        SceneToLoad.sceneToLoad = sceneToLoad;
        SceneManager.LoadScene("LoadingScreen");
    }

    public void LoadLoadingScreen(int sceneToLoad)
    {
        SceneToLoad.sceneToLoad = SceneManager.GetSceneByBuildIndex(sceneToLoad).name;
        SceneManager.LoadScene("LoadingScreen");
    }
}
