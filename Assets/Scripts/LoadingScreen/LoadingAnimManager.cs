using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class LoadingAnimManager : MonoBehaviour
{
    [SerializeField] private VideoPlayer player;
    public static bool done;
    private static int totalFrames;

    void Awake()
    {
        player.Pause();
        done = false;
        totalFrames = 122;
    }

    public IEnumerator StepFrames(int frames, float t)
    {
        float tpf = t / frames;
        while(frames > 0)
        {
            player.StepForward();
            --frames;
            --totalFrames;
            Debug.Log(totalFrames);
            if (totalFrames <= 0)
            {
                done = true;
                StopAllCoroutines();
            }
            yield return new WaitForSeconds(tpf);
        }
    }
}
