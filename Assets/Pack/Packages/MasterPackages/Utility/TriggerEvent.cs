using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class TriggerEvent<T> : MonoBehaviour
{
    [SerializeField] UnityEvent<T> OnEnterTrigger = null;
    [SerializeField] UnityEvent<T> OnTrigger = null;
    [SerializeField] UnityEvent<T> OnExitTrigger = null;

    private void OnTriggerEnter(Collider other)
    {
        CheckComponent(other, OnEnterTrigger);
    }

    private void OnTriggerStay(Collider other)
    {
        CheckComponent(other, OnTrigger);
    }

    private void OnTriggerExit(Collider other)
    {
        CheckComponent(other, OnExitTrigger);
    }

    void InvokeEvent(UnityEvent<T> ue, T t)
    {
        ue?.Invoke(t);
    }

    void CheckComponent(Collider c, UnityEvent<T> ue)
    {
        T cl = c.GetComponent<T>();

        if (cl == null)
            return;
        else
            InvokeEvent(ue, cl);
    }
}