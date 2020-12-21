using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseUtility : MonoBehaviour
{
    static bool pause = false;
    static float timeScalePrePause = 1f;

    public static void Pause()
    {
        if (!pause)
        {
            pause = true;
            timeScalePrePause = Time.timeScale;
            Time.timeScale = 0;
        }
    }

    public static void Resume()
    {
        if (pause)
        {
            pause = false;
            Time.timeScale = timeScalePrePause;
        }
    }

    public static void PauseAndResume()
    {
        if (pause)
        {
            Resume();
        }
        else
            Pause();
    }
}