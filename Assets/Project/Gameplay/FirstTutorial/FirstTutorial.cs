using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstTutorial : MonoBehaviour
{
    [SerializeField] GameObject tutorialPanel;
    bool firstGameplay;

    private void Awake()
    {
        firstGameplay = System.Convert.ToBoolean(PlayerPrefs.GetInt("firstGameplay", System.Convert.ToInt32(true)));

        if (firstGameplay == true)
        {
            tutorialPanel.SetActive(true);
            PauseUtility.Pause();
            PlayerPrefs.SetInt("firstGameplay", System.Convert.ToInt32(false));
        }
    }
}