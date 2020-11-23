using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneNavigation : MonoBehaviour
{
    public static void ChangeScene(string _name)
    {
        SceneManager.LoadScene(_name);
    }

    public static void ChangeScene(int _index)
    {
        SceneManager.LoadScene(_index);
    }

    public static void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public static void QuitGame()
    {
        Application.Quit();
    }
}