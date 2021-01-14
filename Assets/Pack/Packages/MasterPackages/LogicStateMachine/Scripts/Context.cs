using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Context
{
    public System.Action GoNext;
    public System.Action<string> GoString;
    public System.Action<int> GoInt;
}