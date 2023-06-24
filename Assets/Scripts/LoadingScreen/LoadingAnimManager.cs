using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class LoadingAnimManager : MonoBehaviour
{
    [SerializeField] private VideoPlayer player;
    public static bool done;
    private static int totalFrames;

    void Start()
    {
        player.Pause();
        player.StepForward();
        done = false;
        totalFrames = 121;
    }

    public IEnumerator StepFrames(int frames, float t)
    {
        float tpf = t / frames;
        while(frames > 0)
        {
            player.StepForward();
            --frames;
            --totalFrames;
            if (totalFrames <= 0)
            {
                done = true;
                StopAllCoroutines();
            }
            yield return new WaitForSeconds(tpf);
        }
    }
}
