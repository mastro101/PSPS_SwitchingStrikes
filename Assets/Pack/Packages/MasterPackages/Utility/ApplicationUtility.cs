using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ApplicationUtility : Singleton<ApplicationUtility>
{
    public static Action OnApplicationQuitEvent;
    public Action OnApplicationStartFocusEvent;
    public Action OnApplicationEndFocusEvent;
    public Action OnApplicationStartPauseEvent;
    public Action OnApplicationEndPauseEvent;

    static bool isQuitting = false;
    static bool isChangeScene = false;
    static Scene oldScene;

    protected override ApplicationUtility Setup()
    {
        isQuitting = false;
        isChangeScene = false;
        SceneNavigation.OnChangeScene += OnChangeScene;
        SceneManager.sceneLoaded += GetInstance().SceneManager_sceneLoaded;
        SceneManager.sceneUnloaded += SceneManager_sceneUnloaded;
        return this;
    }

    private void SceneManager_sceneUnloaded(Scene arg0)
    {
        isChangeScene = false;
    }

    private void SceneManager_sceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        isChangeScene = false;
    }

    static void OnChangeScene()
    {
        isChangeScene = true;
        oldScene = SceneManager.GetActiveScene();
    }

    private void OnDisable()
    {
        SceneNavigation.OnChangeScene -= OnChangeScene;
    }

    private void OnApplicationQuit()
    {
        isQuitting = true;
        OnApplicationQuitEvent?.Invoke();
    }

    private void OnApplicationFocus(bool focus)
    {
        if (focus)
            GetInstance().OnApplicationStartFocusEvent?.Invoke();
        else
            GetInstance().OnApplicationEndFocusEvent?.Invoke();
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause)
            GetInstance().OnApplicationStartPauseEvent?.Invoke();
        else
            GetInstance().OnApplicationEndPauseEvent?.Invoke();
    }

    public static bool IsQuitting()
    {
        return isQuitting;
    }

    public static GameObject SafeInstantiate(GameObject _gameObject, Vector3 pos, Quaternion rot, Transform parent)
    {
        if (isQuitting || isChangeScene)
        {
            return null;
        }
        if (!instance)
            new GameObject(typeof(ApplicationUtility).Name, typeof(ApplicationUtility));
        return Instantiate(_gameObject, pos, rot, parent);
    }
}