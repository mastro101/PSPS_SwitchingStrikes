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
        set 
        {
            _value = value;
            OnChangeValue?.Invoke(_value);
        }
    }

    public virtual void SetDefault() { value = default; }

    public T GetValue()
    {
        return value;
    }

    public Action<T> OnChangeValue;

    private void OnEnable()
    {
        this.hideFlags = HideFlags.DontUnloadUnusedAsset;
    }

    private void OnDisable()
    {
        if (Application.isPlaying)
            Destroy(this);
    }
}