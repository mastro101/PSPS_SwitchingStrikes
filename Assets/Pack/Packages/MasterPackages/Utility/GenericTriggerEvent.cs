using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class GenericTriggerEvent : MonoBehaviour
{
    [SerializeField] string classNameToTrigger = null;
    [Space]
    [SerializeField] UnityEvent<Collider> OnEnterTrigger = null;
    [SerializeField] UnityEvent<Collider> OnTrigger = null;
    [SerializeField] UnityEvent<Collider> OnExitTrigger = null;

    private void OnTriggerEnter(Collider other)
    {
        if (checkComponent(other))
            OnEnterTrigger?.Invoke(other);
    }

    private void OnTriggerStay(Collider other)
    {
        if (checkComponent(other))
            OnTrigger?.Invoke(other);
    }

    private void OnTriggerExit(Collider other)
    {
        if (checkComponent(other))
            OnExitTrigger?.Invoke(other);
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