﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GenericStartEvent : MonoBehaviour
{
    [SerializeField] UnityEvent OnStart = null;

    private void Start()
    {
        OnStart?.Invoke();
    }
}