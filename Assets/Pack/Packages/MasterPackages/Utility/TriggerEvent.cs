using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class TriggerEvent : MonoBehaviour
{
    [SerializeField] string classNameToTrigger = null;
    [Space]
    [SerializeField] UnityEvent OnEnterTrigger = null;
    [SerializeField] UnityEvent OnTrigger = null;
    [SerializeField] UnityEvent OnExitTrigger = null;

    private void OnTriggerEnter(Collider other)
    {
        if (checkComponent(other))
            OnEnterTrigger?.Invoke();
    }

    private void OnTriggerStay(Collider other)
    {
        if (checkComponent(other))
            OnTrigger?.Invoke();
    }

    private void OnTriggerExit(Collider other)
    {
        if (checkComponent(other))
            OnExitTrigger?.Invoke();
    }

    bool checkComponent(Collider c)
    {
        var cl = c.GetComponent(classNameToTrigger);

        if (cl == null)
            return false;
        else
            return true;
    }
}