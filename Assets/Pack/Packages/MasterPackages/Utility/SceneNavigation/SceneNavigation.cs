using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneNavigation : MonoBehaviour
{
    public static System.Action OnChangeScene;

    public static void ChangeScene(string _name)
    {
        OnChangeScene?.Invoke();
        SceneManager.LoadScene(_name);
    }

    public static void ChangeScene(int _index)
    {
        OnChangeScene?.Invoke();
        SceneManager.LoadScene(_index);
    }

    public static void LoadSceneAdditive(string _name)
    {
        SceneManager.LoadSceneAsync(_name, LoadSceneMode.Additive);
    }

    public static void LoadSceneAdditive(int _index)
    {
        SceneManager.LoadSceneAsync(_index, LoadSceneMode.Additive);
    }

    public static void ReloadScene()
    {
        ChangeScene(SceneManager.GetActiveScene().name);
    }

    public static void QuitGame()
    {
        Application.Quit();
    }
}