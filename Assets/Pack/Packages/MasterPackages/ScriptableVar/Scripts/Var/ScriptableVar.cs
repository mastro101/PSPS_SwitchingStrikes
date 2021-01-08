using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ScriptableVar<T> : ScriptableObject
{
    [SerializeField] T _value = default;
    public T value 
    { 
        get { return _value; } 
        private set 
        { 
            _value = value; 
            OnChangeValue?.Invoke(_value); 
        }
    }

    public void SetValue(T _var)
    {
        value = _var;
    }

    public T GetValue()
    {
        return value;
    }

    public Action<T> OnChangeValue;
}