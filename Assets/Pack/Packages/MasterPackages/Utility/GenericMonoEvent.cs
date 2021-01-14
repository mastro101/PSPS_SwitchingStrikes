using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GenericMonoEvent : MonoBehaviour
{
    [SerializeField] UnityEvent OnEnableUE = null;
    [SerializeField] UnityEvent OnDisableUE = null;
    [SerializeField] UnityEvent OnAwakeUE = null;
    [SerializeField] UnityEvent OnStartUE = null;
    [SerializeField] UnityEvent OnDestroyUE = null;

    private void OnEnable()
    {
        OnEnableUE?.Invoke();
    }

    private void OnDisable()
    {
        OnDisableUE?.Invoke();
    }

    private void Awake()
    {
        OnAwakeUE?.Invoke();
    }

    private void Start()
    {
        OnStartUE?.Invoke();
    }

    private void OnDestroy()
    {
        OnDestroyUE?.Invoke();
    }
}