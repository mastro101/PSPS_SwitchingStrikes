using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "floatValue" , menuName = "ScriptableVar/float", order = 1)]
public class FloatData : ScriptableVar<float>
{
    public void Add(float f)
    {
        SetValue(value + f);
    }
}