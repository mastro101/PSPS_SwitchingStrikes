using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewEventData", menuName = "ScriptableVar/Event")]
public class EventSignal : ScriptableObject
{
    List<EventListener> listeners = new List<EventListener>();

    public void Invoke()
    {
        for (int i = 0; i < listeners.Count; i++)
        {
            listeners[i]?.Invoke();
        }
    }

    public void AddListener(EventListener listener)
    {
        listeners.Add(listener);
    }

    public void RemoveListener(EventListener listener)
    {
        listeners.Remove(listener);
    }
}