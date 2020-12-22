using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventListener : MonoBehaviour
{
    [SerializeField] EventSignal signal = null;
    public UnityEvent response;

    private void OnEnable()
    {
        signal.AddListener(this);
    }

    private void OnDisable()
    {
        signal.RemoveListener(this);
    }

    public void Invoke()
    {
        response?.Invoke();
    }
}