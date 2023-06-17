using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneBehaviour : MonoBehaviour
{
    private LoadingAnimManager loadingAnimManager;
    void Awake()
    {
        loadingAnimManager = GetComponent<LoadingAnimManager>();
        Time.timeScale = 1.0f;
    }
    void Start()
    {
        StartCoroutine(LoadSceneAsync());
    }

    private IEnumerator LoadSceneAsync()
    {
        AsyncOperation operation;
        
        if (SceneManager.GetSceneByName(SceneToLoad.sceneToLoad).IsValid())
        {
            operation = SceneManager.LoadSceneAsync(SceneToLoad.sceneToLoad);
        } 
        else
        {
            Debug.LogError($"No valid scene by name: {SceneToLoad.sceneToLoad} found!");
            operation = SceneManager.LoadSceneAsync("SampleScene");
        }
        Debug.Log("I'm outside!");
        operation.allowSceneActivation = false;
        float prevProcces = 0;
        float currProcess = 0;

        while (!operation.isDone && !LoadingAnimManager.done)
        {
            currProcess = operation.progress;
            float diff = currProcess - prevProcces;
            int frames = Mathf.RoundToInt(diff * 122);
            if (frames > 0)
            {
                StartCoroutine(loadingAnimManager.StepFrames(Mathf.Clamp(frames, 1, 20), 0.1f));
            }

            if (currProcess >= 0.9f)
            {
                StartCoroutine(loadingAnimManager.StepFrames(20, 0.1f));
            }
            prevProcces = currProcess;

            yield return new WaitForSeconds(0.1f);
        }

        operation.allowSceneActivation = true;
    }
}
